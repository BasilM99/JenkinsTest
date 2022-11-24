using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.User
{
    public class InvitationViewModel
    {
        public IEnumerable<InvitationDto> Items { get; set; }
        public long TotalCount { get; set; }
    }

    public class InvitationListViewModel
    {
        public int id { get; set; }
        public string invitationcode { get; set; }
        public DateTime InvitationDate { get; set; }
        public string EmailAddress { get; set; }
        public int accountid { get; set; }

        public virtual bool IsAccepted { get; set; }

        public virtual string IsAcceptedString
        {
            get
            {
                return IsAccepted ? ResourcesUtilities.GetResource("Yes","Global") : ResourcesUtilities.GetResource("No", "Global");
            }
            set { }
        }


    }

}
