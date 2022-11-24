using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.AppSite
{
    public class SubAppsite : IEntity<int>
    {
        public virtual int ID { get; set; }

        public virtual string SubPublisherId { get; set; }

        public virtual string SubPublisherName { get; set; }

        public virtual string SubPublisherUrl { get; set; }

        public virtual string SubPublisherMarketId { get; set; }

        public virtual AppSite AppSite { get; set; }
        public virtual string GetDescription()
        {
            return string.Format("{0}:{1}", AppSite.Name, SubPublisherName);
        }
        //public virtual string GetDescription()
        //{
        //    return this.ToString();
        //}
        public virtual bool IsDeleted { get; set; }

    }


   
    public class SubAppsiteTransfomer
    {
     
        public int Id { get; set; }
  
        public string SubPublisherId { get; set; }
      
        public string SubPublisherName { get; set; }
      
        public string SubPublisherUrl { get; set; }
   
        public string SubPublisherMarketId { get; set; }
     
        public int AppSiteId { get; set; }
        public string ExchangeName { get; set; }
        public int ExchangeId { get; set; }
        public int AccountId { get; set; }
        public string AppSiteName { get; set; }

    }
}
