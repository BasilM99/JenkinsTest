using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Deals
{
    public class Filter
    {
        public bool showArchived { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int[] checkedRecords { get; set; }
        public List<int> ExchangeFiltred { get; set; }

        public int? page { get; set; }
        public int? size { get; set; }
        public int? Accountid { get; set; }

        public string PublisherName { get; set; }
        public string Name { get; set; }
        public int? AdvertiserId { get; set; }
        public int? ExchangeId { get; set; }

        public int? PublisherId { get; set; }

        public List<int> AdFormat { get; set; }

        public bool ShowAdvertiser { get; set; }
        public bool OnlyMyGlobal { get; set; }
        public bool IsGlobal { get; set; }


        public List<int> Countries { get; set; }
        public int? AdvertiserAccountId { get; set; }
        public List<int> AdSize { get; set; }

    }
}
