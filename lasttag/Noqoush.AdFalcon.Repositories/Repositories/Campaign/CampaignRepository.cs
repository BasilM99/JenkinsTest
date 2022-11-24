using NHibernate;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework;
using Noqoush.Framework.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CampaignRepository : RepositoryBase<Domain.Model.Campaign.Campaign, int>, ICampaignRepository
    {
        public CampaignRepository(RepositoryImplBase<Domain.Model.Campaign.Campaign, int> repository)
            : base(repository)
        {
        }

        private IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository = IoC.Instance.Resolve<IAccountPortalPermissionsRepository>();
        private IAdCreativeRepository AdCreativeRepository = IoC.Instance.Resolve<IAdCreativeRepository>();
        private IAdGroupRepository AdGroupRepository = IoC.Instance.Resolve<IAdGroupRepository>();
        public int GetAdvertiserId(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<Domain.Model.Campaign.Campaign, Domain.Model.Campaign.Campaign> rootQuery = nhibernateSession.QueryOver<Domain.Model.Campaign.Campaign>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            return rootQuery.Select(M => M.Advertiser.ID).SingleOrDefault<int>();



        }
        public string GetAdvertiserName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<Domain.Model.Campaign.Campaign, Domain.Model.Campaign.Campaign> rootQuery = nhibernateSession.QueryOver<Domain.Model.Campaign.Campaign>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            var adv= rootQuery.Select(M => M.Advertiser).SingleOrDefault<Advertiser>();

            if (adv!=null)
            {
                return adv.Name.ToString();
            }
            return string.Empty;

        }
        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<Domain.Model.Campaign.Campaign, Domain.Model.Campaign.Campaign> rootQuery = nhibernateSession.QueryOver<Domain.Model.Campaign.Campaign>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            return rootQuery.Select(M => M.Name).SingleOrDefault<string>();


        }

        public bool IsAllowedGroup(AdGroup group)
        {
            return (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager")
                          || (group.Objective.AdAction.AdTypes.Where(x => x.Permission == null && (x.SubTypes == null || x.SubTypes.Count() == 0)).Count() > 0)
                          || (group.Objective.AdAction.AdTypes.Where(x => x.Permission != null && _AccountPortalPermissionsRepository.checkAdPermissions(x.Permission.Code)).Count() > 0)
                          || (group.Objective.AdAction.AdTypes.Where(x => x.Permission == null && x.SubTypes != null && x.SubTypes.Count() > 0 && x.SubTypes.Where(z => z.Permission != null && z.AdTypeActions.Where(q => q.ActionType.ID == group.Objective.AdAction.ID).Count() > 0 && _AccountPortalPermissionsRepository.checkAdPermissions(z.Permission.Code)).Count() > 0).Count() > 0)
                          );

        }
        public bool IsAllowedGroup(int id)
        {
            var group = AdGroupRepository.Get(id);

            return (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager")
                          || (group.Objective.AdAction.AdTypes.Where(x => x.Permission == null && (x.SubTypes == null || x.SubTypes.Count() == 0)).Count() > 0)
                          || (group.Objective.AdAction.AdTypes.Where(x => x.Permission != null && _AccountPortalPermissionsRepository.checkAdPermissions(x.Permission.Code)).Count() > 0)
                          || (group.Objective.AdAction.AdTypes.Where(x => x.Permission == null && x.SubTypes != null && x.SubTypes.Count() > 0 && x.SubTypes.Where(z => z.Permission != null && z.AdTypeActions.Where(q => q.ActionType.ID == group.Objective.AdAction.ID).Count() > 0 && _AccountPortalPermissionsRepository.checkAdPermissions(z.Permission.Code)).Count() > 0).Count() > 0)
                          );

        }

        public bool IsAllowedAd(AdCreative ad)
        {
            return (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager")
                || (ad.Type.Permission != null && _AccountPortalPermissionsRepository.checkAdPermissions(ad.Type.Permission.Code))
                || (ad.Type.Permission == null && (ad.Type.SubTypes == null || (ad.Type.SubTypes != null && ad.Type.SubTypes.Count() == 0)))
                || (ad.Type.SubTypes != null && ad.Type.SubTypes.Count() > 0 && ad.Type.SubTypes.Where(x => x.Permission != null).Count() > 0 && ad.Type.SubTypes.Where(x => x.Permission != null && _AccountPortalPermissionsRepository.checkAdPermissions(x.Permission.Code)).Count() > 0)
                );
        }


        public bool IsAllowedAd(int id)
        {
            var ad = AdCreativeRepository.Get(id);
            return (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AdOps") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager")
                || (ad.Type.Permission != null && _AccountPortalPermissionsRepository.checkAdPermissions(ad.Type.Permission.Code))
                || (ad.Type.Permission == null && (ad.Type.SubTypes == null || ad.Type.SubTypes.Count() == 0))
                || (ad.Type.SubTypes != null && ad.Type.SubTypes.Count() > 0 && ad.Type.SubTypes.Where(x => x.Permission != null).Count() > 0 && ad.Type.SubTypes.Where(x => x.Permission != null && _AccountPortalPermissionsRepository.checkAdPermissions(x.Permission.Code)).Count() > 0)
                );
        }


        public IList<Domain.Model.Campaign.AdGroup> GetAllAdGroupByAccount(int AccountId)
        {
            IList<AdGroup> AdGroups = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            Domain.Model.Campaign.Campaign CampaignAlias = null;
            AdGroups = nhibernateSession.QueryOver<AdGroup>()
                     .JoinAlias(M => M.Campaign, () => CampaignAlias)
                     .Where(M => !M.IsDeleted)
                     .Where(() => !CampaignAlias.IsDeleted && CampaignAlias.Account.ID == AccountId)
                     .List();

            return AdGroups;

        }



    }
}
