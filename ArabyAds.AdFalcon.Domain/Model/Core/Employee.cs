using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class Employee : Party
    {
        public Employee()
        {
        }
        public virtual Model.Account.Account Account { get; set; }
        public virtual JobPosition JobPosition { get; set; }
        public override string GetDescription()
        {
            return Name;
        }
        public override void Validate()
        {
            base.Validate();
            if (IsValid)
            {
                IsValid = false;
                //create business Exception to hold error data list 
                var error = new BusinessException();
                //validate Name
                if (this.JobPosition == null)
                {
                    error.Errors.Add(new ErrorData { ID = "JobPositionBR" });
                }
                if (error.Errors.Count > 0)
                {
                    IsValid = false;
                    throw (error);
                }
                IsValid = DataAnnotationsValidator.TryValidate(this);
            }
        }
    }
}
