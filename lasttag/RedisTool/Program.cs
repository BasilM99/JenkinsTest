
using NHibernate;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.Persistence.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisTool
{
    //class Program
    //{
    //    static void menu()
    //    {
    //        Console.WriteLine();
    //        Console.WriteLine("Main menu :- ");
    //        Console.WriteLine();

    //        Console.WriteLine("\t 1 : delete all keys ");
    //        Console.WriteLine("\t 2 : delete a apecific pattern keys ");
    //        Console.WriteLine("\t 3 : exit ");
    //        Console.WriteLine();
    //        Console.WriteLine("please choose an option ");


    //    }
    //    static void DeleteKey()
    //    {
    //        Console.WriteLine();

    //        Console.WriteLine("please enter a pattern ");

    //        string input = Console.ReadLine();
    //        if (!string.IsNullOrEmpty(input))
    //        {
    //           // _redis = new StackExchange.Redis.Extensions.Core.StackExchangeRedisCacheClient(new ProtobufSerializer());
    //            var keys = _redis.SearchKeys(string.Format("{0}*", input));
    //            if (keys != null && keys.Count() > 0)
    //            {
    //                _redis.RemoveAll(keys);
    //                runProgress();

    //                Console.WriteLine("pattern keys removed");

    //            }
    //            else
    //            {
    //                Console.WriteLine("Pattern not found");
    //            }

    //            Thread.Sleep(50);
    //            menu();
    //            _redis.Dispose();
    //        }
    //    }
    //    static void runProgress()
    //    {
    //        using (var progress = new ProgressBar())
    //        {
    //            for (int i = 0; i <= 100; i++)
    //            {
    //                progress.Report((double)i / 100);
    //                Thread.Sleep(20);
    //            }
    //        }
    //    }

    //    static void DeleteAll()
    //    {

    //       // _redis = new StackExchange.Redis.Extensions.Core.StackExchangeRedisCacheClient(new ProtobufSerializer());
    //        var keys = _redis.SearchKeys(string.Format("*"));
    //        if (keys != null && keys.Count() > 0)
    //        {
    //            _redis.RemoveAll(keys);
    //            runProgress();

    //            Console.WriteLine("All keys removed ");

    //        }
    //        else
    //        {
    //            Console.WriteLine("No keys ");
    //        }

    //        Thread.Sleep(50);
    //        menu();
    //        _redis.Dispose();

    //    }
    //    private static StackExchange.Redis.Extensions.Core.StackExchangeRedisCacheClient _redis = null;
    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            menu();
    //            while (true)
    //            {

    //                string input = Console.ReadLine();
    //                if (!string.IsNullOrEmpty(input) && Convert.ToInt16(input) > 0 && Convert.ToInt16(input) < 4)
    //                {
    //                    switch (input)
    //                    {
    //                        case "1":
    //                            DeleteAll();
    //                            break;
    //                        case "2":
    //                            DeleteKey();
    //                            break;
    //                        case "3":
    //                            return;

    //                        default:
    //                            break;
    //                    }
    //                }
    //                else
    //                {
    //                    Console.WriteLine("wrong input ! please try again ");
    //                    Thread.Sleep(50);
    //                    menu();
    //                }
    //            }

    //        }
    //        catch (Exception e)
    //        {

    //            throw e;
    //        }

    //    }

    //}
    class Program
    {

        static void Initializer()
        {

            Assembly[] assemblies = new Assembly[] { Assembly.Load("Noqoush.AdFalcon.Persistence"), Assembly.Load("Noqoush.Framework.DomainServices") };
            var UoWFactory = new NHibernateUnitOfWorkFactory();
            UoWFactory.Initialize(assemblies);
            UnitOfWork.SetUnitOfWorkFactory(UoWFactory);


            UnitOfWork.Create();

        }

        static IList<AdGroup> GetAdGroups()
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            //IQueryOver<AudienceSegmentTargeting, AudienceSegmentTargeting> rootQuery = nhibernateSession.QueryOver<AudienceSegmentTargeting>();

            //rootQuery.Where(x => !x.IsDeleted);
            //rootQuery.JoinQueryOver<AdCreative>(x => x.AdGroup.Ads);
            //rootQuery.Where(x => x.AdGroup.Ads.Count() > 0);
            //rootQuery.Select(x => x.AdGroup);

            //IList<AdGroup> results = rootQuery.List<AdGroup>();


            AudienceSegmentTargeting AudienceSegmentTargetingAlias = null;
            //AdCreative AdCreativeAlias = null;
            AdGroup AdGroupAlias = null;

            IQueryOver<AudienceSegmentTargeting, AudienceSegmentTargeting> catQuery =
                nhibernateSession.QueryOver<AudienceSegmentTargeting>(() => AudienceSegmentTargetingAlias)
                    .JoinAlias(() => AudienceSegmentTargetingAlias.AdGroup, () => AdGroupAlias)
                   //.Where(() => AdGroupAlias.Ads.Count() > 0)
                   .Select(x => x.AdGroup);

            return catQuery.List<AdGroup>();

        }

        static void Main(string[] args)
        {
            Initializer();

            IList<AdGroup> AdGroups = GetAdGroups();
            foreach (AdGroup adGroup in AdGroups)
            {
                adGroup.SetAdsMaxDataBid();
             
                 UnitOfWork.Current.Save(adGroup);

            }

             UnitOfWork.Current.Commit();


        }

    }


}
