using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{
    public class LoginInfo
    {
        [Required(ResourceName= "EnterUserName")]
        public string  Username { get; set; }

        [Required(ResourceName = "EnterPassword")]
        public string Password { get; set; }
        public bool rememberMe { get; set; }
        public string returnUrl  { get; set; }
    }
}
