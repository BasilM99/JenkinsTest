using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Objective
{
    public class AdActionTypeTrackingEvent : IEntity<int>
    {
        public virtual bool IsDeleted { get; set; }

        public virtual int ID { get; protected set; }

        public virtual CostModelWrapper CostModelWrapper { get; set; }

        public virtual CostModelWrapperEnum CostModelWrapperEnum
        {
            get
            {
                if (CostModelWrapper != null)
                    return (CostModelWrapperEnum)this.CostModelWrapper.ID;

                return 0;
            }
        }

        public virtual TrackingEvent Event { get; set; }

        public virtual string StatisticsColumnName { get; set; }

        public virtual string Prerequisites { get; set; }

        public virtual bool AllPreRequisitesRequired { get; set; }

        public virtual bool AllowDuplicate { get; set; }
        public virtual bool IsBillable { get; set; }
        public virtual List<int> GetAllPrerequisitesIds()
        {
            return GetPrerequisiteIds(this);
        }

        public virtual List<string> GetAllPrerequisitesCodes()
        {
            return GetPrerequisiteCodes(this);
        }


        public virtual string GetDescription()
        {
            return Event.GetDescription();
        }

        private static List<int> GetPrerequisiteIds(AdActionTypeTrackingEvent adActionTrackingEvent)
        {
            List<int> Prerequisites = new List<int>();
            if (adActionTrackingEvent != null)
            {
                if (adActionTrackingEvent.Prerequisites != null)
                {
                    var prerequisitesList = adActionTrackingEvent.Prerequisites.Split(',');
                    Prerequisites = prerequisitesList.Select(x => int.Parse(x)).ToList<int>();

                    //   Prerequisites.Add(adActionTrackingEvent.Prerequisite.ID);
                    //  Prerequisites.AddRange(GetPrerequisiteIds(adActionTrackingEvent.Prerequisite));
                }
            }

            return Prerequisites;
        }

        private static List<string> GetPrerequisiteCodes(AdActionTypeTrackingEvent adActionTrackingEvent)
        {
            List<string> Prerequisites = new List<string>();
            if (adActionTrackingEvent != null)
            {
                foreach (int id in GetPrerequisiteIds(adActionTrackingEvent))
                {
                    Prerequisites.Add(IoC.Instance.Resolve<IAdActionTypeTrackingEventRepository>().Get(id).Event.Code);

                }
                if (adActionTrackingEvent.Prerequisites != null)
                {
                    //  Prerequisites.Add(adActionTrackingEvent.Event.Code);
                    // Prerequisites.AddRange(GetPrerequisiteCodes(adActionTrackingEvent.Prerequisite));
                }
            }

            return Prerequisites;
        }

        public virtual string GetPrerequisiteCodesDescription(string PrerequisitesString)
        {
            List<int> Prerequisites = new List<int>();
            string result = string.Empty;
            if (!string.IsNullOrEmpty(PrerequisitesString))
            {
                var prerequisitesList = PrerequisitesString.Split(',');
                Prerequisites = prerequisitesList.Select(x => int.Parse(x)).ToList<int>();
                if (Prerequisites != null)
                {
                    foreach (int id in Prerequisites)
                    {
                        result = result + IoC.Instance.Resolve<IAdActionTypeTrackingEventRepository>().Get(id).Event.Code + ",";


                    }

                }
            }

            return result;
        }

    }
}
