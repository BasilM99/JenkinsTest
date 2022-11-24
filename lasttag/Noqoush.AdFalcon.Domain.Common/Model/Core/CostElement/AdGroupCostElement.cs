using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement
{

    public enum  AdGroupCostElementScope{
        [EnumMember]
        Unknown =0,
        [EnumMember]
        Inventory =1,
        [EnumMember]
        DataProvider =2

}
   /*public class AdGroupCostElement : IEntity<int>
    {
        private DateTime? _DateTo;
        private int _Type;
        private DateTime _DateFrom;
        public virtual DPPartner Provider { get; set; }
        public virtual AdGroupCostElementScope Scope { get; set; }
        public virtual int ID { get; set; }
        public virtual AdGroup AdGroup { get; set; }
        public virtual CostElement CostElement { get; set; }
        public virtual Party Beneficiary { get; set; }

      
        public virtual int Type
        {
            get
            {


                return _Type;


            }
            set
            {

                _Type = 1;
             
            }
        }

        public virtual decimal Value { get; protected set; }
        public virtual DateTime FromDate { get {

              
                return _DateFrom;


            } set {

                _DateFrom = value;
                _DateFrom= new DateTime(_DateFrom.Year, _DateFrom.Month, _DateFrom.Day, _DateFrom.Hour, 0, 0);
            } }
        public virtual DateTime? ToDate { get {

                return _DateTo;

            } set {
                _DateTo = value;
                if(_DateTo.HasValue)
                    _DateTo= new DateTime(_DateTo.Value.Year, _DateTo.Value.Month, _DateTo.Value.Day, _DateTo.Value.Hour, 0, 0);

            } }
        public virtual CostModelWrapper CostModelWrapper { get; set; }
        public virtual CostModelWrapperEnum CostModelWrapperEnum
        {
            get
            {
                if (this.CostModelWrapper != null)
                {
                    return (CostModelWrapperEnum)this.CostModelWrapper.ID;
                }

                return 0;
            }
        }

        public virtual string GetDescription()
        {
            return Value.ToString();
        }
        public virtual AdGroupCostElement Clone()
        {
            return new AdGroupCostElement
                       {
                           CostElement = this.CostElement,
                           Beneficiary = this.Beneficiary,
                           Value = this.Value,
                           FromDate = this.FromDate,
                           ToDate = this.ToDate,

                Scope = this.Scope,
                Provider = this.Provider,
                CostModelWrapper = this.CostModelWrapper
                       };
        }

        public virtual bool IsDeleted { get; set; }

        public virtual decimal GetReadableValue()
        {
            if (this.CostElement.Type == CalculationType.Percentage)
            {
                return this.Value * 100;
            }
            else
            {
                if (!this.CostElement.IsOneTime)
                {
                    if (this.CostModelWrapper != null)
                    {
                        return this.Value * this.CostModelWrapper.Factor;
                    }
                }
            }

            return this.Value;
        }
        public virtual void SetValue(decimal value)
        {
            this.Value = value;

        }
        public virtual void SetCostElementValue(decimal value, CostModelWrapper costModelWrapper)
        {
            if (costModelWrapper == null)
                throw new ArgumentNullException("costModelWrapper");


            if (this.CostElement.Type == CalculationType.Percentage)
            {
                this.Value = value / 100.0M;
            }
            else
            {
                if (!this.CostElement.IsOneTime)
                {
                    Value = value / costModelWrapper.Factor;
                }
                else
                {
                    Value = value;
                }
            }

            this.CostModelWrapper = costModelWrapper;
        }
    }


    public class AdGroupFee : IEntity<int>
    {
        private int _Type;
        private DateTime? _DateTo;
        private DateTime _DateFrom;
        public virtual DPPartner Provider { get; set; }
        public virtual AdGroupCostElementScope Scope { get; set; }
        public virtual int ID { get; set; }
        public virtual AdGroup AdGroup { get; set; }
        public virtual Fee Fee { get; set; }
        public virtual Party Beneficiary { get; set; }

        public virtual bool IsRemoved { get; set; }

        
        public virtual int Type
        {
            get
            {


                return _Type;


            }
            set
            {

                _Type = 2;

            }
        }
        public virtual decimal Value { get; protected set; }
        public virtual DateTime FromDate
        {
            get
            {


                return _DateFrom;


            }
            set
            {

                _DateFrom = value;
                _DateFrom = new DateTime(_DateFrom.Year, _DateFrom.Month, _DateFrom.Day, _DateFrom.Hour, 0, 0);
            }
        }
        public virtual DateTime? ToDate
        {
            get
            {

                return _DateTo;

            }
            set
            {
                _DateTo = value;
                if (_DateTo.HasValue)
                    _DateTo = new DateTime(_DateTo.Value.Year, _DateTo.Value.Month, _DateTo.Value.Day, _DateTo.Value.Hour, 0, 0);

            }
        }
        public virtual CostModelWrapper CostModelWrapper { get; set; }
        public virtual CostModelWrapperEnum CostModelWrapperEnum
        {
            get
            {
                if (this.CostModelWrapper != null)
                {
                    return (CostModelWrapperEnum)this.CostModelWrapper.ID;
                }

                return 0;
            }
        }

        public virtual string GetDescription()
        {
            return Value.ToString();
        }
        public virtual AdGroupFee Clone()
        {
            return new AdGroupFee
            {
                Fee = this.Fee,
                Beneficiary = this.Beneficiary,
                Value = this.Value,
                FromDate = this.FromDate,
                ToDate = this.ToDate,

                Scope = this.Scope,
                Provider = this.Provider,
                CostModelWrapper = this.CostModelWrapper
            };
        }

        public virtual bool IsDeleted { get; set; }

        public virtual decimal GetReadableValue()
        {
            if (this.Fee.Type == CalculationType.Percentage)
            {
                return this.Value * 100;
            }
            else
            {
                //if (!this.Fee.IsOneTime)
                //{
                    if (this.CostModelWrapper != null)
                    {
                        return this.Value * this.CostModelWrapper.Factor;
                    }
                //}
            }

            return this.Value;
        }
        public virtual void SetValue(decimal value)
        {
            this.Value = value;

        }
        public virtual void SetCostElementValue(decimal value, CostModelWrapper costModelWrapper)
        {
            if (costModelWrapper == null)
                throw new ArgumentNullException("costModelWrapper");


            if (this.Fee.Type == CalculationType.Percentage)
            {
                this.Value = value / 100.0M;
            }
            else
            {
                //if (!this.Fee.IsOneTime)
                {
                    Value = value / costModelWrapper.Factor;
                }
                //else
                //{
                //    Value = value;
                //}
            }

            this.CostModelWrapper = costModelWrapper;
        }
    }*/
}
