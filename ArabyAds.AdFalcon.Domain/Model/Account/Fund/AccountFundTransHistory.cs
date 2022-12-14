using System;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using ArabyAds.AdFalcon.Domain.Repositories;

namespace ArabyAds.AdFalcon.Domain.Model.Account
{
    [Serializable]
    public partial class AccountFundTransHistory : IEntity<int>
    {

        private static IUserRepository _userRepository = null;
        private static IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = Framework.IoC.Instance.Resolve<IUserRepository>();
                }
                return _userRepository;
            }
        }
        private const string _format = "{0}:{1}";
        public AccountFundTransHistory()
        {
        }
        public virtual AccountFundTransStatus FundTransStatus { get; set; }
        public virtual int AccountId { get; set; }
        public virtual string NoqoushReceiptNumber { get; set; }
        public virtual string TransactionId { get; set; }
        public virtual decimal Amount { get; set; }

        public virtual decimal VATAmount { get; set; }
        public virtual decimal OriginalAmount { get; set; }
        public virtual AccountFundType AccountFundType { get; set; }
        public virtual int CreatedById { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual int ID { get; set; }
        public virtual string Payee { get; set; }
        public virtual DateTime TransactionDate { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual int? ObjectRelatedId { get; set; }
        public virtual AccountFundTransType FundTransType { get; set; }

        public virtual string Comment { get; set; }
        public virtual Document Attachment { get; set; }

        public virtual string GetCreatedbyIdDescription(string createdbyId)
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(createdbyId))
            {
               var userId= Convert.ToInt32(createdbyId);
                if(userId>0)
                name=UserRepository.Get(userId).GetName(); ;

            }
            return name;
            
        }

        public virtual string GetDescription()
        {
            return string.Format(_format,  FundTransType.GetDescription(), Amount.ToString("F2"));
        }
    }
}