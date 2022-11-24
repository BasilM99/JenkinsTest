using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal
{

    public class PMPDealListViewModelBase : ListViewModelBase
    {
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
    public class PMPDealListViewModel : PMPDealListViewModelBase
    {
        public IEnumerable<PMPDealDto> Items { get; set; }

        public int PublisherId { get; set; }

        public int TotalCount { get; set; }

        public int ExchangeId { get; set; }
        public int? advertiserId { get; set; }


        public string DealName { get; set; }

        public string PublisherName { get; set; }
        public bool fromTargeting { get; set; }


        public string ExchangeName { get; set; }
        public bool PreventEdit { get; set; }

    }

}
