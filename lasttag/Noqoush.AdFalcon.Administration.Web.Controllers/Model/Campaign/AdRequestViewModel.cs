using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Campaign
{
    public class AdRequestViewModel
    {
        public AdRequestTargetingDtoResultDto AllItems { set; get; }
        public AdRequestDialogViewModel AdRequestDialog { set; get; }

    }

    public class AdRequestDialogViewModel
    {
        public IList<SelectListItem> Types { set; get; }
        public IList<SelectListItem> Platforms { set; get; }
        public IList<SelectListItem> Versions { set; get; }

    }



}
