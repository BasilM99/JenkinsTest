using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.Framework.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
   
    public class AdvertiserCriteria : CriteriaBase<Advertiser>
    {
        public string Value { get; set; }
        public string Culture { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserCriteria Commoncr)
        {
            Value = Commoncr.Value;
         

            Culture = Commoncr.Culture;

            

            Page = Commoncr.Page;
            Size = Commoncr.Size;





        }
        public  Expression<Func<Advertiser, bool>> GetExpressionForName()
        {
            Expression<Func<Advertiser, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(Culture))
            {
                filter = C => C.Name.Values.Any(v => v.Value.ToLower().Equals(Value.ToLower()) && v.Culture == Culture);
            }
            else if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = C => C.Name.Values.Any(v => v.Value.ToLower().Equals(Value.ToLower()));
            }
            return filter;
        }
        public override Expression<Func<Advertiser, bool>> GetExpression()
        {
            Expression<Func<Advertiser, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(Culture))
            {
                filter = C => C.Name.Values.Any(v => v.Value.Contains(Value) && v.Culture == Culture);
            }
            else if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = C=>C.Name.Values.Any(v => v.Value.Contains(Value)  );
            }
            return filter;
        }

        public override Func<Advertiser, bool> GetWhere()
        {
            Func<Advertiser, bool> filter = c => true;
            if (!string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(Culture))
            {
                filter = c => c.Name.GetValue(Culture).StartsWith(Value);
            }
            else if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = c => c.Name.ToString().StartsWith(Value); 
            }
            return filter;
        }
    }




    public class AdvertiserAccountCriteria : CriteriaBase<AdvertiserAccount>
    {
        public int AccountId { get; set; }
        public string culture { get; set; }
        public int? userId { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public bool IsReadOnly { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
    
        public string Name { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserAccountCriteria Commoncr)
        {
            AccountId = Commoncr.AccountId;
            userId = Commoncr.userId;
            IsPrimaryUser = Commoncr.IsPrimaryUser;
            IsReadOnly = Commoncr.IsReadOnly;

            Name = Commoncr.Name;
            showArchived = Commoncr.showArchived;
            culture = Commoncr.culture;

            showActive = Commoncr.showActive;



            showArchived = Commoncr.showArchived;


            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;





        }

        public Expression<Func<AdvertiserAccount, bool>> GetExpressionNoAccess()
        {

            if (Name == null)
            {
                Name = string.Empty;
            }
            Expression<Func<AdvertiserAccount, bool>> filter = null;
            if (userId.HasValue)
            {

                filter =
                   (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                   && c.Account.ID == AccountId

                   && (c.IsRestricted == true &&  !c.Users.Any(s => s.User.ID == userId && s.IsDeleted == false))
                   && (string.IsNullOrEmpty(Name) || c.Name.Contains(Name))
                   );
            }
            else
            {
                filter =
                    (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                    && c.Account.ID == AccountId

                    //&& ( c.Users.Any(s => s.User.ID == userId))
                    && (string.IsNullOrEmpty(Name) || c.Name.Contains(Name))
                    );

            }

            return filter;
        }
        public override Expression<Func<AdvertiserAccount, bool>> GetExpression()
        {

            if (Name == null)
            {
                Name = string.Empty;
            }
            Expression<Func<AdvertiserAccount, bool>> filter=null;
            if (userId.HasValue && !Framework.OperationContext.Current.UserInfo<IUserInfo>().IsPrimaryUser)
            {
                if (!Framework.OperationContext.Current.UserInfo<IUserInfo>().IsReadOnly     )
                {
                    filter =
                       (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                       && c.Account.ID == AccountId

                       && (c.IsRestricted == false || c.Users.Any(s => s.User.ID == userId && s.IsDeleted == false))
                       && (string.IsNullOrEmpty(Name) || c.Name.Contains(Name))
                       );
                }
                else
                {
                    filter =
                       (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                       && c.Account.ID == AccountId

                       && (c.IsRestricted == true &&  c.Users.Any(s => s.User.ID == userId  && s.IsDeleted == false))
                       && (string.IsNullOrEmpty(Name) || c.Name.Contains(Name))
                       );


                }
            }
            else
            {
                filter =
                    (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                    && c.Account.ID == AccountId

                    //&& ( c.Users.Any(s => s.User.ID == userId))
                    && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name))
                    );

            }

            return filter;
        }

        public override Func<AdvertiserAccount, bool> GetWhere()
        {
          
            Func<AdvertiserAccount, bool> filter = (c => (c.IsDeleted == false || c.IsDeleted == showArchived) && c.Account.ID == AccountId /*&& (!userId.HasValue || c.User.ID == userId)*/ );
            return filter;
        }
    }
}
