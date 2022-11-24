using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    public class ResetPasswordInfo
    {


        [RegularExpression(@"((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        [StringLength(16, 6, ResourceName = "InvalidPassword")]
        [Required]
        //[DataMember]
        public string Password { get; set; }

        //[RegularExpression(@"((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ResourceName = "ComplexPass")]
        //[StringLength(16, 6, ResourceName = "InvalidPassword")]
        [CompareAttribute("Password", ResourceName = "PasswordAndConfirmPasswordMatch")]
        [Required]
        public string ConfirmPassword { get; set; }

        //[DataMember]
        public string EmailAddress { get; set; }

       // [DataMember]
        public string Token { get; set; }
    }
}
