using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    public class LoginInfo
    {
        [Required(ResourceName= "EnterUserName")]
        public string  Username { get; set; }

        [Required(ResourceName = "EnterPassword")]
        public string Password { get; set; }
    }
}
