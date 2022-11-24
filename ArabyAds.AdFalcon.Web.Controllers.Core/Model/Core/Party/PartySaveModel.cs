using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Core.Party
{
    public class PartySaveModel
    {
        public string type { get; set; }
    }

    public class EmployeePartySaveModel : PartySaveModel
    {
        public EmployeeDto PartyDto { get; set; }
    }
    public class BusinessPartnerPartySaveModel : PartySaveModel
    {
        public BusinessPartnerDto PartyDto { get; set; }
    
    }
}
