using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    //[DataContract(Name = "AdCreativeUnitTrackerType")]
    //public enum AdCreativeUnitTrackerType
    //{
    //    [EnumMember]
   
    //    Undefined = 0,
    //    [EnumMember]

    //    URL = 1,


    //    [EnumMember]
     
    //    JS = 2,


    //}
    public class AdCreativeUnitTracker : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }

        public virtual AdCreativeUnit CreativeUnit { get; set; }
        public virtual AdGroupTrackingEvent AdGroupEvent { get; set; }
        public virtual string TrackingUrl { get; set; }
        public virtual string TrackingJS { get; set; }
        public virtual AdCreativeUnitTrackerType AdCreativeUnitTrackerType { get; set; }
        public virtual string GetDescription()
        {
            return AdGroupEvent.Description;
        }
        public virtual string Describer(string id)
        {
            string code;
            try
            {
                code = IoC.Instance.Resolve<IAdGroupTrackingEventRepository>().Get(Convert.ToInt32(id)).Description;

            }
            catch (Exception e)
            {

                throw e;
            }


            return code;
        }

    }
}
