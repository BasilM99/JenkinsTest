using System;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.Framework;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.AppSite
{
    //[DataContract()]
    //public enum CalculationMode
    //{
    //    [EnumMember]
    //    [EnumText("PercentageType", "Lookup")]
    //    Percentage = 1,
    //    [EnumMember]
    //    [EnumText("FixedType", "Lookup")]
    //    Fixed = 2,
    //}
    public class AppSiteRevenueCalculationSetting : IEntity<int>
    {


        public virtual int ID { get; protected set; }
        public virtual AppSite AppSite { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual CalculationMode CalculationMode { get; set; }
        [PropertyDescriptorValue("CalculationMode")]
        public virtual decimal Value { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
    

        public virtual string GetValueDescription(string value , string calculationMode)
        {

            CalculationMode enumTobe = (CalculationMode)Enum.Parse(typeof(CalculationMode), calculationMode);


            if (enumTobe == CalculationMode.Percentage)
            {
                return (Convert.ToDecimal(value) * 100) + "%";
            }
            else
            {
                return Convert.ToDecimal(value).ToString("F2") + "$";

            }
        }
        public virtual void Deactive()
        {
            this.ToDate = Framework.Utilities.Environment.GetServerTime();
        }
        public virtual string GetDescription()
        {
            return string.Format("{0}:{1}", AppSite.Name, CalculationMode.ToText());
        }
        protected bool Equals(AppSiteRevenueCalculationSetting other)
        {
            return CalculationMode == other.CalculationMode && Value == other.Value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)CalculationMode * 397) ^ Value.GetHashCode();
            }
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AppSiteRevenueCalculationSetting)obj);
        }

    }
}