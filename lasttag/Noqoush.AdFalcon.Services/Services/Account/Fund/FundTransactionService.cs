using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Model.Account.Fund;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Exceptions;
using Noqoush.AdFalcon.Exceptions.Core;
using Noqoush.AdFalcon.Exceptions.Fund;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.EventBroker.Context;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Utilities.EmailsSender;
using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Services.Services.Account.Fund
{
    public class FundTransactionService : IFundTransactionService
    {
        private readonly IAccountFundTransHistoryRepository _fundTransRepository = null;
        private readonly IAccountFundPgwRepository _pgwRepository = null;
        private readonly IAccountFundTransStatusRepository _accountFundTransStatusRepository;
        private readonly IAccountRepository _accountRepository = null;
        private readonly IConfigurationManager _configurationManager = null;
        private IDocumentRepository _documentRepository;
        private readonly IAccountPaymentDetailsRepository _accountPaymentDetailsRepository = null;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IAccountFundTransTypeRepository _accountFundTransTypeRepository;
        private readonly IAccountFundTypeRepository _accountFundTypeRepository;
        private ICampaignRepository _CampaignRepository;
        private IMailSender _mailSender;
        private IAdGroupRepository _adGroupRepository;
        static readonly object LockObj = new object();


        public FundTransactionService(
            IAccountFundTransHistoryRepository fundTransRepository,
            IAccountFundPgwRepository pgwRepository,
            IAccountRepository accountRepository,
            IConfigurationManager configurationManager,
            IDocumentRepository documentRepository,
            IAccountPaymentDetailsRepository accountPaymentDetailsRepository,
            ICurrencyRepository currencyRepository,
            IAccountFundTransStatusRepository accountFundTransStatusRepository,
            IAccountFundTransTypeRepository accountFundTransTypeRepository,
            IAccountFundTypeRepository accountFundTypeRepository,
                        ICampaignRepository CampaignRepository,
                        IMailSender mailSender, IAdGroupRepository adGroupRepository
    )
        {
            _fundTransRepository = fundTransRepository;
            _pgwRepository = pgwRepository;
            _accountRepository = accountRepository;
            _configurationManager = configurationManager;
            _documentRepository = documentRepository;
            _accountPaymentDetailsRepository = accountPaymentDetailsRepository;
            _currencyRepository = currencyRepository;
            _accountFundTransStatusRepository = accountFundTransStatusRepository;
            _accountFundTransTypeRepository = accountFundTransTypeRepository;
            _accountFundTypeRepository = accountFundTypeRepository;
            this._CampaignRepository = CampaignRepository;
            _mailSender = mailSender;
            _adGroupRepository = adGroupRepository;

        }

        private string GetReceiptNumber()
        {
            //TODO:Osaleh to rewrite this code
            // generate Receipt Number
            return string.Format("{0}/{1}/{2}", Framework.Utilities.Environment.GetServerTime().Year, ApplicationPrefix, GetCounter());
        }
        private int GetCounter()
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByYear(:YearId,:CounterName)");
            query.SetString("CounterName", "Fund");
            query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);
        }
        private string ApplicationPrefix
        {
            get
            {
                const string key = "ApplicationPrefix-CacheKey";
                var value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                if (string.IsNullOrWhiteSpace(value))
                {
                    lock (LockObj)
                    {
                        value = Framework.Caching.CacheManager.Current.DefaultProvider.Get<string>(key);
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            value = _configurationManager.GetConfigurationSetting(null, null, "ApplicationPrefix");
                            Framework.Caching.CacheManager.Current.DefaultProvider.Put(key, value);
                        }
                    }
                }
                return value;
            }
        }
        private void CheckAccountFundTransHistory(AccountFundTransHistory item)
        {
            if (item == null)
            {
                throw new DataNotFoundException();
            }
            if ((!OperationContext.Current.UserInfo<AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountId.HasValue) || (item.AccountId != OperationContext.Current.UserInfo<AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountId.Value))
            {
                throw new AccountNotValidException();
            }
        }
        /// <summary>
        /// Get Transaction details for the given id.
        /// </summary>
        /// <returns></returns>
        public FundTransactionDto GetFundTransactionById(int id)
        {
            var item = _fundTransRepository.Get(id);
            CheckAccountFundTransHistory(item);
            return MapperHelper.Map<FundTransactionDto>(item);
        }
        public FundTransactionDto GetFundTransactionByref(string refId)
        {
            var item = _fundTransRepository.Query(M=>M.TransactionId== refId).SingleOrDefault();
            CheckAccountFundTransHistory(item);
            return MapperHelper.Map<FundTransactionDto>(item);
        }

        public void UpdateFundTransactionByref(int Id ,string refId)
        {
            var item = _fundTransRepository.Get( Id);
            CheckAccountFundTransHistory(item);
            item.TransactionId = refId;
            _fundTransRepository.Save(item);
        }
        public int InitiateFundTransaction(FundTransactionDto data)
        {
            AccountFundTransHistory fundTrans;

            switch (data.FundTransType.ID)
            {
                case (int)AccountFundTransTypeIds.CreditCard:
                    fundTrans = AutoMapper.Mapper.DynamicMap<FundTransactionDto, AccountFundTransHistoryPgw>(data);
                    fundTrans.OriginalAmount = data.Amount;
                    fundTrans.Currency = Domain.Model.Core.Currency.USD;
                    (fundTrans as AccountFundTransHistoryPgw).SystemPaymentDetail = Domain.Model.Core.SystemPaymentDetails.CreditCard;
                    break;
                case (int)AccountFundTransTypeIds.PayPal:
                    fundTrans = AutoMapper.Mapper.DynamicMap<FundTransactionDto, AccountFundTransHistoryPaypal>(data);
                    fundTrans.OriginalAmount = data.Amount;
                    fundTrans.Currency = Domain.Model.Core.Currency.USD;
                    (fundTrans as AccountFundTransHistoryPaypal).SystemPaymentDetail = Domain.Model.Core.SystemPaymentDetails.PayPal;
                    break;
                default:
                    fundTrans = AutoMapper.Mapper.DynamicMap<FundTransactionDto, AccountFundTransHistory>(data);
                    break;
            }


            fundTrans.AccountFundType = _accountFundTypeRepository.Get(data.FundType.ID);
            // current date and time.
            fundTrans.CreationDate = Framework.Utilities.Environment.GetServerTime();

            // save the transaction 
            _fundTransRepository.Save(fundTrans);

            // return the generated transaction ID.
            return fundTrans.ID;
        }

        /// <summary>
        /// Close an opened fund transaction.
        /// </summary>
        /// <param name="fundTransactionDTO"> The response data</param>
        /// <returns></returns>
        public bool CloseFundTransaction(FundTransactionResponseDto fundTransactionDTO)
        {
            // Get the transaction original record
            AccountFundTransHistory transHist = _fundTransRepository.Get(fundTransactionDTO.ID);
            if (transHist == null)
                throw new TransactionNotFoundExpectation(new List<ErrorData>()
                                                             {
                                                                 new ErrorData(message:string.Format(Framework.Resources.ResourceManager.Instance.GetResource("TransactionNotFoundBR", "Global"),fundTransactionDTO.ID.ToString()))
                                                             });

            // TODO!!: The below is commented out until adFalcon decide the correct behavior, 
            // this behavior could be happened when the user stay long time before finishing the payment on the pgw, 
            // meanwhile the windows service might already closed the transaction as “Failed”.

            // if the transaction already has response, then do not proceed.
            if (transHist.FundTransStatus.ID != AccountFundTransStatus.Pending.ID)
                throw new TransactionAlreadyClosedExpectation(new List<ErrorData>()
                                                                  {
                                                                      new ErrorData(
                                                                          message:string.Format(Framework.Resources.ResourceManager.Instance.GetResource("TransactionAlreadyClosedBR","Global"), fundTransactionDTO.ID.ToString()))
                                                                  });
            decimal CommingAmount = fundTransactionDTO.Amount;
            // Fill before mapping
            fundTransactionDTO.TransactionDate = transHist.TransactionDate;
            fundTransactionDTO.Amount = transHist.Amount;
            fundTransactionDTO.VATAmount = transHist.VATAmount;
            //if (fundTransactionDTO.Amount == 0)
            //{
            //    fundTransactionDTO.Amount = transHist.Amount;
            //    fundTransactionDTO.VATAmount = transHist.VATAmount;
            //}
            //else
            //{
            //    fundTransactionDTO.VATAmount = transHist.VATAmount;
            //}

            switch (transHist.FundTransType.ID)
            {
                case (int)AccountFundTransTypeIds.CreditCard:
                    AutoMapper.Mapper.DynamicMap<PgwFundTransactionResponseDto, AccountFundTransHistoryPgw>(fundTransactionDTO as PgwFundTransactionResponseDto, transHist as AccountFundTransHistoryPgw);
                    break;
                default:

                    AutoMapper.Mapper.DynamicMap<PayPalFundTransactionResponseDto, AccountFundTransHistoryPaypal>(fundTransactionDTO as PayPalFundTransactionResponseDto, transHist as AccountFundTransHistoryPaypal);
                    break;
            }


            if (fundTransactionDTO.FundTransStatus.ID == AccountFundTransStatus.Committed.ID)
            {
                // Check if the provided critical data match the initiated data, if not match then do not commit the transaction.
                if ((transHist.Amount + transHist.VATAmount) != (CommingAmount))
                {
                    throw new TransactionAmountExpectation(new List<ErrorData>()
                                                           {
                                                               new ErrorData(
                                                                   message:string.Format(Framework.Resources.ResourceManager.Instance.GetResource("TransactionAmountBR","Global"), fundTransactionDTO.ID.ToString()))
                                                           });
                }

                if (transHist.FundTransType.ID == (int)AccountFundTransTypeIds.PayPal)
                {
                    string emailAddress = (fundTransactionDTO as PayPalFundTransactionResponseDto).EmailAddress;

                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        Domain.Model.Account.Account account = _accountRepository.Get(transHist.AccountId);

                        AccountPaymentDetails accountPaymentDetails = new PayPalAccountPaymentDetails();
                        (accountPaymentDetails as PayPalAccountPaymentDetails).UserName = emailAddress;
                        accountPaymentDetails.AccountType = PayemntAccountType.PayPal;
                        accountPaymentDetails.SubType = PayemntAccountSubType.Payment;
                        var item = account.GetPayPalAccount(accountPaymentDetails as PayPalAccountPaymentDetails);
                        if (item != null)
                            accountPaymentDetails = item;

                        account.AddAccountPaymentDetailSystem(accountPaymentDetails);

                        (transHist as AccountFundTransHistoryPaypal).AccountPaymentDetail = accountPaymentDetails as PayPalAccountPaymentDetails;
                    }

                }
                // update Account Total Fund 
                var accountObj = _accountRepository.Get(transHist.AccountId);
                accountObj.AddFund(transHist.Amount);
                _accountRepository.Save(accountObj);
                // accountObj.PublishAccountAmountForKafka();
                // generate Receipt Number
                transHist.NoqoushReceiptNumber = GetReceiptNumber();
                transHist.TransactionId = transHist.TransactionId;
                transHist.FundTransStatus = _accountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Committed);
            }
            else
            {
                transHist.FundTransStatus = _accountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Failed);
            }
            // save the updated object.
            _fundTransRepository.Save(transHist);
            return transHist.FundTransStatus.ID == AccountFundTransStatus.Committed.ID;
        }

        /// <summary>
        /// returns all active PGWs
        /// </summary>
        /// <returns></returns>
        public ICollection<PgwDto> GetRegistredPGWs()
        {
            var result = (from pgw in _pgwRepository.Query(x => x.IsDeleted == false) select AutoMapper.Mapper.DynamicMap<AccountFundPgw, PgwDto>(pgw)).ToList();
            return result;
        }

        /// <summary>
        /// Get all pending transaction that happened after the given date and for the given PGW.
        /// </summary>
        /// <param name="fundTransactionTypeId">Payment gateway id</param>
        /// <param name="dtTo">Get pending old transaction up to this date</param>
        /// <returns></returns>
        public IList<FundTransactionResponseDto> GetPendingFundTransactions(int fundTransactionTypeId, DateTime dtTo)
        {
            var result = _fundTransRepository.Query(x => x.FundTransStatus.ID == (int)AccountFundTransStatusIds.Pending && x.TransactionDate <= dtTo && x.FundTransType.ID == fundTransactionTypeId).ToList().OrderByDescending(p => p.CreationDate);

            if (fundTransactionTypeId == AccountFundTransType.CreditCard.ID)
            {
                return result.Select(p => MapperHelper.Map<PgwFundTransactionResponseDto>(p)).Cast<FundTransactionResponseDto>().ToList();
            }
            else
            {
                return result.Select(p => MapperHelper.Map<PayPalFundTransactionResponseDto>(p)).Cast<FundTransactionResponseDto>().ToList();
            }
        }

        /// <summary>
        /// Returns information for the provided pgw id.
        /// </summary>
        /// <param name="id">Payment gateway id</param>
        /// <returns></returns>
        public PgwDto GetPgwInfo(int id)
        {
            return AutoMapper.Mapper.DynamicMap<AccountFundPgw, PgwDto>(_pgwRepository.Get(id));
        }

        /// Returns information for the provided pgw code.
        /// </summary>
        /// <param name="id">Payment gateway code</param>
        /// <returns></returns>
        public PgwDto GetPgwInfoByCode(string code)
        {
            AccountFundPgw fundPgw = _pgwRepository.Query(p => p.Code == code && !p.IsDeleted).LastOrDefault();

            if (fundPgw == null) { return null; }
            else if(fundPgw.Code.ToLower()== "migs")
            {
            var Settings=  fundPgw.Settings;
            }
            return AutoMapper.Mapper.DynamicMap<AccountFundPgw, PgwDto>(fundPgw);
        }

        public void AddFund(NewFundDto fundDto)
        {
            var error = fundDto.Validate();

            Domain.Model.Account.Account currentAccount = _accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalAccountId.Value);

            if ((!fundDto.AccountId.HasValue) || (!fundDto.FundType.HasValue))
                throw error;

            Domain.Model.Account.Account account = _accountRepository.Get(fundDto.AccountId.Value);

            AccountFundTransHistory fund = null;
            var valid = true;
            switch ((AccountFundTransTypeIds)fundDto.FundType)
            {
                case AccountFundTransTypeIds.FundTransfer:
                    {
                        fund = new AccountFundTransHistory
                        {
                            FundTransType = AccountFundTransType.FundTransfer,
                        };
                        break;
                    }
                case AccountFundTransTypeIds.Cash:
                    {
                        fund = new AccountFundTransHistory
                        {
                            FundTransType = AccountFundTransType.Cash,
                        };
                        break;
                    }
                case AccountFundTransTypeIds.FreeCredit:
                    {
                        fund = new AccountFundTransHistory
                        {
                            FundTransType = AccountFundTransType.FreeCredit,
                        };
                        break;
                    }

                case AccountFundTransTypeIds.OverBudgetRefund:
                    {
                        fund = new AccountFundTransHistory
                        {
                            FundTransType = AccountFundTransType.OverBudgetRefund,
                        };
                        break;
                    }
                case AccountFundTransTypeIds.BalanceTransfer:
                    {
                        fund = new AccountFundTransHistory
                        {
                            FundTransType = AccountFundTransType.BalanceTransfer,
                        };
                        break;
                    }
                case AccountFundTransTypeIds.WireTransfer:
                    {
                        if (!fundDto.SystemPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "SystemPaymentDetailBR" });
                            valid = false;
                        }

                        var paymentAccount = GetPaymentAccount(fundDto, PayemntAccountType.Bank, account);
                        if (paymentAccount == null)
                        {
                            error.Errors.Add(new ErrorData { ID = "AccountPaymentDetailBR" });
                            valid = false;
                        }
                        else
                        {
                            var errors = paymentAccount.GetValidateErrors();
                            foreach (var errorData in errors)
                            {
                                error.Errors.Add(errorData);
                            }
                        }
                        if (valid)
                        {
                            fund = new AccountFundTransHistoryWire()
                            {
                                FundTransType = AccountFundTransType.WireTransfer,
                                SystemPaymentDetail = _accountPaymentDetailsRepository.Get(fundDto.SystemPaymentDetailId.Value) as BankAccountPaymentDetails,
                                AccountPaymentDetail = paymentAccount as BankAccountPaymentDetails
                            };
                        }
                        break;
                    }
                case AccountFundTransTypeIds.BankCheck:
                    {


                        if (!fundDto.SystemPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "SystemPaymentDetailBR" });
                            valid = false;
                        }
                        if (string.IsNullOrWhiteSpace(fundDto.IssuerName))
                        {
                            error.Errors.Add(new ErrorData { ID = "IssuerNameBR" });
                            valid = false;
                        }
                        if (string.IsNullOrWhiteSpace(fundDto.CheckNo))
                        {
                            error.Errors.Add(new ErrorData { ID = "CheckNoBR" });
                            valid = false;
                        }
                        if (!fundDto.DueDate.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "DueDateBR" });
                            valid = false;
                        }

                        if (string.IsNullOrWhiteSpace(fundDto.IssuerBankName))
                        {
                            error.Errors.Add(new ErrorData { ID = "IssuerBankNameBR" });
                            valid = false;
                        }
                        if (string.IsNullOrWhiteSpace(fundDto.IssuerBankBranch))
                        {
                            error.Errors.Add(new ErrorData { ID = "IssuerBankBranchBR" });
                            valid = false;
                        }
                        if (valid)
                        {
                            fund = new AccountFundTransHistoryCheck()
                            {
                                SystemPaymentDetail = _accountPaymentDetailsRepository.Get(fundDto.SystemPaymentDetailId.Value) as BankAccountPaymentDetails,
                                FundTransType = AccountFundTransType.Check,
                                IssuerName = fundDto.IssuerName,
                                IssuerBankName = fundDto.IssuerBankName,
                                IssuerBankBranch = fundDto.IssuerBankBranch,
                                CheckNo = fundDto.CheckNo,
                                DueDate = fundDto.DueDate.Value
                            };
                        }
                        break;
                    }
                case AccountFundTransTypeIds.PayPal:
                    {

                        if (!fundDto.SystemPaymentDetailId.HasValue)
                        {
                            error.Errors.Add(new ErrorData { ID = "SystemPaymentDetailBR" });
                            valid = false;
                        }
                        var paymentAccount = GetPaymentAccount(fundDto, PayemntAccountType.PayPal, account);
                        if (paymentAccount == null)
                        {
                            error.Errors.Add(new ErrorData { ID = "AccountPaymentDetailBR" });
                            valid = false;
                        }
                        else
                        {
                            var errors = paymentAccount.GetValidateErrors();
                            foreach (var errorData in errors)
                            {
                                error.Errors.Add(errorData);
                            }
                        }
                        if (valid)
                        {
                            fund = new AccountFundTransHistoryPaypal()
                            {
                                FundTransType = AccountFundTransType.PayPal,
                                SystemPaymentDetail = _accountPaymentDetailsRepository.Get(fundDto.SystemPaymentDetailId.Value) as PayPalAccountPaymentDetails,
                                AccountPaymentDetail = paymentAccount as PayPalAccountPaymentDetails
                            };
                        }
                        break;
                    }
                default:
                    {
                        var type = _accountFundTransTypeRepository.Get((int)fundDto.FundType);
                        if (type == null)
                        {
                            error.Errors.Add(new ErrorData { ID = "PaymentTypeBR" });
                            break;
                        }
                        else
                        {
                            fund = new AccountFundTransHistory
                            {
                                FundTransType = type
                            };
                        }
                        break;
                    }
            }
            if (error.Errors.Count > 0)
            {
                throw (error);
            }

            fund.AccountFundType = _accountFundTypeRepository.Get(fundDto.TypeId);
            var multiplier = 1;
            multiplier = fund.AccountFundType.Multiplier;
            fund.Amount = fundDto.Amount.Value * multiplier;

            fund.VATAmount = fundDto.VatAmount.HasValue ? fundDto.VatAmount.Value * multiplier : 0;
            fund.OriginalAmount = fundDto.OriginalAmount.HasValue ? fundDto.OriginalAmount.Value * multiplier : fund.Amount;




            fund.CreatedById = currentAccount.PrimaryUser.ID;
            fund.AccountId = fundDto.AccountId.Value;
            fund.CreationDate = DateTime.UtcNow;
            fund.TransactionId = fundDto.TransactionId;
            if ((AccountFundTransTypeIds)fundDto.FundType == AccountFundTransTypeIds.BankCheck)
            {
                fund.TransactionId = fundDto.CheckNo;
            }
            fund.Comment = fundDto.Comment;
            fund.TransactionDate = fundDto.TransactionDate.Value;
            fund.NoqoushReceiptNumber = GetReceiptNumber();
            fund.FundTransStatus = _accountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Committed);
            if (fundDto.CurrencyId.HasValue)
            {
                fund.Currency = _currencyRepository.Get(fundDto.CurrencyId.Value);
            }
            else
            {
                fund.Currency = Domain.Model.Core.Currency.USD;
            }

            int attachmentId;
            if (int.TryParse(fundDto.AttachmentId, out attachmentId))
            {
                fund.Attachment = _documentRepository.Get(attachmentId);
                fund.Attachment.UpdateUsage();
            }

            account.AddFund(fund.Amount);
            _accountRepository.Save(account);
            _fundTransRepository.Save(fund);
            //    account.PublishAccountAmountForKafka();
            #region EventBroker

            Dictionary<string, object> extraParameter = new Dictionary<string, object>();
            extraParameter.Add("Id", fund.ID);
            extraParameter.Add("NotifyUser", fundDto.NotifyUser);

            EventBrokerContext.Current.ExtraParameters.Add(extraParameter);

            #endregion
        }

        public void AddOverBudgetReturnFundFromCampaign(int campaignId, decimal invoicedAmount)
        {

            var Campaign = _CampaignRepository.Get(campaignId);
            if (invoicedAmount > 0)
            {
                invoicedAmount = invoicedAmount * 1;
            }
            if (Campaign != null)
            {
                NewFundDto fundDto = new NewFundDto();
                fundDto.AccountId = Campaign.Account.ID;
                fundDto.Amount = invoicedAmount;
                fundDto.TransactionDate = Framework.Utilities.Environment.GetServerTime();
                fundDto.ObjectRelatedId = campaignId;
                fundDto.TypeId = AccountFundType.Fund.ID;
                var error = fundDto.Validate();

                Domain.Model.Account.Account currentAccount = _accountRepository.Get(fundDto.AccountId.Value);

                Domain.Model.Account.Account account = _accountRepository.Get(fundDto.AccountId.Value);

                AccountFundTransHistory fund = null;

                fund = new AccountFundTransHistory
                {
                    FundTransType = AccountFundTransType.OverBudgetRefund,
                };


                fund.AccountFundType = _accountFundTypeRepository.Get((int)AccountFundTypeIds.Fund);
                var multiplier = 1;
                multiplier = fund.AccountFundType.Multiplier;
                fund.Amount = fundDto.Amount.Value * multiplier;

                fund.VATAmount = fundDto.VatAmount.HasValue ? fundDto.VatAmount.Value * multiplier : 0;
                fund.OriginalAmount = fundDto.OriginalAmount.HasValue ? fundDto.OriginalAmount.Value * multiplier : fund.Amount;

                fund.CreatedById = currentAccount.PrimaryUser.ID;
                fund.AccountId = fundDto.AccountId.Value;
                fund.CreationDate = DateTime.UtcNow;
                fund.TransactionId = fundDto.TransactionId;

                fund.Comment = fundDto.Comment;
                fund.TransactionDate = fundDto.TransactionDate.Value;
                fund.NoqoushReceiptNumber = GetReceiptNumber();
                fund.ObjectRelatedId = fundDto.ObjectRelatedId;
                fund.FundTransStatus = _accountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Committed);
                if (fundDto.CurrencyId.HasValue)
                {
                    fund.Currency = _currencyRepository.Get(fundDto.CurrencyId.Value);
                }
                else
                {
                    fund.Currency = Domain.Model.Core.Currency.USD;
                }

                int attachmentId;
                if (int.TryParse(fundDto.AttachmentId, out attachmentId))
                {
                    fund.Attachment = _documentRepository.Get(attachmentId);
                    fund.Attachment.UpdateUsage();
                }

                account.AddFund(fund.Amount);
                _accountRepository.Save(account);
                _fundTransRepository.Save(fund);


                // account.PublishAccountAmountForKafka();

                #region EventBroker

                Dictionary<string, object> extraParameter = new Dictionary<string, object>();
                extraParameter.Add("Id", fund.ID);
                extraParameter.Add("NotifyUser", fundDto.NotifyUser);
                extraParameter.Add("ObjectRelatedId", fundDto.ObjectRelatedId.Value);
                extraParameter.Add("CampaignName", Campaign.Name);
                EventBrokerContext.Current.ExtraParameters.Add(extraParameter);
                #endregion
            }



        }

        public void AddOverBudgetReturnFundFromAccount(int AccountId, decimal invoicedAmount)
        {

            var account = _accountRepository.Get(AccountId);
            if (invoicedAmount > 0)
            {
                invoicedAmount = invoicedAmount * 1;
            }
            if (account != null)
            {

                var fundTypeOb = _accountFundTypeRepository.Get((int)AccountFundTypeIds.Fund);

                NewFundDto fundDto = new NewFundDto();
                fundDto.AccountId = account.ID;
                fundDto.Amount = invoicedAmount;
                fundDto.TransactionDate = Framework.Utilities.Environment.GetServerTime();
                //fundDto.ObjectRelatedId = campaignId;
                fundDto.TypeId = AccountFundType.Fund.ID;
                var error = fundDto.Validate();

                Domain.Model.Account.Account currentAccount = _accountRepository.Get(fundDto.AccountId.Value);

                // Domain.Model.Account.Account account = _accountRepository.Get(fundDto.AccountId.Value);

                AccountFundTransHistory fund = null;

                fund = new AccountFundTransHistory
                {
                    FundTransType = AccountFundTransType.OverBudgetRefund,
                };


                fund.AccountFundType = fundTypeOb;
                var multiplier = 1;
                multiplier = fund.AccountFundType.Multiplier;
                fund.Amount = fundDto.Amount.Value * multiplier;

                fund.VATAmount = fundDto.VatAmount.HasValue ? fundDto.VatAmount.Value * multiplier : 0;
                fund.OriginalAmount = fundDto.OriginalAmount.HasValue ? fundDto.OriginalAmount.Value * multiplier : fund.Amount;

                fund.CreatedById = currentAccount.PrimaryUser.ID;
                fund.AccountId = fundDto.AccountId.Value;
                fund.CreationDate = DateTime.UtcNow;
                fund.TransactionId = fundDto.TransactionId;

                fund.Comment = fundDto.Comment;
                fund.TransactionDate = fundDto.TransactionDate.Value;
                fund.NoqoushReceiptNumber = GetReceiptNumber();
                fund.ObjectRelatedId = fundDto.ObjectRelatedId;
                fund.FundTransStatus = _accountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Committed);
                if (fundDto.CurrencyId.HasValue)
                {
                    fund.Currency = _currencyRepository.Get(fundDto.CurrencyId.Value);
                }
                else
                {
                    fund.Currency = Domain.Model.Core.Currency.USD;
                }

                int attachmentId;
                if (int.TryParse(fundDto.AttachmentId, out attachmentId))
                {
                    fund.Attachment = _documentRepository.Get(attachmentId);
                    fund.Attachment.UpdateUsage();
                }


                var firstFund = _fundTransRepository.Query(M => M.FundTransType.ID == AccountFundTransType.OverBudgetRefund.ID  && M.AccountId == AccountId && M.CreationDate >= DateTime.UtcNow.AddMonths(-1)).FirstOrDefault();
                account.AddFund(fund.Amount);
                _accountRepository.Save(account);
                if (firstFund == null)
                {
                    _fundTransRepository.Save(fund);
                }
                else
                {
                    firstFund.Amount = firstFund.Amount + fund.Amount;
                    //multiplier = 1;
                    //multiplier = firstFund.AccountFundType.Multiplier;
                    //firstFund.Amount = firstFund.Amount * multiplier;


                    firstFund.OriginalAmount = firstFund.Amount;


                    _fundTransRepository.Save(firstFund);
                    fund = firstFund;
                }





                // account.PublishAccountAmountForKafka();

                #region EventBroker

                Dictionary<string, object> extraParameter = new Dictionary<string, object>();
                extraParameter.Add("Id", fund.ID);
                extraParameter.Add("NotifyUser", false);
                //extraParameter.Add("ObjectRelatedId", fundDto.ObjectRelatedId.Value);
                //extraParameter.Add("CampaignName", Campaign.Name);
                EventBrokerContext.Current.ExtraParameters.Add(extraParameter);
                #endregion
            }



        }
        private void SendEmail(string resourcekey, Dictionary<string, string> values)
        {
            string emaildbnody = Framework.Resources.ResourceManager.Instance.GetResource(resourcekey, "EventBroker_Emails");

            foreach (string key in values.Keys)
            {

                string temp = "" + key + "";
                emaildbnody = emaildbnody.Replace(temp, values[key]);



            }

            _mailSender.SendEmail("", "", values["To"], values["To"], values["Subject"], emaildbnody, true, Domain.Configuration.EmailAdmin);
        }

        public void SendCampaignbillingInfoacknowledgment(int CampaignId, string FieldToChange, decimal RequestedAmount, decimal CommittedAmount, DateTime ModOn)
        {

            var Campaign = _CampaignRepository.Get(CampaignId);

            if (Campaign != null && (Campaign.ModifiedOn == ModOn))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();

                values.Add("@ObjectType", Framework.Resources.ResourceManager.Instance.GetResource("Campaign"));
                values.Add("@Object_TypeName", Campaign.Name);
                values.Add("@FieldToChange", FieldToChange);
                values.Add("@RequestedAmount", RequestedAmount.ToString("F2"));
                values.Add("@CommittedAmount", CommittedAmount.ToString("F2"));
                //values.Add("To", Campaign.Account.PrimaryUser.EmailAddress);
                values.Add("To", Domain.Configuration.EmailAdmin);
                values.Add("Subject", Framework.Resources.ResourceManager.Instance.GetResource("CampaignbillingInfoacknowledgmentSubject", "Mail") + "-Account:" + Campaign.Account.Name);

                //values.Add("@CampaignName", campobj.Name);
                values.Add("@AccountName", Campaign.Account.Name);

                Campaign.Budget = CommittedAmount;
                _CampaignRepository.Save(Campaign);
                SendEmail("KafkaEventAck", values);
            }
        }


        public void SendAdGroupbillingInfoacknowledgment(int AdGroupId, string FieldToChange, decimal? RequestedAmount, decimal? CommittedAmount, DateTime ModOn)
        {

            var AdGroup = _adGroupRepository.Get(AdGroupId);
            var campobj= _CampaignRepository.Get(AdGroup.Campaign.ID);
            if (AdGroup != null && (AdGroup.ModifiedOn == ModOn))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();

                values.Add("@ObjectType", Framework.Resources.ResourceManager.Instance.GetResource("AdGroup"));
                values.Add("@Object_TypeName", AdGroup.Name);
                values.Add("@FieldToChange", FieldToChange);
                if (RequestedAmount.HasValue)
                    values.Add("@RequestedAmount", RequestedAmount.Value.ToString("F2"));
                else
                    values.Add("@RequestedAmount", Framework.Resources.ResourceManager.Instance.GetResource("NoValue", "Global"));
                if (CommittedAmount.HasValue)
                    values.Add("@CommittedAmount", CommittedAmount.Value.ToString("F2"));
                else
                    values.Add("@CommittedAmount", Framework.Resources.ResourceManager.Instance.GetResource("NoValue", "Global"));
                //values.Add("To", AdGroup.Campaign.Account.PrimaryUser.EmailAddress);

                values.Add("To", Domain.Configuration.EmailAdmin);
                var subject = Framework.Resources.ResourceManager.Instance.GetResource("AdGroupbillinInfoacknowledgmentSubject", "Mail") + "-Campaign:"+ campobj.Name+"-Account:"+ campobj.Account.Name;
                values.Add("Subject", subject);
                values.Add("@CampaignName", campobj.Name);
                values.Add("@AccountName", campobj.Account.Name);

                AdGroup.Budget = CommittedAmount;
                _adGroupRepository.Save(AdGroup);
                SendEmail("KafkaEventAck", values);
            }
        }


        public void testPublickEventKafka()
        {

            AdGroup fff = new AdGroup();
            fff.testPublickEventKafka();
        }

        #region Private Regions

        private AccountPaymentDetails GetPaymentAccount(NewFundDto fundDto, PayemntAccountType accountType, Domain.Model.Account.Account account)
        {
            AccountPaymentDetails accountPaymentDetails = null;

            if (fundDto.AccountPaymentDetailId.HasValue)
                accountPaymentDetails = _accountPaymentDetailsRepository.Get(fundDto.AccountPaymentDetailId.Value);
            else
            {
                //try to create new payment details
                switch (accountType)
                {
                    case PayemntAccountType.Bank:
                        {
                            accountPaymentDetails = MapperHelper.Map<BankAccountPaymentDetails>(fundDto.PaymentDetail);
                            accountPaymentDetails.AccountType = PayemntAccountType.Bank;
                            accountPaymentDetails.SubType = PayemntAccountSubType.Payment;
                            var item = account.GetBankAccount(accountPaymentDetails as BankAccountPaymentDetails);
                            if (item != null)
                                accountPaymentDetails = item;
                            break;
                        }
                    case PayemntAccountType.PayPal:
                        {
                            accountPaymentDetails = MapperHelper.Map<PayPalAccountPaymentDetails>(fundDto.PaymentDetail);
                            accountPaymentDetails.AccountType = PayemntAccountType.PayPal;
                            accountPaymentDetails.SubType = PayemntAccountSubType.Payment;
                            var item = account.GetPayPalAccount(accountPaymentDetails as PayPalAccountPaymentDetails);
                            if (item != null)
                                accountPaymentDetails = item;
                            break;
                        }
                }
                if ((accountPaymentDetails != null) && (accountPaymentDetails.GetValidateErrors().Count == 0))
                {
                    /*bankPaymentResponse =*/
                    account.AddAccountPaymentDetailSystem(accountPaymentDetails);
                }
                /* if ((bankPaymentResponse == AddPaymentResponse.NoChanges))
                 {
                     throw new NoChangesException();
                 }*/
            }
            return accountPaymentDetails;
        }

        #endregion
    }
}
