using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Response;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class KPIService : IKPIService
    {
        private IKPIConfigRepository _KPIConfigRepository;

        public KPIService(IKPIConfigRepository kPIConfigRepository)
        {
            _KPIConfigRepository = kPIConfigRepository;
        }
        public IList<KPIConfigDto> GetKPIConfigsForCampaigns(ValueMessageWrapper<bool> isDefault)
        {
            var configs = _KPIConfigRepository.Query(m => m.IsDefault == isDefault.Value && m.ForCampaign);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsForAdvertisers(ValueMessageWrapper<bool> isDefault)
        {
            var configs = _KPIConfigRepository.Query(m => m.IsDefault == isDefault.Value && m.ForAdvertiser);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsForDeals(ValueMessageWrapper<bool> isDefault)
        {
            var configs = _KPIConfigRepository.Query(m => m.IsDefault == isDefault.Value && m.ForDeals);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsForPublishers(ValueMessageWrapper<bool> isDefault)
        {
            var configs = _KPIConfigRepository.Query(m => m.IsDefault == isDefault.Value && m.ForPublisher);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsForDataProviders(ValueMessageWrapper<bool> isDefault)
        {
            var configs = _KPIConfigRepository.Query(m => m.IsDefault == isDefault.Value && m.ForDataProvider);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }



        public IList<KPIConfigDto> GetKPIConfigsAllForCampaigns()
        {
            var configs = _KPIConfigRepository.Query(m =>  m.ForCampaign);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsAllForAdvertisers()
        {
            var configs = _KPIConfigRepository.Query(m =>  m.ForAdvertiser);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsAllForDeals()
        {
            var configs = _KPIConfigRepository.Query(m =>  m.ForDeals);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsAllForPublishers()
        {
            var configs = _KPIConfigRepository.Query(m => m.ForPublisher);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }

        public IList<KPIConfigDto> GetKPIConfigsAllForDataProviders()
        {
            var configs = _KPIConfigRepository.Query(m =>  m.ForDataProvider);
            return MapperHelper.Map<IList<KPIConfigDto>>(configs);
        }




        public KPIConfigDto GetKPIConfig(ValueMessageWrapper<int> id) 
        {
            var config = _KPIConfigRepository.Get(id.Value);
            return MapperHelper.Map<KPIConfigDto>(config);
        }

        public IList<KPIDTO> GetKPIs(GetKPIRequest request)
        {
            IList<KPIDTO> kpis = new List<KPIDTO>();
            IDictionary<string, (decimal?, double?)> dbKpis; 
            switch (request.KPIScope) {

                case KPIScope.Campaign:
                    dbKpis = _KPIConfigRepository.GetKPIsForCampaigns(request.DataBaseFields, request.FromDate, request.ToDate, request.AccountId, request.Ids);
                    break;
                case KPIScope.AdGroup:
                    dbKpis = _KPIConfigRepository.GetKPIsForAdGroups(request.DataBaseFields, request.FromDate, request.ToDate, request.AccountId, request.Ids);
                    break;
                case KPIScope.Ads:
                    dbKpis = _KPIConfigRepository.GetKPIsForAds(request.DataBaseFields, request.FromDate, request.ToDate, request.AccountId, request.Ids);
                    break;
                case KPIScope.Advertiser:
                    dbKpis = _KPIConfigRepository.GetKPIsForAdvertisers(request.DataBaseFields, request.FromDate, request.ToDate, request.AccountId, request.Ids);
                    break;
                case KPIScope.DataProvider:
                    dbKpis = _KPIConfigRepository.GetKPIsForDataProviders(request.DataBaseFields, request.FromDate, request.ToDate, request.AccountId, request.Ids);
                    break;
                case KPIScope.Deals:
                    dbKpis = _KPIConfigRepository.GetKPIsForDeals(request.DataBaseFields, request.FromDate, request.ToDate, request.AccountId,  request.Ids);
                    break;
                case KPIScope.Publisher:
                    dbKpis = _KPIConfigRepository.GetKPIsForPublishers(request.DataBaseFields, request.FromDate, request.ToDate, request.AccountId, request.Ids);
                    break;
                default:
                    dbKpis = new Dictionary<string, (decimal?, double?)>();
                    break;

            }

            foreach ((string column, (decimal? kpival, double? kpiGr)) in dbKpis)
            {
                kpis.Add(new KPIDTO { DBField= column, MetricValue= kpival, MetricGrowthValue = kpiGr });
            }

            return kpis;
        }
    }
}
