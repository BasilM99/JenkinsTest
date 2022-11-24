using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Campaign
{
    public class AdRequestViewModel
    {
        public int adGroupId { set; get; }

        
        public AdRequestTargetingDtoResultDto AllItems { set; get; }
        public AdRequestDialogViewModel AdRequestDialog { set; get; }

    }

    public class AdRequestDialogViewModel
    {
        public int adGroupId { set; get; }
        public IList<SelectListItem> Types { set; get; }
        public IList<SelectListItem> Platforms { set; get; }
        public IList<SelectListItem> Versions { set; get; }

    }



}
