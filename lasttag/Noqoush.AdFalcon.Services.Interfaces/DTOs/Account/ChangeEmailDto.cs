using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    public class ChangeEmailDto
    {
        public string Hashing { get; set; }
        public bool duplicateBuyer { get; set; }
        public string ActivationCode { get; set; }

        public int? buyerId { get; set; }
    }
}
