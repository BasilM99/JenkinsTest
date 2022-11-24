using System;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    public class SystemBankAccount : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string BeneficiaryName { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankAddress { get; set; }
        public virtual string RecipientAccountNumber { get; set; }
        public virtual string SWIFT { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime ActiveFrom { get; set; }
        public virtual DateTime? ActiveTo { get; set; }

        public virtual string GetDescription()
        {
            return BankName;
        }
    }
}

