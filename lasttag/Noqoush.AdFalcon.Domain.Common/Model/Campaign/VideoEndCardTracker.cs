using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
  

    public class VideoEndCardTracker : IEntity<int>
    {
        public virtual int ID { get;  set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string Url { get; set; }

        public virtual VideoEndCard Card { get; set; }
        public virtual VideoEndCardTracker Clone()
        {

            VideoEndCardTracker cloned = new VideoEndCardTracker()
            {
                Card = this.Card,
                Url = this.Url,
                IsDeleted = this.IsDeleted
               
            };

            return cloned;
        }
        public virtual string GetDescription()
        {
            return this.Url;
        }
    }
}
