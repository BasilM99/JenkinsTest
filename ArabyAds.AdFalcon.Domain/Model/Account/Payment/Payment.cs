
using System;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Domain.Model.Account.Payment
{
    public class Payment : IEntity<int>
    {
        private const string _format = "{0}";
        public virtual bool IsDeleted { get; set; }
        public virtual Account Account{get;set;}
        public virtual decimal Amount { get; set; }
        public virtual decimal VATAmount { get; set; }
        public virtual decimal OriginalAmount { get; set; }
        public virtual DateTime? ForMonth { get; set; }
        public virtual string TransactionId { get; set; }
        public virtual string AdFalconReceiptNo { get; set; }
        public virtual string Comment { get; set; }
        public virtual Document Attachment { get; set; }

        public virtual DateTime TransactionDate{get;set;}
        public virtual DateTime CreationDate { get; set; }

        public virtual PaymentType Type{get;set;}
        public virtual User User{get;set;}
        public virtual int ID { get; protected set; }
        public virtual Currency Currency { get; set; }
        public virtual Decimal ExchangeRate { get; set; }
        
        public virtual string GetDescription()
        {
            return string.Format(_format, Amount.ToString("F2"));
        }
    }
}

