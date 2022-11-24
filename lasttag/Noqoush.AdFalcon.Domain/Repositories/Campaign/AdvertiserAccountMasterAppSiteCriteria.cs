using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework;
using Noqoush.AdFalcon.Domain.Model.Account;

using Noqoush.AdFalcon.Domain.Utilities;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    public class AdvertiserAccountMasterAppSiteCriteria : CriteriaBase<AdvertiserAccountMasterAppSite>
    {

        public int? AccountId { get; set; }
        public string culture { get; set; }
        public MasterAppSiteStatus  Status { get; set; }
        public MasterAppSiteType Type { get; set; }

        public int? userId { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool showGlobalAndAccount { get; set; }
        public bool showAccountAndAdvertiser { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public bool? GlobalScope { get; set; }
        public string Name { get; set; }
        public int? AdvAccountId { get; set; }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserAccountMasterAppSiteCriteria Commoncr)
        {
            AdvAccountId = Commoncr.AdvAccountId;

            Name = Commoncr.Name;
            GlobalScope = Commoncr.GlobalScope;
            IsPrimaryUser = Commoncr.IsPrimaryUser;
            AccountId = Commoncr.AccountId;
            culture = Commoncr.culture;

            Status = Commoncr.Status;

            Type = Commoncr.Type;


            userId = Commoncr.userId;

            showActive = Commoncr.showActive;



            showArchived = Commoncr.showArchived;

            showGlobalAndAccount = Commoncr.showGlobalAndAccount;
            showAccountAndAdvertiser = Commoncr.showAccountAndAdvertiser;

            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;





    }

        public override Expression<Func<AdvertiserAccountMasterAppSite, bool>> GetExpression()
        {

            if (Name == null)
            {
                Name = string.Empty;
            }
            Expression<Func<AdvertiserAccountMasterAppSite, bool>> filter = null;

            if (!this.showGlobalAndAccount)
            {
                filter =
                   (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                   && ((!userId.HasValue) /*|| (c.User.ID == userId)*/)
                          && ((!AccountId.HasValue) || (c.Account.ID == AccountId))
               && ((!GlobalScope.HasValue) || (c.GlobalScope == GlobalScope))
                    && ( (c.Link.ID == AdvAccountId))
                     && ((Status == MasterAppSiteStatus.None) || (c.Status == Status))
                           && ((Type == MasterAppSiteType.None) || (c.Type == Type))
                   && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name))
                   );
                 if (this.showAccountAndAdvertiser)
                {
                    filter =
                       (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                       && ((!userId.HasValue) /*|| (c.User.ID == userId)*/)
                              && ((!AccountId.HasValue) || (c.Account.ID == AccountId)    || ((!AdvAccountId.HasValue) || (c.Link.ID == AdvAccountId)) )
                   && ((!GlobalScope.HasValue) || (c.GlobalScope == GlobalScope))
                      
                         && ((Status == MasterAppSiteStatus.None) || (c.Status == Status))
                               && ((Type == MasterAppSiteType.None) || (c.Type == Type))
                       && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name))
                       );

                }

            }
           
            else
            {
                filter =
                    (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                     /* && ((!userId.HasValue) || c.User.ID == userId))
                           */

                     && ((c.GlobalScope == true) || (c.Link.ID == AdvAccountId) || ((!AccountId.HasValue) || (c.Account.ID == AccountId)))
                      && ((Status == MasterAppSiteStatus.None) || (c.Status == Status))
                               && ((Type == MasterAppSiteType.None) || (c.Type == Type))
                    && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name))
                    );


            }

            return filter;
        }

        public override Func<AdvertiserAccountMasterAppSite, bool> GetWhere()
        {
         
            return null;
        }
    }





    public class AudienceSegmentCriteria : CriteriaBase<AudienceSegment>
    {
        public string Value { get; set; }
        public string Culture { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public int AdvAccountId { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public bool showArchived { get; set; }


        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AudienceSegmentCriteria Commoncr)
        {
            Value = Commoncr.Value;
            AdvAccountId = Commoncr.AdvAccountId;
            Name = Commoncr.Name;
           
            Culture = Commoncr.Culture;

            showArchived = Commoncr.showArchived;



            showArchived = Commoncr.showArchived;


            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;





        }

        public override Expression<Func<AudienceSegment, bool>> GetExpression()
        {
            Expression<Func<AudienceSegment, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(Culture))
            {
                filter = C => C.Name.Values.Any(v => v.Value.Contains(Value) && v.Culture == Culture) && (C.IsDeleted == false || C.IsDeleted == showArchived) && ((C.Advertiser.ID == AdvAccountId)) && C.Provider.IsExternalProvider == false;
            }
            else if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = C => C.Name.Values.Any(v => v.Value.Contains(Value)) && (C.IsDeleted == false || C.IsDeleted == showArchived) && ((C.Advertiser.ID == AdvAccountId)) && C.Provider.IsExternalProvider == false;
            }
            else
            {
                filter = C =>  (C.IsDeleted == false || C.IsDeleted == showArchived) && ((C.Advertiser.ID == AdvAccountId)) && C.Provider.IsExternalProvider == false;
            }
            return filter;
        }

        public override Func<AudienceSegment, bool> GetWhere()
        {
            Func<AudienceSegment, bool> filter = c => true;
            if (!string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(Culture))
            {
                filter = C => C.Name.GetValue(Culture).StartsWith(Value) && (C.IsDeleted == false || C.IsDeleted == showArchived) && ((C.Advertiser.ID == AdvAccountId)) && C.Provider.IsExternalProvider == false;
            }
            else if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = C => C.Name.ToString().StartsWith(Value) && (C.IsDeleted == false || C.IsDeleted == showArchived) && ((C.Advertiser.ID == AdvAccountId)) && C.Provider.IsExternalProvider == false;
            }
            else
            {
                filter = C =>  (C.IsDeleted == false || C.IsDeleted == showArchived) && ((C.Advertiser.ID == AdvAccountId)) && C.Provider.IsExternalProvider == false;
            }
            return filter;
        }
    }
    public class AdvertiserAccountMasterAppSiteItemCriteria : CriteriaBase<AdvertiserAccountMasterAppSiteItem>
    {
        public int? AccountId { get; set; }
        public string culture { get; set; }
        public int? StatusId { get; set; }
        public int? userId { get; set; }
        public MasterAppSiteItemType Type { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }

        public string Name { get; set; }
        public string BundleId { get; set; }
        public string AppSiteId { get; set; }
        public string Domain { get; set; }
        public int MasterListId { get; set; }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.AdvertiserAccountMasterAppSiteItemCriteria Commoncr)
        {
            MasterListId = Commoncr.MasterListId;
            Domain = Commoncr.Domain;
            Name = Commoncr.Name;
            AppSiteId = Commoncr.AppSiteId;
            IsPrimaryUser = Commoncr.IsPrimaryUser;
            AccountId = Commoncr.AccountId;
            culture = Commoncr.culture;

            BundleId = Commoncr.BundleId;

            Type = Commoncr.Type;


            userId = Commoncr.userId;

            showActive = Commoncr.showActive;



            showArchived = Commoncr.showArchived;

            StatusId = Commoncr.StatusId;
            AccountId = Commoncr.AccountId;

            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;





        }

        public override Expression<Func<AdvertiserAccountMasterAppSiteItem, bool>> GetExpression()
        {
            if (Name == null)
            {
                Name = string.Empty;
            }
            if (Domain == null)
            {
                Domain = string.Empty;
            }
            if (AppSiteId == null)
            {
                AppSiteId = string.Empty;
            }
            if (BundleId == null)
            {
                BundleId = string.Empty;
            }

            Expression<Func<AdvertiserAccountMasterAppSiteItem, bool>> filter = null;
             filter =
                    (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
            
                        && c.Link.ID == MasterListId
                    //&& ( c.Users.Any(s => s.User.ID == userId))
                         && ((!userId.HasValue) /*|| (c.User.ID == userId)*/)
                          && ((!AccountId.HasValue) || (c.Account.ID == AccountId))
                     && ((Type == MasterAppSiteItemType.None) || (c.Type == Type))
                    && (string.IsNullOrEmpty(Name) || c.AppSiteName.ToLower().Contains(Name))
                     // && (string.IsNullOrEmpty(Name) || c.AppSiteID.ToLower().Contains(AppSiteId))
                  && (string.IsNullOrEmpty(BundleId) || c.BundleID.ToLower().Contains(BundleId))
                         && (string.IsNullOrEmpty(Domain) || c.Domain.ToLower().Contains(Domain))
                    );

         

            return filter;
        }

        public override Func<AdvertiserAccountMasterAppSiteItem, bool> GetWhere()
        {

            return null;
        }
    }


    public class PixelCriteria : CriteriaBase<Pixel>
    {

        public int? AccountId { get; set; }
        public string culture { get; set; }
        public PixelStatus Status { get; set; }
        public MasterAppSiteType Type { get; set; }

        public int? userId { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool showGlobalAndAccount { get; set; }
        public bool showAccountAndAdvertiser { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public bool? GlobalScope { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }
        public int? AdvAccountId { get; set; }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.PixelCriteria Commoncr)
        {
            AdvAccountId = Commoncr.AdvAccountId;
            Value = Commoncr.Value;
            Name = Commoncr.Name;
            GlobalScope = Commoncr.GlobalScope;
            IsPrimaryUser = Commoncr.IsPrimaryUser;
            AccountId = Commoncr.AccountId;
            culture = Commoncr.culture;

            Status = Commoncr.Status;

            Type = Commoncr.Type;


            userId = Commoncr.userId;

            showActive = Commoncr.showActive;



            showArchived = Commoncr.showArchived;

            showGlobalAndAccount = Commoncr.showGlobalAndAccount;
            showAccountAndAdvertiser = Commoncr.showAccountAndAdvertiser;

            DataFrom = Commoncr.DataFrom; DataTo = Commoncr.DataTo;

            Page = Commoncr.Page;
            Size = Commoncr.Size;





        }

        public override Expression<Func<Pixel, bool>> GetExpression()
        {

            if (Name == null)
            {
                Name = string.Empty;
            }
            Expression<Func<Pixel, bool>> filter = null;

            if (!this.showGlobalAndAccount)
            {
                filter =
                   (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                   //&& ((!userId.HasValue) || (c.User.ID == userId))
                          && ((!AccountId.HasValue) || (c.Account.ID == AccountId))
          
                    && ((c.Link.ID == AdvAccountId))
                     && ((Status == PixelStatus.None) || (c.Status == Status))
                          
                   && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name))
                   );
                if (this.showAccountAndAdvertiser)
                {
                    filter =
                       (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                       //&& ((!userId.HasValue) || (c.User.ID == userId))
                              && ((!AccountId.HasValue) || (c.Account.ID == AccountId) || ((!AdvAccountId.HasValue) || (c.Link.ID == AdvAccountId)))
                

                         && ((Status == PixelStatus.None) || (c.Status == Status))
                             
                       && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name))
                       );

                }

            }

            else
            {
                filter =
                    (c => (c.IsDeleted == false || c.IsDeleted == showArchived)
                    //&& ((!userId.HasValue) || (c.User.ID == userId))


                     && (  (c.Link.ID == AdvAccountId) || ((!AccountId.HasValue) || (c.Account.ID == AccountId)))
                      && ((Status == PixelStatus.None) || (c.Status == Status))
                            
                    && (string.IsNullOrEmpty(Name) || c.Name.ToLower().Contains(Name))
                      && (string.IsNullOrEmpty(Value) || c.Name.ToLower().StartsWith(Value))

                    );


            }


            //if (!string.IsNullOrEmpty(Value))
            //{
               
            //    filter = filter.AndAlso(c => c.Name.StartsWith(Value));
            //}



            return filter;
        }

        public override Func<Pixel, bool> GetWhere()
        {

            return null;
        }
    }
}
