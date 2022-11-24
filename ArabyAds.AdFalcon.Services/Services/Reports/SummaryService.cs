using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Persistence.Reports.Repositories;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Services.Services.Reports
{
    public class SummaryService : ISummaryService
    {
        ISummaryRepository _SummaryRepository;

        public SummaryService(ISummaryRepository summaryRepository)
        {
            this._SummaryRepository = summaryRepository;
        }

        public ValueMessageWrapper<decimal> GetAccountTotalRevenue(FromToDateMessage request)
        {
            return ValueMessageWrapper.Create(_SummaryRepository.GetAccountTotalRevenue(request.FromDate, request.ToDate, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value));
        }

        public ValueMessageWrapper<decimal> GetAccountTotalSpend(FromToDateMessage request)
        {
            return ValueMessageWrapper.Create(_SummaryRepository.GetAccountTotalSpend(request.FromDate, request.ToDate, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value));
        }
        public ValueMessageWrapper<decimal> GetAdvertiserAccountTotalSpend(GetAdvertiserAccountTotalSpendRequest request)
        {
            return ValueMessageWrapper.Create(_SummaryRepository.GetAdvertiserAccountTotalSpend(request.FromDate, request.ToDate, request.Id));
        }

    }
}
