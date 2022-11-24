using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Persistence.Reports.Repositories;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;

namespace Noqoush.AdFalcon.Services.Services.Reports
{
    public class SummaryService : ISummaryService
    {
        ISummaryRepository _SummaryRepository;

        public SummaryService(ISummaryRepository summaryRepository)
        {
            this._SummaryRepository = summaryRepository;
        }

        public decimal GetAccountTotalRevenue(DateTime fromDate, DateTime toDate)
        {
            return _SummaryRepository.GetAccountTotalRevenue(fromDate, toDate, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }

        public decimal GetAccountTotalSpend(DateTime fromDate, DateTime toDate)
        {
            return _SummaryRepository.GetAccountTotalSpend(fromDate, toDate, OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
        }
        public decimal GetAdvertiserAccountTotalSpend(DateTime fromDate, DateTime toDate, int Id)
        {
            return _SummaryRepository.GetAdvertiserAccountTotalSpend(fromDate, toDate, Id);
        }

    }
}
