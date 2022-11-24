using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using NHibernate.Criterion.Lambda;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class AccountRepository : RepositoryBase<ArabyAds.AdFalcon.Domain.Model.Account.Account, int>, IAccountRepository
    {
        AuditTrial AuditTrialAlias = null;
        AuditTrialStat AuditTrialStatAlias = null;
        AuditTrialSessionStat AuditTrialSessionStatAlias = null;
        public AccountRepository(RepositoryImplBase<ArabyAds.AdFalcon.Domain.Model.Account.Account, int> repository)
            : base(repository)
        {


        }

        public IEnumerable<AuditTrialDto> GeAuditTrialForObjectRoot(AuditTrialFilter filter, out int TotalCount)
        {

            AuditTrialDto dto = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AuditTrial, AuditTrial> rootQuery = nhibernateSession.QueryOver<AuditTrial>(() => AuditTrialAlias);
          
                var countQuery = nhibernateSession.QueryOver<AuditTrial>(()=>AuditTrialAlias);

      

            //joins
            //rootQuery.Where(
            //                 p => p.RootObjectId > 0);
            if (filter.RootID > 0)
            {

                rootQuery.Where(
                        p => p.RootObjectId == filter.RootID);
                countQuery.Where(
                        p => p.RootObjectId == filter.RootID);
            }
            if (filter.ObjectTypeID > 0)
            {

                //ObjectType ObjectTypeALias = null;
                //rootQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);
                //countQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);
     //           var LastUserSubQuery = QueryOver.Of<ObjectType>().Select(M => M.ID)
     //.Where(M => M.ID == AuditTrialAlias.ObjectType.ID).Where(M => M.RootID == filter.ObjectTypeID);
                rootQuery.WithSubquery.WhereProperty(p => p.ObjectType.ID)
                    .In(QueryOver.Of<ObjectType>()

                         .Where(M => M.RootID == filter.ObjectTypeID)
                         //.Where(() => productSupplierAlias.SupplierProductNumber == searchtext)
                         .Select(p => p.ID)
                    );
                countQuery.WithSubquery.WhereProperty(p => p.ObjectType.ID)
            .In(QueryOver.Of<ObjectType>()

                 .Where(M => M.RootID == filter.ObjectTypeID)
                 //.Where(() => productSupplierAlias.SupplierProductNumber == searchtext)
                 .Select(p => p.ID)
            );
                //   rootQuery.Where(
                //            () => ObjectTypeALias.RootID == filter.ObjectTypeID);
                //   countQuery.Where(
                //() => ObjectTypeALias.RootID == filter.ObjectTypeID);


            }

        //    if (filter.UserId > 0)
        //    {

        //        rootQuery.Where(
        //                p => p.User == filter.UserId);
        //        countQuery.Where(
        //p => p.User == filter.UserId);


        //    }

            if (!string.IsNullOrEmpty(filter.UserName))
            {

                var disjunction = new Disjunction();





                disjunction.Add(Restrictions.Where < ArabyAds.AdFalcon.Domain.Model.Account.User > (M=>M.FirstName.IsLike(filter.UserName,MatchMode.Anywhere)));


                disjunction.Add(Restrictions.Where<ArabyAds.AdFalcon.Domain.Model.Account.User>(M => M.LastName.IsLike(filter.UserName, MatchMode.Anywhere)));
                var UserObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.User>().Select(M => M.ID)
    .Where(disjunction).Where(M => M.ID == AuditTrialAlias.User);


                countQuery.WithSubquery.WhereExists(UserObjectIdSubQuery);
                rootQuery.WithSubquery.WhereExists(UserObjectIdSubQuery);


            }

            if (filter.ActionTimeFrom.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeFrom.Value.Year, filter.ActionTimeFrom.Value.Month, filter.ActionTimeFrom.Value.Day, 0, 0, 0);
                rootQuery.Where(
                   p => p.ActionTime >= dateonly);

                countQuery.Where(
  p => p.ActionTime >= dateonly);
            }
            if (filter.ActionTimeTo.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeTo.Value.Year, filter.ActionTimeTo.Value.Month, filter.ActionTimeTo.Value.Day, 23, 59, 59);

                rootQuery.Where(
                   p => p.ActionTime <= dateonly);

                countQuery.Where(
   p => p.ActionTime <= dateonly);
            }
            var projections = new List<IProjection>();
            projections.Add(Projections.Group<AuditTrial>(M => M.SessionId).WithAlias(() => dto.SessionId));
            projections.Add(Projections.Property<AuditTrial>(M => M.ActionTime).WithAlias(() => dto.ActionTime));
            projections.Add(Projections.Property<AuditTrial>(M => M.User).WithAlias(() => dto.User));
            //projections.Add(Projections.RowCountInt64().WithAlias(() => dto.CountRecords));
            rootQuery.Select(projections.ToArray());
            //IFutureValue<int> futureCount = rootQuery.ToRowCountQuery().FutureValue<int>();



            if (filter.PageIndex > 0)
            {
                var pageIndexM = filter.PageIndex - 1;


                rootQuery.OrderBy(item => item.ActionTime).Desc();
                rootQuery.Skip(pageIndexM * filter.PageSize)
                                         .Take(filter.PageSize);

            }
            else
            {


                rootQuery.OrderBy(item => item.ActionTime).Desc();

            }
            rootQuery.TransformUsing(Transformers.AliasToBean<AuditTrialDto>());
            TotalCount = countQuery
        .Select(Projections.CountDistinct<AuditTrial>(x => x.SessionId))

        .FutureValue<int>()
        .Value;

            return rootQuery.List<AuditTrialDto>();
        }

        public IEnumerable<AuditTrialDto> GeAuditTrialMainRoots(AuditTrialFilter filter, out int TotalCount)
        {
            ObjectType ObjectTypeALias = null;
            AuditTrialDto dto = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AuditTrial, AuditTrial> rootQuery = nhibernateSession.QueryOver<AuditTrial>(()=> AuditTrialAlias);
            var countQuery = nhibernateSession.QueryOver<AuditTrial>(()=> AuditTrialAlias);
            var LastActionTimeSubQuery = QueryOver.Of<AuditTrialStat>().Select(M=>M.LastActionTime)
              .Where(M => M.RootObjectTypeId == ObjectTypeALias.RootID).Where(M => M.RootObjectId == AuditTrialAlias.RootObjectId);

            var LastUserSubQuery = QueryOver.Of<AuditTrialStat>().Select(M => M.LastUser)
        .Where(M => M.RootObjectTypeId == ObjectTypeALias.RootID).Where(M => M.RootObjectId == AuditTrialAlias.RootObjectId);
    //        var AccountSubQuery = QueryOver.Of<AuditTrialStat>().Select(M => M.AccountId)
    //.Where(M => M.RootObjectTypeId == ObjectTypeALias.RootID).Where(M => M.RootObjectId == AuditTrialAlias.RootObjectId).Where(M => M.AccountId == filter.UserId);


            AuditTrialStat AuditTrialStatAlias = null;
          
            //rootQuery.JoinAlias(() => AuditTrialAlias.RootObjectId, () => AuditTrialStatAlias.RootObjectId);
            //rootQuery.Where(()=>AuditTrialStatAlias.RootObjectTypeId == ObjectTypeALias.RootID);

            var objectActionRecreateCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "RecreateCollection");
              var objectActionUpdatCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "UpdatCollection");
            var objectActionRemoveCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "RemoveCollection");

            var objectTypeAppSite = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName== "ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite");
            var objectTypeCamp = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName == "ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign");
            var objectTypeRep = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName == "ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler");
            var objectTypeAcc = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName == "ArabyAds.AdFalcon.Domain.Model.Account.Account");

            rootQuery.Where(M => M.ObjectAction.ID != objectActionRecreateCollection.ID);
            rootQuery.Where(M => M.ObjectAction.ID != objectActionUpdatCollection.ID);
            rootQuery.Where(M => M.ObjectAction.ID != objectActionRemoveCollection.ID);

            countQuery.Where(M => M.ObjectAction.ID != objectActionRecreateCollection.ID);
            countQuery.Where(M => M.ObjectAction.ID != objectActionUpdatCollection.ID);
            countQuery.Where(M => M.ObjectAction.ID != objectActionRemoveCollection.ID);
            //joins
            rootQuery.Where(
                             p => p.RootObjectId > 0);
           
            
            countQuery.Where(
p => p.RootObjectId > 0);
            if (filter.ObjectActionID > 0)
            {

                rootQuery.Where(
                        p => p.ObjectAction.ID == filter.ObjectActionID);
                countQuery.Where(
 p => p.ObjectAction.ID == filter.ObjectActionID);
            }
         
            rootQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);
            countQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);

            if (filter.ObjectTypeID > 0)
            {
                if (!string.IsNullOrEmpty(filter.objectTypeName))
                {
                    rootQuery.WithSubquery.WhereExists(GetObjectNameDetachedCriteria(filter.objectshortType, filter.objectTypeName));
                    countQuery.WithSubquery.WhereExists(GetObjectNameDetachedCriteria(filter.objectshortType, filter.objectTypeName));
                }

                rootQuery.Where(
                         () => ObjectTypeALias.RootID == filter.ObjectTypeID);

                countQuery.Where(
() => ObjectTypeALias.RootID == filter.ObjectTypeID);


            }

            if (filter.UserId > 0)
            {

                var disjunction = new Disjunction();
                var disjunction1 = new Disjunction();


                var conjunction1 = new Conjunction();

                var ReportSchedulerObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler>().Select(M => M.ID)
                                          .Where(M => M.Account.ID == filter.UserId).Where(M => M.ID == AuditTrialAlias.RootObjectId);
                conjunction1.Add(Subqueries.WhereExists(ReportSchedulerObjectIdSubQuery));
                conjunction1.Add(Restrictions.Where(() => ObjectTypeALias.RootID == objectTypeRep.ID));
                disjunction.Add(conjunction1);
                var disjunction2 = new Disjunction();
                var conjunction2 = new Conjunction();
                var  AppSiteObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.ID)
                    .Where(M => M.Account.ID== filter.UserId).Where(M => M.ID == AuditTrialAlias.RootObjectId);

                conjunction2.Add(Subqueries.WhereExists(AppSiteObjectIdSubQuery));
                conjunction2.Add(Restrictions.Where(() => ObjectTypeALias.RootID == objectTypeAppSite.ID));

                disjunction.Add(conjunction2);
                var conjunction3 = new Conjunction();
                var disjunction3 = new Disjunction();
                var CampaignObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign>().Select(M => M.ID)
                   .Where(M => M.Account.ID == filter.UserId).Where(M => M.ID == AuditTrialAlias.RootObjectId);

                conjunction3.Add(Subqueries.WhereExists(CampaignObjectIdSubQuery));
                conjunction3.Add(Restrictions.Where(() => ObjectTypeALias.RootID == objectTypeCamp.ID));
                disjunction.Add(conjunction3);
                var conjunction4 = new Conjunction();
                var disjunction4 = new Disjunction();
                var UserObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts>().Select(M => M.User.ID)
                    .Where(M => M.Account.ID == filter.UserId).Where(M => M.Account.ID == AuditTrialAlias.RootObjectId);

              

                conjunction4.Add(Subqueries.WhereExists(UserObjectIdSubQuery));
                conjunction4.Add(Restrictions.Where(() => ObjectTypeALias.RootID == objectTypeAcc.ID));
                disjunction.Add(conjunction4);

                rootQuery.Where(
                   disjunction);

                countQuery.Where(
disjunction);
//                rootQuery.Where(
//           disjunction2);

//                countQuery.Where(
//disjunction2);
//                rootQuery.Where(
//           disjunction3);

//                countQuery.Where(
//disjunction3);
//                rootQuery.Where(
//    disjunction4);

//                countQuery.Where(
//disjunction4);

//                rootQuery.WithSubquery.WhereExists(
//              AccountSubQuery);

//                countQuery.WithSubquery.WhereExists(
//AccountSubQuery);
            }

            if (filter.ActionTimeFrom.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeFrom.Value.Year, filter.ActionTimeFrom.Value.Month, filter.ActionTimeFrom.Value.Day, 0, 0, 0);

                rootQuery.Where(
                   p => p.ActionTime >= dateonly);

                countQuery.Where(
  p => p.ActionTime >= dateonly);
            }
            if (filter.ActionTimeTo.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeTo.Value.Year, filter.ActionTimeTo.Value.Month, filter.ActionTimeTo.Value.Day, 23, 59, 59);

                rootQuery.Where(
                   p => p.ActionTime <= dateonly);

                countQuery.Where(
   p => p.ActionTime <= dateonly);
            }
            var projections = new List<IProjection>();
            projections.Add(Projections.Group<AuditTrial>(M => M.RootObjectId).WithAlias(() => dto.RootObjectId));
            projections.Add(Projections.SubQuery(LastActionTimeSubQuery).WithAlias(() => dto.ActionTime));

            //projections.Add(Projections.SubQuery(LastUserSubQuery).WithAlias(() => dto.User));
            //projections.Add();
            // projections.Add(Projections.Max<AuditTrial>(M => M.ActionTime).WithAlias(() => dto.ActionTime));
            projections.Add(Projections.Group(() => ObjectTypeALias.RootID).WithAlias(() => dto.RootObjectTypeID));
            // projections.Add(Projections.RowCountInt64().WithAlias(() => dto.CountRecords));
            rootQuery.Select(projections.ToArray());


            if (filter.PageIndex > 0)
            {
                var pageIndexM = filter.PageIndex - 1;


                rootQuery.OrderBy(Projections.SubQuery(LastActionTimeSubQuery)).Desc();
                rootQuery.Skip(pageIndexM * filter.PageSize)
                                         .Take(filter.PageSize);

            }
            else
            {


                rootQuery.OrderBy(Projections.Max<AuditTrial>(M => M.ActionTime)).Desc();

            }
            rootQuery.TransformUsing(Transformers.AliasToBean<AuditTrialDto>());
            TotalCount = countQuery
           .Select(
            Projections.Distinct(



                 Projections.ProjectionList()
                    .Add(Projections.Property<AuditTrial>(x => x.RootObjectId))
                    .Add(Projections.Property(() => ObjectTypeALias.RootID))


                )



                )

           .List<object[]>().Count
           ;
            return rootQuery.List<AuditTrialDto>();
        }

        public dynamic GetObjectNameDetachedCriteria(string objectTypeName, string Name)
        {
            dynamic ObjectNameSubQuery = null;

            if (!string.IsNullOrEmpty(objectTypeName))
            {
                switch (objectTypeName)
                {
                    case "ReportScheduler":



                        ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler>().Select(M => M.ID)
                            .Where(M => M.Name.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.ID == AuditTrialAlias.RootObjectId);




                        return ObjectNameSubQuery;

                    case "AppSite":




                        ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.ID)
                            .Where(M => M.Name.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.ID == AuditTrialAlias.RootObjectId);
                        return ObjectNameSubQuery;




                    case "Campaign":



                        ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign>().Select(M => M.ID)
                            .Where(M => M.Name.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.ID == AuditTrialAlias.RootObjectId);

                        return ObjectNameSubQuery;

                    case "Account":


                        ArabyAds.AdFalcon.Domain.Model.Account.User useralias = null;

                        ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts>().Select(M => M.User.ID)
                            .JoinAlias(M=>M.User, ()=> useralias).Where(() => useralias.FirstName.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(() => useralias.LastName.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.Account.ID == AuditTrialAlias.RootObjectId);
                        return ObjectNameSubQuery;

                    default:
                        throw new Exception("inValid objectType Name");

                }



            }
            else
            {
                throw new Exception("objectType Must has a name");

            }


        }
        public DateTime GeMaxActionTime(int objectRootId, int objectRootTypeId)
        {

            AuditTrialDto dto = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AuditTrial, AuditTrial> rootQuery = nhibernateSession.QueryOver<AuditTrial>(() => AuditTrialAlias);
         
            var objectActionRecreateCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "RecreateCollection");
            var objectActionUpdatCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "UpdatCollection");
            var objectActionRemoveCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "RemoveCollection");

            rootQuery.Where(M => M.ObjectAction.ID != objectActionRecreateCollection.ID);
            rootQuery.Where(M => M.ObjectAction.ID != objectActionUpdatCollection.ID);
            rootQuery.Where(M => M.ObjectAction.ID != objectActionRemoveCollection.ID);

      
     
            ObjectType ObjectTypeALias = null;
            rootQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);


            rootQuery.Where(M=>M.RootObjectId== objectRootId);

                rootQuery.Where(
                         () => ObjectTypeALias.RootID == objectRootTypeId);




            rootQuery.SelectList(L => L.SelectMax(M=>M.ActionTime));


            return rootQuery.SingleOrDefault<DateTime>();
        }


        public bool IsAccountDSP(int Id)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ArabyAds.AdFalcon.Domain.Model.Account.Account accountAlias = null;
            IQueryOver<ArabyAds.AdFalcon.Domain.Model.Account.Account, ArabyAds.AdFalcon.Domain.Model.Account.Account> rootQuery = nhibernateSession.QueryOver<ArabyAds.AdFalcon.Domain.Model.Account.Account>(() => accountAlias);
            rootQuery.Where(M=>M.AccountRole==Domain.Common.Model.Account.AccountRole.DSP);
            rootQuery.Where(M => M.ID == Id);

            var ID=rootQuery.Select(M => M.PrimaryUser.ID).SingleOrDefault<int>();
            return ID > 0;

        }
        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ArabyAds.AdFalcon.Domain.Model.Account.User userAlias = null;
            IQueryOver<ArabyAds.AdFalcon.Domain.Model.Account.User, ArabyAds.AdFalcon.Domain.Model.Account.User> rootQuery = nhibernateSession.QueryOver<ArabyAds.AdFalcon.Domain.Model.Account.User>(()=> userAlias);

            //joins
      


            var UseraccountsSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts>().Select(M => M.Account.ID)
                            .Where(M => M.Account.ID== Id).Where(M => M.User.ID == userAlias.ID);
            var projections = new List<IProjection>();
         
            projections.Add(Projections.Property<ArabyAds.AdFalcon.Domain.Model.Account.User>(M => M.FirstName).WithAlias(()=>userAlias.FirstName));
            projections.Add(Projections.Property<ArabyAds.AdFalcon.Domain.Model.Account.User>(M => M.LastName).WithAlias(() => userAlias.LastName));

          
            rootQuery.Select(projections.ToArray());
            rootQuery.WithSubquery.WhereExists(
                       UseraccountsSubQuery);

            //rootQuery.TransformUsing()
            rootQuery.TransformUsing(Transformers.AliasToBean<ArabyAds.AdFalcon.Domain.Model.Account.User>());
            var users= rootQuery.List< ArabyAds.AdFalcon.Domain.Model.Account.User> ();
            return users[0].FirstName + " " + users[0].LastName;

        }
        public string GetObjectNameForUserName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<ArabyAds.AdFalcon.Domain.Model.Account.User, ArabyAds.AdFalcon.Domain.Model.Account.User> rootQuery = nhibernateSession.QueryOver<ArabyAds.AdFalcon.Domain.Model.Account.User>();

            //joins
            ArabyAds.AdFalcon.Domain.Model.Account.User userAlias = null;
            var projections = new List<IProjection>();

            projections.Add(Projections.Property<ArabyAds.AdFalcon.Domain.Model.Account.User>(M => M.FirstName).WithAlias(() => userAlias.FirstName));
            projections.Add(Projections.Property<ArabyAds.AdFalcon.Domain.Model.Account.User>(M => M.LastName).WithAlias(() => userAlias.LastName));


            rootQuery.Select(projections.ToArray());
            rootQuery.Where(
                        p => p.ID == Id);

            //rootQuery.TransformUsing()
            rootQuery.TransformUsing(Transformers.AliasToBean<ArabyAds.AdFalcon.Domain.Model.Account.User>());
            var users = rootQuery.List<ArabyAds.AdFalcon.Domain.Model.Account.User>();
            return users[0].FirstName + " " + users[0].LastName;

        }
        public bool IsRootObjectRelatedToAccount(int rootobjecid,int AccountId, int? UserId, string TypeName)
        {
            QueryOver ObjectNameSubQuery = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            if (!string.IsNullOrEmpty(TypeName))
            {
                switch (TypeName)


                {
                    case "ReportScheduler":


                        if (UserId.HasValue)
                        {
                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler>().Select(M => M.ID)
                            .Where(M => M.ID == rootobjecid).Where(M => M.Account.ID == AccountId).Where(M => M.User.ID == UserId);

                          
                        }
                        else
                        {
                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler>().Select(M => M.ID)
                           .Where(M => M.ID == rootobjecid).Where(M => M.Account.ID == AccountId);



                        }

                        break;

                     

                    case "AppSite":

                        if (UserId.HasValue)
                        {
                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.ID)
                                                       .Where(M => M.ID == rootobjecid).Where(M => M.Account.ID == AccountId).Where(M => M.User.ID == UserId);


                        }
                        else
                        {
                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.ID)
                             .Where(M => M.ID == rootobjecid).Where(M => M.Account.ID == AccountId);



                        }



                        break;




                    case "Campaign":

                        if (UserId.HasValue)
                        {

                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign>().Select(M => M.ID)
                                  .Where(M => M.ID == rootobjecid).Where(M => M.Account.ID == AccountId).Where(M => M.User.ID == UserId);


                        }
                        else
                        {

                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign>().Select(M => M.ID)
                                  .Where(M => M.ID == rootobjecid).Where(M => M.Account.ID == AccountId);

                        }




                        break;


                    case "Account":


                        if (UserId.HasValue)
                        {

                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts>().Select(M => M.User.ID)
                             .Where(M => M.Account.ID == rootobjecid).Where(M => M.User.ID == UserId);


                        }
                        else
                        {

                            ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts>().Select(M => M.User.ID)
                             .Where(M => M.Account.ID == rootobjecid);

                        }
                       /*.Where(M => M.Account.ID == AccountId)*/;

                        break;

                    default:
                        throw new Exception("inValid objectType Name");

                }


                if (ObjectNameSubQuery!=null)
                {
                   var listResult= ObjectNameSubQuery.DetachedCriteria.GetExecutableCriteria(nhibernateSession).List();
                    if (listResult!=null)
                    {

                        if (listResult.Count>0)

                        {
                            return true;

                        }

                    }
                }



            }
            else
            {
                throw new Exception("objectType Must has a name");

            }

            return false;
        }

        #region New Imp

        public IEnumerable<AuditTrialDto> GeAuditTrialMainRootsUsingStat(AuditTrialFilter filter, out int TotalCount)
        {
            ObjectType ObjectTypeALias = null;
            AuditTrialDto dto = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AuditTrialStat, AuditTrialStat> rootQuery = nhibernateSession.QueryOver<AuditTrialStat>(() => AuditTrialStatAlias);
            var countQuery = nhibernateSession.QueryOver<AuditTrialStat>(() => AuditTrialStatAlias);
            var LastActionTimeSubQuery = QueryOver.Of<AuditTrialStat>().Select(M => M.LastActionTime)
              .Where(M => M.RootObjectTypeId == ObjectTypeALias.RootID).Where(M => M.RootObjectId == AuditTrialAlias.RootObjectId);

            var LastUserSubQuery = QueryOver.Of<AuditTrialStat>().Select(M => M.LastUser)
        .Where(M => M.RootObjectTypeId == ObjectTypeALias.RootID).Where(M => M.RootObjectId == AuditTrialAlias.RootObjectId);
            //        var AccountSubQuery = QueryOver.Of<AuditTrialStat>().Select(M => M.AccountId)
            //.Where(M => M.RootObjectTypeId == ObjectTypeALias.RootID).Where(M => M.RootObjectId == AuditTrialAlias.RootObjectId).Where(M => M.AccountId == filter.UserId);


            //AuditTrialStat AuditTrialStatAlias = null;

            //rootQuery.JoinAlias(() => AuditTrialAlias.RootObjectId, () => AuditTrialStatAlias.RootObjectId);
            //rootQuery.Where(()=>AuditTrialStatAlias.RootObjectTypeId == ObjectTypeALias.RootID);

            //var objectActionRecreateCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "RecreateCollection");
            //var objectActionUpdatCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "UpdatCollection");
            //var objectActionRemoveCollection = UnitOfWork.Current.EntitySet<ObjectAction>().FirstOrDefault(p => p.ObjectActionName != null && p.ObjectActionName == "RemoveCollection");

            var objectTypeAppSite = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName == "ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite");
            var objectTypeCamp = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName == "ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign");
            var objectTypeRep = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName == "ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler");
            var objectTypeAcc = UnitOfWork.Current.EntitySet<ObjectType>().FirstOrDefault(p => p.ObjectTypeName != null && p.ObjectTypeName == "ArabyAds.AdFalcon.Domain.Model.Account.Account");

            //rootQuery.Where(M => M.ObjectAction.ID != objectActionRecreateCollection.ID);
            //rootQuery.Where(M => M.ObjectAction.ID != objectActionUpdatCollection.ID);
            //rootQuery.Where(M => M.ObjectAction.ID != objectActionRemoveCollection.ID);

            //countQuery.Where(M => M.ObjectAction.ID != objectActionRecreateCollection.ID);
            //countQuery.Where(M => M.ObjectAction.ID != objectActionUpdatCollection.ID);
            //countQuery.Where(M => M.ObjectAction.ID != objectActionRemoveCollection.ID);
//            //joins
//            rootQuery.Where(
//                             p => p.RootObjectId > 0);


//            countQuery.Where(
//p => p.RootObjectId > 0);
//            if (filter.ObjectActionID > 0)
//            {

//                rootQuery.Where(
//                        p => p.ObjectAction.ID == filter.ObjectActionID);
//                countQuery.Where(
// p => p.ObjectAction.ID == filter.ObjectActionID);
//            }

            //rootQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);
            //countQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);

            if (filter.ObjectTypeID > 0)
            {
                if (!string.IsNullOrEmpty(filter.objectTypeName))
                {
                    rootQuery.WithSubquery.WhereExists(GetObjectNameDetachedCriteriaUsingStat(filter.objectshortType, filter.objectTypeName));
                    countQuery.WithSubquery.WhereExists(GetObjectNameDetachedCriteriaUsingStat(filter.objectshortType, filter.objectTypeName));
                }

                rootQuery.Where(
                         () => AuditTrialStatAlias.RootObjectTypeId == filter.ObjectTypeID);

                countQuery.Where(
() => AuditTrialStatAlias.RootObjectTypeId == filter.ObjectTypeID);


            }

            if (filter.UserId > 0)
            {

                var disjunction = new Disjunction();
                var disjunction1 = new Disjunction();


                var conjunction1 = new Conjunction();

                var ReportSchedulerObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler>().Select(M => M.ID)
                                          .Where(M => M.Account.ID == filter.AccountId).Where(M => M.ID == AuditTrialStatAlias.RootObjectId);
                if (!filter.IsPrimaryUser)
                {
                    ReportSchedulerObjectIdSubQuery.Where(M => M.User.ID == filter.UserId);
                }
                conjunction1.Add(Subqueries.WhereExists(ReportSchedulerObjectIdSubQuery));
                conjunction1.Add(Restrictions.Where(() => AuditTrialStatAlias.RootObjectTypeId == objectTypeRep.ID));
                disjunction.Add(conjunction1);
                var disjunction2 = new Disjunction();
                var conjunction2 = new Conjunction();
                var AppSiteObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.ID)
                    .Where(M => M.Account.ID == filter.AccountId).Where(M => M.ID == AuditTrialStatAlias.RootObjectId);
                if (!filter.IsPrimaryUser)
                {
                    AppSiteObjectIdSubQuery.Where(M => M.User.ID == filter.UserId);
                }
                conjunction2.Add(Subqueries.WhereExists(AppSiteObjectIdSubQuery));
                conjunction2.Add(Restrictions.Where(() => AuditTrialStatAlias.RootObjectTypeId == objectTypeAppSite.ID));

                disjunction.Add(conjunction2);
                var conjunction3 = new Conjunction();
                var disjunction3 = new Disjunction();
                var CampaignObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign>().Select(M => M.ID)
                   .Where(M => M.Account.ID == filter.AccountId).Where(M => M.ID == AuditTrialStatAlias.RootObjectId);
                if (!filter.IsPrimaryUser)
                {
                    CampaignObjectIdSubQuery.Where(M => M.User.ID == filter.UserId);
                }
                conjunction3.Add(Subqueries.WhereExists(CampaignObjectIdSubQuery));
                conjunction3.Add(Restrictions.Where(() => AuditTrialStatAlias.RootObjectTypeId == objectTypeCamp.ID));
                disjunction.Add(conjunction3);
                var conjunction4 = new Conjunction();
                var disjunction4 = new Disjunction();
                var UserObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts>().Select(M => M.User.ID)
                    .Where(M => M.Account.ID == filter.AccountId).Where(M => M.Account.ID == AuditTrialStatAlias.RootObjectId);


                if (!filter.IsPrimaryUser)
                {
                    UserObjectIdSubQuery.Where(M => M.User.ID == filter.UserId);
                }
                conjunction4.Add(Subqueries.WhereExists(UserObjectIdSubQuery));
                conjunction4.Add(Restrictions.Where(() => AuditTrialStatAlias.RootObjectTypeId == objectTypeAcc.ID));
                disjunction.Add(conjunction4);

                rootQuery.Where(
                   disjunction);

                countQuery.Where(
disjunction);
                //                rootQuery.Where(
                //           disjunction2);

                //                countQuery.Where(
                //disjunction2);
                //                rootQuery.Where(
                //           disjunction3);

                //                countQuery.Where(
                //disjunction3);
                //                rootQuery.Where(
                //    disjunction4);

                //                countQuery.Where(
                //disjunction4);

                //                rootQuery.WithSubquery.WhereExists(
                //              AccountSubQuery);

                //                countQuery.WithSubquery.WhereExists(
                //AccountSubQuery);
            }

            if (filter.ActionTimeFrom.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeFrom.Value.Year, filter.ActionTimeFrom.Value.Month, filter.ActionTimeFrom.Value.Day, 0, 0, 0);

                rootQuery.Where(
                   p => p.LastActionTime >= dateonly);

                countQuery.Where(
  p => p.LastActionTime >= dateonly);
            }
            if (filter.ActionTimeTo.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeTo.Value.Year, filter.ActionTimeTo.Value.Month, filter.ActionTimeTo.Value.Day, 23, 59, 59);

                rootQuery.Where(
                   p => p.LastActionTime <= dateonly);

                countQuery.Where(
   p => p.LastActionTime <= dateonly);
            }
            var projections = new List<IProjection>();
            projections.Add(Projections.Group<AuditTrialStat>(M => M.RootObjectId).WithAlias(() => dto.RootObjectId));
            projections.Add(Projections.Property<AuditTrialStat>(M=>M.LastActionTime).WithAlias(() => dto.ActionTime));
            projections.Add(Projections.Property<AuditTrialStat>(M => M.LastUser).WithAlias(() => dto.User));

            //projections.Add(Projections.SubQuery(LastUserSubQuery).WithAlias(() => dto.User));
            //projections.Add();
            // projections.Add(Projections.Max<AuditTrial>(M => M.ActionTime).WithAlias(() => dto.ActionTime));
            projections.Add(Projections.Group(() => AuditTrialStatAlias.RootObjectTypeId).WithAlias(() => dto.RootObjectTypeID));
            // projections.Add(Projections.RowCountInt64().WithAlias(() => dto.CountRecords));
            rootQuery.Select(projections.ToArray());


            if (filter.PageIndex > 0)
            {
                var pageIndexM = filter.PageIndex - 1;


                rootQuery.OrderBy(Projections.Property<AuditTrialStat>(M => M.LastActionTime)).Desc();
                rootQuery.Skip(pageIndexM * filter.PageSize)
                                         .Take(filter.PageSize);

            }
            else
            {


                rootQuery.OrderBy(Projections.Property<AuditTrialStat>(M => M.LastActionTime)).Desc();

            }
            rootQuery.TransformUsing(Transformers.AliasToBean<AuditTrialDto>());
            TotalCount = countQuery
           .Select(
            Projections.Distinct(



                 Projections.ProjectionList()
                    .Add(Projections.Property<AuditTrialStat>(x => x.RootObjectId))
                    .Add(Projections.Property(() => AuditTrialStatAlias.RootObjectTypeId))


                )



                )

           .List<object[]>().Count
           ;
            return rootQuery.List<AuditTrialDto>();
        }

        public dynamic GetObjectNameDetachedCriteriaUsingStat(string objectTypeName, string Name)
        {
            dynamic ObjectNameSubQuery = null;

            if (!string.IsNullOrEmpty(objectTypeName))
            {
                switch (objectTypeName)
                {
                    case "ReportScheduler":



                        ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Core.ReportScheduler>().Select(M => M.ID)
                            .Where(M => M.Name.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.ID == AuditTrialStatAlias.RootObjectId);




                        return ObjectNameSubQuery;

                    case "AppSite":




                        ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>().Select(M => M.ID)
                            .Where(M => M.Name.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.ID == AuditTrialStatAlias.RootObjectId);
                        return ObjectNameSubQuery;




                    case "Campaign":



                        ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Campaign.Campaign>().Select(M => M.ID)
                            .Where(M => M.Name.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.ID == AuditTrialStatAlias.RootObjectId);

                        return ObjectNameSubQuery;

                    case "Account":


                        ArabyAds.AdFalcon.Domain.Model.Account.User UserAlias = null;
                                                ObjectNameSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts>().Select(M => M.User.ID)
                            .JoinAlias(M=>M.User, ()=> UserAlias).Where(() => UserAlias.FirstName.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(() => UserAlias.LastName.IsInsensitiveLike(Name, MatchMode.Anywhere)).Where(M => M.Account.ID == AuditTrialStatAlias.RootObjectId);
                        return ObjectNameSubQuery;

                    default:
                        throw new Exception("inValid objectType Name");

                }



            }
            else
            {
                throw new Exception("objectType Must has a name");

            }


        }

        public IEnumerable<AuditTrialDto> GeAuditTrialForObjectRootUsingStat(AuditTrialFilter filter, out int TotalCount)
        {

            AuditTrialDto dto = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AuditTrialSessionStat, AuditTrialSessionStat> rootQuery = nhibernateSession.QueryOver<AuditTrialSessionStat>(() => AuditTrialSessionStatAlias);

            var countQuery = nhibernateSession.QueryOver<AuditTrialSessionStat>(() => AuditTrialSessionStatAlias);



            //joins
            //rootQuery.Where(
            //                 p => p.RootObjectId > 0);
            if (filter.RootID > 0)
            {

                rootQuery.Where(
                        p => p.RootObjectId == filter.RootID);
                countQuery.Where(
                        p => p.RootObjectId == filter.RootID);
            }
            if (filter.ObjectTypeID > 0)
            {

                //ObjectType ObjectTypeALias = null;
                //rootQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);
                //countQuery.JoinAlias(M => M.ObjectType, () => ObjectTypeALias);
                //           var LastUserSubQuery = QueryOver.Of<ObjectType>().Select(M => M.ID)
                //.Where(M => M.ID == AuditTrialAlias.ObjectType.ID).Where(M => M.RootID == filter.ObjectTypeID);
                rootQuery.WithSubquery.WhereProperty(p => p.RootObjectTypeId)
                    .In(QueryOver.Of<ObjectType>()

                         .Where(M => M.RootID == filter.ObjectTypeID)
                         //.Where(() => productSupplierAlias.SupplierProductNumber == searchtext)
                         .Select(p => p.ID)
                    );
                countQuery.WithSubquery.WhereProperty(p => p.RootObjectTypeId)
            .In(QueryOver.Of<ObjectType>()

                 .Where(M => M.RootID == filter.ObjectTypeID)
                 //.Where(() => productSupplierAlias.SupplierProductNumber == searchtext)
                 .Select(p => p.ID)
            );
                //   rootQuery.Where(
                //            () => ObjectTypeALias.RootID == filter.ObjectTypeID);
                //   countQuery.Where(
                //() => ObjectTypeALias.RootID == filter.ObjectTypeID);


            }

            //    if (filter.UserId > 0)
            //    {

            //        rootQuery.Where(
            //                p => p.User == filter.UserId);
            //        countQuery.Where(
            //p => p.User == filter.UserId);


            //    }

            if (!string.IsNullOrEmpty(filter.UserName))
            {

                var disjunction = new Disjunction();





                disjunction.Add(Restrictions.Where<ArabyAds.AdFalcon.Domain.Model.Account.User>(M => M.FirstName.IsLike(filter.UserName, MatchMode.Anywhere)));


                disjunction.Add(Restrictions.Where<ArabyAds.AdFalcon.Domain.Model.Account.User>(M => M.LastName.IsLike(filter.UserName, MatchMode.Anywhere)));
                var UserObjectIdSubQuery = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.User>().Select(M => M.ID)
    .Where(disjunction).Where(M => M.ID == AuditTrialSessionStatAlias.User);


                countQuery.WithSubquery.WhereExists(UserObjectIdSubQuery);
                rootQuery.WithSubquery.WhereExists(UserObjectIdSubQuery);


            }

            if (filter.ActionTimeFrom.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeFrom.Value.Year, filter.ActionTimeFrom.Value.Month, filter.ActionTimeFrom.Value.Day, 0, 0, 0);
                rootQuery.Where(
                   p => p.ActionTime >= dateonly);

                countQuery.Where(
  p => p.ActionTime >= dateonly);
            }
            if (filter.ActionTimeTo.HasValue)
            {
                var dateonly = new DateTime(filter.ActionTimeTo.Value.Year, filter.ActionTimeTo.Value.Month, filter.ActionTimeTo.Value.Day, 23, 59, 59);

                rootQuery.Where(
                   p => p.ActionTime <= dateonly);

                countQuery.Where(
   p => p.ActionTime <= dateonly);
            }
            var projections = new List<IProjection>();
            projections.Add(Projections.Group<AuditTrialSessionStat>(M => M.SessionId).WithAlias(() => dto.SessionId));
            projections.Add(Projections.Property<AuditTrialSessionStat>(M => M.ActionTime).WithAlias(() => dto.ActionTime));
            projections.Add(Projections.Property<AuditTrialSessionStat>(M => M.User).WithAlias(() => dto.User));
            //projections.Add(Projections.RowCountInt64().WithAlias(() => dto.CountRecords));
            rootQuery.Select(projections.ToArray());
            //IFutureValue<int> futureCount = rootQuery.ToRowCountQuery().FutureValue<int>();



            if (filter.PageIndex > 0)
            {
                var pageIndexM = filter.PageIndex - 1;


                rootQuery.OrderBy(item => item.ActionTime).Desc();
                rootQuery.Skip(pageIndexM * filter.PageSize)
                                         .Take(filter.PageSize);

            }
            else
            {


                rootQuery.OrderBy(item => item.ActionTime).Desc();

            }
            rootQuery.TransformUsing(Transformers.AliasToBean<AuditTrialDto>());
            TotalCount = countQuery
        .Select(Projections.CountDistinct<AuditTrialSessionStat>(x => x.SessionId))

        .FutureValue<int>()
        .Value;

            return rootQuery.List<AuditTrialDto>();
        }

        #endregion


        public IEnumerable<ArabyAds.AdFalcon.Domain.Model.Account.Account> QueryByCratiriaForUsers(Domain.Repositories.Account.UserCriteriaBase criteria, out int Count)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ArabyAds.AdFalcon.Domain.Model.Account.User userAlias = null;
            ArabyAds.AdFalcon.Domain.Model.Account.Account AccountAlias = null;
            IQueryOver<ArabyAds.AdFalcon.Domain.Model.Account.Account, ArabyAds.AdFalcon.Domain.Model.Account.Account> rootQuery = nhibernateSession.QueryOver<ArabyAds.AdFalcon.Domain.Model.Account.Account>(() => AccountAlias);
            //ArabyAds.AdFalcon.Domain.Model.Account.Account AccountAlias = null;

            ArabyAds.AdFalcon.Domain.Model.Account.UserAccounts UserAccountAlias = null;
            rootQuery.JoinAlias(() => AccountAlias.PrimaryUser, () => userAlias);
                  

            criteria.Name = string.IsNullOrEmpty(criteria.Name) ? string.Empty : criteria.Name.Trim();
            criteria.CompanyName = string.IsNullOrEmpty(criteria.CompanyName) ? string.Empty : criteria.CompanyName.Trim();
            criteria.Email = string.IsNullOrEmpty(criteria.Email) ? string.Empty : criteria.Email.Trim();
            rootQuery.Where(() => AccountAlias.Tenant.ID == ApplicationContext.Instance.Tenant.ID);
            if (!string.IsNullOrEmpty(criteria.Email))
            {
                rootQuery.Where(() => userAlias.EmailAddress.IsInsensitiveLike(criteria.Email.ToLower(), MatchMode.Anywhere));

            }


            if (!string.IsNullOrEmpty(criteria.CompanyName))
            {
                rootQuery.Where(() => userAlias.Company.IsInsensitiveLike(criteria.CompanyName.ToLower(), MatchMode.Anywhere));

            }

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                // rootQuery.Where(M =>( M.FirstName+" "+M.LastName).IsInsensitiveLike(criteria.Name, MatchMode.Anywhere));


                rootQuery.Where(NHibernate.Criterion.Expression.Sql("CONCAT(UPPER(useralias1_.FirstName), ' ', UPPER(useralias1_.LastName)) like UPPER(?)", "%" + criteria.Name + "%", NHibernateUtil.String));
                //rootQuery.Where(M => M.LastName.IsInsensitiveLike(criteria.Name, MatchMode.Anywhere));
            }
            if (criteria.AccountId.HasValue)
            {
                rootQuery.Where(() => AccountAlias.ID == criteria.AccountId.Value);

            }
            if (criteria.Role>0)
            {
            
                rootQuery.Where(() => AccountAlias.AccountRole == (AccountRole)Enum.Parse(typeof(AccountRole), criteria.Role.ToString()));

            }
            if (criteria.hideCurrentUser)
            {

                rootQuery.Where(() => AccountAlias.ID != OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId);

            }
            if (criteria.hideNonPrimary)
            {
                var subqueryPrimaryUser = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.Account.Account>()
    .Where(gm => gm.PrimaryUser.ID == userAlias.ID)
    .Select(gm => gm.ID);
                rootQuery.WithSubquery.WhereExists(subqueryPrimaryUser);

            }

            if (criteria.publisherUsers)
            {

                // rootQuery.JoinAlias(() => userAlias.Account, () => AccountAlias);

                var appSitesAccounts = QueryOver.Of<ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite>()
    .Where(gm => gm.Account.ID == AccountAlias.ID)
    .Select(gm => gm.ID);
                rootQuery.WithSubquery.WhereExists(appSitesAccounts);

            }



            var CountQuery = rootQuery.ToRowCountQuery();
            if (criteria.Page >= 0)
            {
                var pageIndex = criteria.Page;
                rootQuery.OrderBy(item => item.ID);
                rootQuery.Skip(pageIndex * criteria.Size)
                                         .Take(criteria.Size);
                rootQuery.OrderBy(() => userAlias.FirstName);
            }
            else
            {

                rootQuery.OrderBy(() => userAlias.ID);
            }

            Count = CountQuery.SingleOrDefault<int>();
            return rootQuery.List<ArabyAds.AdFalcon.Domain.Model.Account.Account>();


        }
    }
}
