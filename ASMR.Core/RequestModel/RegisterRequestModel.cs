//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 2:17 PM
//
// RegisterRequestModel.cs
//
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
    public class RegisterRequestModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in your first name.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in your last name.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choose your username.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
    }
}
