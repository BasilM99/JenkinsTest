using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    public class ChangePasswordInfo
    {

        [RegularExpression(@"((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        [StringLength(16,6, ResourceName = "InvalidPassword")]
        [Required]
        public string Password { get; set; }

        [RemoteAttribute("CheckPassword","checkpassword","user")]
        [Required]
        public string CurrentPassword { get; set; }

        //[RegularExpression(@"((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        //[StringLength(16, 6, ResourceName = "InvalidPassword")]
        [CompareAttribute("Password",ResourceName = "PasswordAndConfirmPasswordMatch")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
