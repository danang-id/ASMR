//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 1:55 PM
//
// SignInRequestModel.cs
//
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
    public class SignInRequestModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in your username.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}
