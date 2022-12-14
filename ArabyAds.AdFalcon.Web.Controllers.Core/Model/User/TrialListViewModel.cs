using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.User
{
    public class TrialViewModel
    {
        public IEnumerable<TrialDto> Items { get; set; }
        public long TotalCount { get; set; }



    }

    public class TrialListViewModel
    {
        public string ViewName { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public bool IsPrimaryUser { get; set; }

        public int RootId { get; set; }

        public int Total { get; set; }

        public int ObjectRootTypeId { get; set; }

        public IEnumerable<SelectListItem> ObjectTypes { get; set; }
        public IEnumerable<TrialDto> Items { get; set; }

    }

}
