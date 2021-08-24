//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/22/2021 4:59 PM
//
// CreateUserRequestModel.cs
//
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASMR.Core.Enumerations;

namespace ASMR.Core.RequestModel
{
    public class CreateUserRequestModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the first name.")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the last name.")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "EmailAddress")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the email address.")]
        [EmailAddress(ErrorMessage = "The email address you provided is not a valid email address.")]
        public string EmailAddress { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the username.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the password.")]
        /*[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]*/
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", 
            ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character).")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirmation")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the password confirmation.")]
        [Compare("Password", ErrorMessage = "The password confirmation does not match.")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "Please assign minimal a role.")]
        public IEnumerable<Role> Roles { get; set; }
    }
}
