using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdGroupTrackingEvent : AdGroupEvent
    {
       
     
  


    }

    public class AudienceSegmentEventMap : IEntity<int>
    {
        public virtual  bool IsDeleted { get; set ; }

        public virtual int ID { get; set; }
        public virtual AdGroupEvent Event { get; set; }
        public virtual AudienceSegment AudienceSegment { get; set; }
        public virtual string GetDescription()
        {
            return Event.GetDescription() + "-" + AudienceSegment.Name.GetValue();
        }
    }

    public class AdGroupEvent : IEntity<int>
    {

        public virtual string Code { get; set; }

        public virtual AdGroup AdGroup { get; set; }

        public virtual string Description { get; set; }

        public virtual string PreRequisites { get; set; }

        public virtual string StatisticsColumnName { get; set; }

        public virtual List<int> PreRequisitesList
        {
            get
            {
                if (!string.IsNullOrEmpty(PreRequisites))
                {
                    return PreRequisites.Split(',').Select(p => int.Parse(p)).ToList();
                }

                return new List<int>();
            }
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
                        result = result + IoC.Instance.Resolve<IAdGroupConversionEventRepository>().Get(id).Code + ",";


                    }

                }
            }

            return result;
        }
        public virtual bool AllPreRequisitesRequired { get; set; }
        public virtual bool IsTracking { get; set; }
        public virtual bool IsBillable { get; set; }

        public virtual bool AllowDuplicate { get; set; }
        public virtual decimal Revenue { get; set; }

        public virtual bool IsConversion { get; set; }
        public virtual bool IsCustom { get; set; }
        public virtual bool IsPrimary { get; set; }
        public virtual bool IsDeleted
        {
            get;
            set;
        }

        public virtual int ID
        {
            get;
            set;
        }
        public virtual int ValidFor { get; set; }
        public virtual IList<PixelEventMap> PixelListsMap { get; set; }
        public virtual string GetDescription()
        {
            return this.Description;
        }

        public virtual IList<AudienceSegmentEventMap> AudienceSegmentListsMap { get; set; }
    }

    public class AdGroupConversionEvent : AdGroupEvent
    {
      

      


    }

    public class PixelEventMap : IEntity<int>
    {
        public virtual bool IsDeleted { get; set; }

        public virtual int ID { get; set; }
        public virtual AdGroupEvent Event { get; set; }
        public virtual Pixel Pixel { get; set; }
        public virtual string GetDescription()
        {
            return Event.GetDescription() + "-" + Pixel.GetDescription();
        }
    }




}
