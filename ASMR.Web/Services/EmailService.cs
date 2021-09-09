using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Web.Configurations;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Errors.Model;
using SendGrid.Helpers.Mail;

namespace ASMR.Web.Services
{
    internal class SendGridContact
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    internal class SendGridContactSearchResponse
    {
        [JsonPropertyName("result")]
        public IEnumerable<SendGridContact> Results { get; set; }
    }
    
    public interface IEmailService
    {
        public Task<SendGridErrorResponse> UpsertContactAsync(User user);
        
        public Task<SendGridErrorResponse> DeleteContactAsync(string emailAddress);
        
        public Task<SendGridErrorResponse> SendMailAsync(User user, string subject, string plainTextContent, 
            string htmlContent);

        public Task<SendGridErrorResponse> SendMailAsync(string emailAddress, string name, string subject, 
            string plainTextContent, string htmlContent);

        public Task<SendGridErrorResponse> SendMailAsync(EmailAddress to, string subject, string plainTextContent, 
            string htmlContent);
        
        public Task<SendGridErrorResponse> SendTemplateMailAsync(User user, string templateId,
            object dynamicTemplateData);

        public Task<SendGridErrorResponse> SendTemplateMailAsync(string emailAddress, string name, string templateId,
            object dynamicTemplateData);

        public Task<SendGridErrorResponse> SendTemplateMailAsync(EmailAddress to, string templateId, 
            object dynamicTemplateData);

        public Task<SendGridErrorResponse> SendEmailAddressConfirmationMailAsync(User user, 
            string emailAddressConfirmationUrl);

        public Task<SendGridErrorResponse> SendEmailAddressChangedMailAsync(User user, string oldEmailAddress);
        
        public Task<SendGridErrorResponse> SendRegistrationRejectedMailAsync(User user);
        
        public Task<SendGridErrorResponse> SendRegistrationPendingApprovalMailAsync(User user);

        public Task<SendGridErrorResponse> SendWelcomeMailAsync(User user, string role, 
            string signInUrl = "https://asmr.hamzahjundi.me/dashboard");
        
        public Task<SendGridErrorResponse> SendPasswordResetMailAsync(User user, string passwordResetUrl);
    }

    public class EmailService : IEmailService
    {
        private readonly MailOptions _options;
        
        private readonly SendGridClient _sendGridClient;
        private readonly EmailAddress _senderEmailAddress;
        private readonly EmailAddress _replyToEmailAddress;

        public EmailService(IOptions<MailOptions> options)
        {
            _options = options.Value;
            
            var sendGridClientOptions = new SendGridClientOptions
            {
                ApiKey = _options.ApiKey,
                HttpErrorAsException = true
            };
            
            _sendGridClient = new SendGridClient(sendGridClientOptions);
            _senderEmailAddress = new EmailAddress(_options.SenderAddress, _options.SenderName);
            _replyToEmailAddress = new EmailAddress(_options.ReplyToAddress, _options.SenderName);
        }

        public async Task<SendGridErrorResponse> UpsertContactAsync(User user)
        {
            try
            {
                var contactListIds = new Collection<string> { _options.ContactListId };
                var contacts = new Collection<SendGridContact>
                {
                    new()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    }
                };
                var data = JsonSerializer.Serialize(new { list_ids = contactListIds, contacts });
                await _sendGridClient.RequestAsync(BaseClient.Method.PUT, urlPath: "marketing/contacts", 
                    requestBody: data);
                return null;
            }
            catch (Exception exception)
            {
                return JsonSerializer.Deserialize<SendGridErrorResponse>(exception.Message);
            }
        }

        public async Task<SendGridErrorResponse> DeleteContactAsync(string emailAddress)
        {
            try
            {
                var searchData = JsonSerializer.Serialize(new { query = $"email={emailAddress}" });
                var response = await _sendGridClient.RequestAsync(BaseClient.Method.POST, urlPath: "marketing/contacts/search", 
                    requestBody: searchData);
                var responseContent = await response.Body.ReadAsByteArrayAsync();
                var searchResult = JsonSerializer.Deserialize<SendGridContactSearchResponse>(responseContent);
                if (searchResult?.Results is null || !searchResult.Results.Any())
                {
                    return null;
                }

                var contactIds = searchResult.Results.Select(contact => contact.Id);
                var deleteData = JsonSerializer.Serialize(new { ids = string.Join(",", contactIds) });
                await _sendGridClient.RequestAsync(BaseClient.Method.DELETE, urlPath: "marketing/contacts", 
                    requestBody: deleteData);
                return null;
            }
            catch (Exception exception)
            {
                return JsonSerializer.Deserialize<SendGridErrorResponse>(exception.Message);
            }
        }

        public Task<SendGridErrorResponse> SendMailAsync(User user, string subject, string plainTextContent, string htmlContent)
        {
            return SendMailAsync(user.Email, $"{user.FirstName} {user.LastName}", subject, plainTextContent, htmlContent);
        }
        
        public Task<SendGridErrorResponse> SendMailAsync(string emailAddress, string name, string subject, string plainTextContent, string htmlContent)
        {
            var recipientEmailAddress = new EmailAddress(emailAddress, name);
            return SendMailAsync(recipientEmailAddress, subject, plainTextContent, htmlContent);
        }
        
        public async Task<SendGridErrorResponse> SendMailAsync(EmailAddress to, string subject, string plainTextContent, string htmlContent)
        {
            try
            {
                var message = MailHelper.CreateSingleEmail(_senderEmailAddress, 
                    to, 
                    subject, 
                    plainTextContent, 
                    htmlContent);
                message.ReplyTo = _replyToEmailAddress;
                
                await _sendGridClient.SendEmailAsync(message);
                return null;
            }
            catch (Exception exception)
            {
                return JsonSerializer.Deserialize<SendGridErrorResponse>(exception.Message);
            }
        }

        public Task<SendGridErrorResponse> SendTemplateMailAsync(User user, string templateId, 
            object dynamicTemplateData)
        {
            return SendTemplateMailAsync(user.Email, $"{user.FirstName} {user.LastName}", templateId, 
                dynamicTemplateData);
        }

        public Task<SendGridErrorResponse> SendTemplateMailAsync(string emailAddress, string name, string templateId, 
            object dynamicTemplateData)
        {
            var recipientEmailAddress = new EmailAddress(emailAddress, name);
            return SendTemplateMailAsync(recipientEmailAddress, templateId, dynamicTemplateData);
        }

        public async Task<SendGridErrorResponse> SendTemplateMailAsync(EmailAddress to, string templateId, 
            object dynamicTemplateData)
        {
            try
            {
                var message = MailHelper.CreateSingleTemplateEmail(_senderEmailAddress, 
                    to, 
                    templateId, 
                    dynamicTemplateData);
                message.ReplyTo = _replyToEmailAddress;
                
                await _sendGridClient.SendEmailAsync(message);
                return null;
            }
            catch (Exception exception)
            {
                return JsonSerializer.Deserialize<SendGridErrorResponse>(exception.Message);
            }
        }

        public Task<SendGridErrorResponse> SendEmailAddressConfirmationMailAsync(User user,
            string emailAddressConfirmationUrl)
        {
            return SendTemplateMailAsync(user, _options.Templates.EmailAddressConfirmation, new
            {
                name = user.FirstName,
                emailAddressConfirmationUrl
            });
        }

        public Task<SendGridErrorResponse> SendEmailAddressChangedMailAsync(User user, string oldEmailAddress)
        {
            return SendTemplateMailAsync(oldEmailAddress, $"{user.FirstName} {user.LastName}",
                _options.Templates.EmailAddressChanged, 
                new
                {
                    name = user.FirstName,
                    newEmailAddress = user.Email
                });
        }

        public Task<SendGridErrorResponse> SendRegistrationPendingApprovalMailAsync(User user)
        {
            return SendTemplateMailAsync(user, _options.Templates.RegistrationPendingApproval, new
            {
                name = user.FirstName
            });
        }
        
        public Task<SendGridErrorResponse> SendRegistrationRejectedMailAsync(User user)
        {
            return SendTemplateMailAsync(user, _options.Templates.RegistrationRejected, new
            {
                name = user.FirstName
            });
        }

        public Task<SendGridErrorResponse> SendWelcomeMailAsync(User user, string role, 
            string signInUrl = "https://asmr.hamzahjundi.me/dashboard")
        {
            return SendTemplateMailAsync(user, _options.Templates.Welcome, new
            {
                name = user.FirstName,
                role, 
                signInUrl
            });
        }

        public Task<SendGridErrorResponse> SendPasswordResetMailAsync(User user, string passwordResetUrl)
        {
            return SendTemplateMailAsync(user, _options.Templates.PasswordReset, new
            {
                name = user.FirstName, 
                passwordResetUrl
            });
        }
    }
}
