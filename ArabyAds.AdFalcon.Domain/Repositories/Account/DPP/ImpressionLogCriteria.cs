using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.DPP;
using ArabyAds.AdFalcon.Domain.Model.Account.DPP;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    public class ImpressionLogCriteria : CriteriaBase<ArabyAds.AdFalcon.Domain.Model.Account.DPP.ImpressionLog>
    {
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public int DataFromInt { get; set; }
        public int DataToInt { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }

        public ImpressionLogType Type { get; set; }
        public string Name { get; set; }
        public int? DataProviderId { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.ImpressionLogCriteria Commoncr)
        {






            DataFromInt = Commoncr.DataFromInt;

            DataToInt = Commoncr.DataToInt;



            Type = Commoncr.Type;
            Name = Commoncr.Name;

            DataProviderId = Commoncr.DataProviderId;

        
            Page = Commoncr.Page;

            Size = Commoncr.Size;


            DataFrom = Commoncr.DataFrom;
            DataTo = Commoncr.DataTo;

            Name = Commoncr.Name;


        }




        public override Expression<Func<ArabyAds.AdFalcon.Domain.Model.Account.DPP.ImpressionLog, bool>> GetExpression()
        {
            if (Name == null)
            {
                Name = string.Empty;
            }
            Expression<Func<ArabyAds.AdFalcon.Domain.Model.Account.DPP.ImpressionLog, bool>> filter =
                (c => !c.IsDeleted
                && (string.IsNullOrEmpty(Name) || c.Provider.Name.ToLower().Contains(Name.ToLower()))
               && (DataProviderId.HasValue)
                && (DataProviderId == c.Provider.ID)
             && ((DataFrom == null && DataTo == null) || (c.Day >= DataFromInt && c.Day <= DataToInt))

             &&((Type==ImpressionLogType.None) ||(c.Type==Type))
               );
            return filter;
        }

        public override Func<ArabyAds.AdFalcon.Domain.Model.Account.DPP.ImpressionLog, bool> GetWhere()
        {
            Func<ArabyAds.AdFalcon.Domain.Model.Account.DPP.ImpressionLog, bool> filter =
                (c => !c.IsDeleted
                && (DataProviderId == c.Provider.ID)
                && (DataProviderId.HasValue)
                && (string.IsNullOrEmpty(Name) || c.Provider.Name.ToLower().Contains(Name.ToLower()))
                && ((DataFrom == null && DataTo == null) || (c.Day >= DataFromInt && c.Day <= DataToInt))
                     && ((Type == ImpressionLogType.None) || (c.Type == Type))
                );
            return filter;
        }
    }

}
