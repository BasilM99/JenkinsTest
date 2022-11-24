using System;
using System.Collections.Generic;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Domain.Model.Account
{
    public class PayPalAccountPaymentDetails : AccountPaymentDetails
    {
        public virtual string UserName { get; set; }
        public virtual bool IsPrimary { get; set; }

        public virtual void ConvertToPrimary()
        {
            this.IsPrimary = true;
        }

        public virtual void ConvertToNormal()
        {
            this.IsPrimary = false;
        }
        
        public override string GetDescription()
        {
            return UserName;
        }
        public override IList<ErrorData> GetValidateErrors()
        {
            var result = new List<ErrorData>();
            if (string.IsNullOrWhiteSpace(UserName))
            {
                result.Add(new ErrorData { ID = "UserNameBR" });
                IsValid = false;
            }
            return result;
        }
        public override void Validate(bool checkEmpty = false)
        {
            var error = new BusinessException();
            IsValid = true;
            IsHasValue = !string.IsNullOrWhiteSpace(UserName);


            if ((IsHasValue) || (checkEmpty))
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    error.Errors.Add(new ErrorData { ID = "UserNameBR" });
                    IsValid = false;
                }
                if (error.Errors.Count > 0)
                    throw error;
            }
            else
            {
                IsValid = false;
            }
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(PayPalAccountPaymentDetails)) return false;
            return Equals((PayPalAccountPaymentDetails)obj);
        }

        public virtual bool Equals(PayPalAccountPaymentDetails other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UserName, UserName);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (!string.IsNullOrEmpty(UserName) ? UserName.GetHashCode() : 0);
                return result;
            }
        }
    }
}

