using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface IKPIConfigRepository : IKeyedRepository<KPIConfig, int>
    {

        IDictionary<string, (decimal?, double?)> GetKPIsForDeals(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids);

        IDictionary<string, (decimal?, double?)> GetKPIsForPublishers(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids);

        IDictionary<string, (decimal?, double?)> GetKPIsForDataProviders(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids);

        IDictionary<string, (decimal?, double?)> GetKPIsForCampaigns(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids);

        IDictionary<string, (decimal?, double?)> GetKPIsForAdvertisers(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids);

        IDictionary<string, (decimal?, double?)> GetKPIsForAds(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids);
IDictionary<string, (decimal?, double?)> GetKPIsForAdGroups(IList<string> dataBaseFields, DateTime from, DateTime to, int? accountId, params int[] ids);


    }
    public interface IReportCriteriaRepository : IKeyedRepository<ReportCriteria, int>
    {
    }
    public interface IDashBoardCriteriaRepository : IKeyedRepository<DashBoardCriteria, int>
    {
    }
    
}
