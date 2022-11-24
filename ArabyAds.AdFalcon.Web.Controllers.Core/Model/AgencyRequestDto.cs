using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{
    public class ContactUsDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Email]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

    }
    public class AgencyRequestDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        [Email]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Company { get; set; }

        public string Address { get; set; }

        [Required]
        public string Message { get; set; }

    }
}
