using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;

namespace Noqoush.AdFalcon.Domain.Repositories.Account.PMP
{
   
    public class PMPDealCriteria : CriteriaBase<PMPDeal>
    {
   
        public IList<int> Geographies { get; set; }

      
        public IList<int> AdFormats { get; set; }
        public List<int> ExchangeFiltred { get; set; }
        public IList<int> AdSizes { get; set; }
        public string PublisherName { get; set; }
        public bool? Archived { get; set; }
        public int AccountId { get; set; }
        public int? PublisherId { get; set; }
        public int? AdvertiserId { get; set; }


        public int? AdvertiserAccountId { get; set; }
        public int? ExchangeId { get; set; }
        public int? userId { get; set; }
        public bool IsPrimaryUser { get; set; }

        public bool IsGlobal { get; set; }

        public bool OnlyGlobal { get; set; }

        public bool ShowAdvertiser { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DealType Type { get; set; }
        //public int? StatusId { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public int? AppSiteId { get; set; }
        public string Name { get; set; }
        public bool OnlyMyGlobal { get; set; }

        public PMPDealCriteria()
        {
          //  Type = DealType.PrivateAuction;
        }


        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Account.PMP.PMPDealCriteria Commoncr)
        {


         


            Archived = Commoncr.Archived;
             PublisherName = Commoncr.PublisherName;
             AdSizes = Commoncr.AdSizes;
             ExchangeFiltred = Commoncr.ExchangeFiltred;
             AdFormats = Commoncr.AdFormats;





        AdvertiserId = Commoncr.AdvertiserId;
             PublisherId = Commoncr.PublisherId;
             AccountId = Commoncr.AccountId;
             IsPrimaryUser = Commoncr.IsPrimaryUser;
             IsGlobal = Commoncr.IsGlobal;
             Geographies = Commoncr.Geographies;



        AdvertiserAccountId = Commoncr.AdvertiserAccountId;
             ExchangeId = Commoncr.ExchangeId;
             userId = Commoncr.userId;
             IsPrimaryUser = Commoncr.IsPrimaryUser;
             IsGlobal = Commoncr.IsGlobal;
             OnlyGlobal = Commoncr.OnlyGlobal;




        OnlyMyGlobal = Commoncr.OnlyMyGlobal;

            AppSiteId = Commoncr.AppSiteId;



            Type = Commoncr.Type;
            Name = Commoncr.Name;

            ShowAdvertiser = Commoncr.ShowAdvertiser;


            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;

            Name = Commoncr.Name;


        }
        public override Expression<Func<PMPDeal, bool>> GetExpression()
        {

            if (Name == null)
            {
                Name = string.Empty;
            }
            Expression<Func<PMPDeal, bool>> filter =
                c=>((Archived.HasValue ||  c.IsDeleted == Archived  )
                //&& c.Ty == CampaignType
               && (!AdvertiserId.HasValue || c.Advertiser.ID == AdvertiserId)
                  && (!AdvertiserAccountId.HasValue || c.AdvertiserAccount.ID == AdvertiserAccountId)
                && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name.ToLower()))
                );
            return filter;
        }

        public override Func<PMPDeal, bool> GetWhere()
        {
            // Func<Model.Campaign.Campaign, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId) && c.Account.ID == AccountId);
            Func<PMPDeal, bool> filter = (c => c.IsDeleted == false && c.Account.ID == AccountId /*&& (!userId.HasValue || c.User.ID == userId)*/  && (!AdvertiserId.HasValue || c.Advertiser.ID == AdvertiserId) && (!AdvertiserAccountId.HasValue || c.AdvertiserAccount.ID == AdvertiserAccountId));
            return filter;
        }
    }

}
