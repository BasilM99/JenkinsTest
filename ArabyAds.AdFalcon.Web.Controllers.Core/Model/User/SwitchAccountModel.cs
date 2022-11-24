using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.User
{
    public class SwitchAccountModel
    {
        public int NormalId { get; set; }
        public int DSPId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public int ChosenId { get; set; }
        public string returnUrl { get; set; }

    }
}
