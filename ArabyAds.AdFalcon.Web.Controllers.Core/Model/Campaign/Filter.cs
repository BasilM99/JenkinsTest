using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class Filter
    {
        public int?TypeId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int[] checkedRecords { get; set; }
        public int? AdvertiserId { get; set; }
        public int? Id { get; set; }
        public int? AdvertiserAccountId { get; set; }
        public int? page { get; set; }
        public int? size { get; set; }
        public string Name { get; set; }
        public string BundleId { get; set; }
        public string Domain { get; set; }
        public bool showRoot { get; set; }
        public bool showArchived { get; set; }
        public bool showAccountAdv { get; set; }
        public bool showGlobal { get; set; }
    }
}
