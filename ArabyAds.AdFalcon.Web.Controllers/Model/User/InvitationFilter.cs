using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Model.User
{
    public class InvitationFilter
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? page { get; set; }
        public int? size { get; set; }

        public int Type { get; set; }
    }
}
