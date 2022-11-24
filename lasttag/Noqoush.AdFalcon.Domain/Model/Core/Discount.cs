using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    //[DataContract()]
    //public enum DiscountType
    //{
    //    [EnumMember]
    //    [EnumText("PercentageType", "Lookup")]
    //    Default =0,
    //    [EnumMember]
    //    [EnumText("PercentageType", "Lookup")]
    //    Percentage = 1,
    //    [EnumMember]
    //    [EnumText("FixedType", "Lookup")]
    //    Fixed = 2,
    //}
    public class Discount: IComplexEntity
    {
        private const string _format = "{0}:{1}";
        public virtual decimal Value { get; set; }
       
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }

        public virtual DiscountType Type { get; set; }
        public virtual string ValueDescriper { get { return GetValueDescription(); } }
        public virtual void DeActive()
        {
            this.ToDate = Framework.Utilities.Environment.GetServerTime();
        }
        public virtual string GetValueDescription()
        {
            if (Type== DiscountType.Percentage || Type == DiscountType.Default)
            {
                return (Value*100).ToString("F2")+"%";
            }
            else
            {
                return Value.ToString("F2") +"$";

            }
        }

        public virtual string GetDescription()
        {
            return string.Format(_format, Type.ToText(), Value.ToString("F2"));
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Discount)) return false;
            return Equals((Discount)obj);
        }

        public bool Equals(Discount other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Value == Value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Value.GetHashCode();
                return result;
            }
        }
    }
}
