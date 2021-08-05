//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/22/2021 5:01 PM
//
// UpdateUserPasswordRequestModel.cs
//
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
    public class UpdateUserPasswordRequestModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the password.")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character).")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirmation")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the password confirmation.")]
        [Compare("Password", ErrorMessage = "The password confirmation does not match.")]
        public string PasswordConfirmation { get; set; }
    }
}
