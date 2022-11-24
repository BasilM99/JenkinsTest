using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Core.Party
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
