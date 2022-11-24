using Noqoush.AdFalcon.Domain.Common.Model.Account.DPP;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Account.DPP
{

    //[DataContract(Name = "ImpressionLogType")]
    //public enum ImpressionLogType
    //{
    //    [EnumMember]

    //    None = 0,
    //    [EnumMember]

    //    Impression = 1,
    //    [EnumMember]

    //    AdMarkup = 2,
    
    //}
    public class ImpressionLog : IEntity<int>
    {
        public virtual int ID { set; get; }
        public virtual DPPartner Provider { set; get; }

        public virtual ImpressionLogType Type { set; get; }
        
        public virtual DateTime CreationDate
        {
            get;
            set;
        }

        public virtual DateTime LastUpdate
        {
            get;
            set;
        }

        public virtual string Path { set; get; }
        public virtual int Day { set; get; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool Written { get; set; }

        public virtual string GetDescription()
        {
            return Provider != null ? Provider.Name : "";
        }
    }
}
