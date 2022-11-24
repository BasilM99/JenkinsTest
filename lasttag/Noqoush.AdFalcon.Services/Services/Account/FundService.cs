using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.AdFalcon;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using NHibernate;
using Noqoush.Framework.Persistence;
using NHibernate.Transform;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Services.Services
{
    public class FundsService : IFundsService
    {
        IFundsRepository _FundsRepository;

        public FundsService(IFundsRepository fundsRepository)
        {
            this._FundsRepository = fundsRepository;
        }

        public FundResultDto GetAccountFundsHistory(HistoryCriteriaDto fundsCriteria)
        {
            var result = new FundResultDto();
            IEnumerable<Domain.Model.Account.AccountFundTransHistory> list = _FundsRepository.Query((p => p.AccountId == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && (p.CreationDate >= fundsCriteria.FromDate && p.CreationDate <= fundsCriteria.ToDate) && (p.FundTransStatus.ID == (int)AccountFundTransStatusIds.Committed)), fundsCriteria.PageNumber, fundsCriteria.ItemsPerPage, (p => p.TransactionDate), fundsCriteria.Ascending);
            result.Items = list.Select(p => MapperHelper.Map<FundTransactionDto>(p)).ToList();

            result.Total = _FundsRepository.Query((p => p.AccountId == OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value && (p.CreationDate >= fundsCriteria.FromDate && p.CreationDate <= fundsCriteria.ToDate) && (p.FundTransStatus.ID == (int)AccountFundTransStatusIds.Committed))).Count();

            return result;
        }
        public FundResultDto GetTransactionVATHistory(Noqoush.AdFalcon.Domain.Common.Repositories.Account.TransactionVATCriteria   wcriteria)
        {

            TransactionVATCriteria criteria = new TransactionVATCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new FundResultDto();
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ISQLQuery query = null;

            if (!criteria.Payments)
            {
                query =
                        nhibernateSession.CreateSQLQuery(
                            "call sp_FundTransactionsVAT(:AccountId,:PageIndex,:SizeTake,:FromDate,:ToDate)");
                if (criteria.Details)
                {
                    query =
                    nhibernateSession.CreateSQLQuery(
                        "call sp_FundTransactionsVATDetailed(:AccountId,:PageIndex,:SizeTake,:FromDate,:ToDate)");
                }
            }
            else
            {

                query =
                    nhibernateSession.CreateSQLQuery(
                        "call sp_PaymentTransactionsVAT(:AccountId,:PageIndex,:SizeTake,:FromDate,:ToDate)");
                if (criteria.Details)
                {
                    query =
                    nhibernateSession.CreateSQLQuery(
                        "call sp_PaymentTransactionsVATDetailed(:AccountId,:PageIndex,:SizeTake,:FromDate,:ToDate)");
                }
            }
            query.SetTimeout(500);
            if (criteria.DataFrom.HasValue)
            {
                query.SetDateTime("FromDate", criteria.DataFrom.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("FromDate", null);
            }

            if (criteria.DataTo.HasValue)
            {
                query.SetDateTime("ToDate", criteria.DataTo.Value);
            }
            else
            {
                query.SetParameter<DateTime?>("ToDate", null);
            }
            if (criteria.AccountId.HasValue)
            {
                query.SetInt32("AccountId", criteria.AccountId.Value);
            }
            else
            {
                query.SetParameter<int?>("AccountId", null);
            }

            if (criteria.Page.HasValue)
            {
                query.SetInt32("PageIndex", criteria.Size*(criteria.Page.Value-1));
                query.SetInt32("SizeTake", criteria.Size);
            }
            else
            {
                query.SetParameter<int?>("SizeTake", null);
                query.SetParameter<int?>("PageIndex", null);
            }

            query.SetResultTransformer(Transformers.AliasToBean<FundTransactionDto>());
            var itemsr = query.List<FundTransactionDto>().ToList() ?? new List<FundTransactionDto>();
            result.Items = itemsr;
            if(itemsr!=null && itemsr.Count>0 )
            result.Total = (int)itemsr[0].TotalCount;
            return result;
        }
    
    }
}
