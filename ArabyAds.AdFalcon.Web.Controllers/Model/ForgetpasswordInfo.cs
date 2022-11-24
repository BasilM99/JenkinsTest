using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.DataAnnotations;


namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    public class ForgetpasswordInfo
    {
        [Required()]
        [Email(ResourceName = "InvalidEmail")]
        public string Email { get; set; }
    }
}
