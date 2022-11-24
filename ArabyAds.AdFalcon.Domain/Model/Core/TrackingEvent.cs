using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class TrackingEvent : ManagedLookupBase
    {
        private static ICostModelWrapperRepository _costModelWrapperRepository = IoC.Instance.Resolve<ICostModelWrapperRepository>();
        public virtual string EventName { get; set; }
        public virtual string Code { get; set; }
        public virtual int ValidFor { get; set; }
        public virtual int? DefaultFrequencyCapping { get; set; }

        private CostModelWrapper _costModelWrapper;
        public virtual CostModelWrapper CostModelWrapper { get {

                return _costModelWrapper ??= _costModelWrapperRepository.Query(M => M.Event.ID == ID).FirstOrDefault();

            } }


        public virtual bool IsConversion { get; set; }
        //public virtual string GetDescription()
        //{
        //    return this.Name.Value;
        //}
    }
}
