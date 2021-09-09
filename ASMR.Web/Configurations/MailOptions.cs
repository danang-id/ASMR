namespace ASMR.Web.Configurations
{
	public class MailTemplates
	{
		public string EmailAddressConfirmation { get; set; }
		public string EmailAddressChanged { get; set; }
		public string RegistrationPendingApproval { get; set; }
		public string RegistrationRejected { get; set; }
		public string Welcome { get; set; }
		public string PasswordReset { get; set; }
	}
	
	public class MailOptions
	{
		public string ApiKey { get; set; }
		public string SenderAddress { get; set; }
		public string SenderName { get; set; }
		public string ReplyToAddress { get; set; }
		public string ContactListId { get; set; }
		public MailTemplates Templates { get; set; }
	}
}