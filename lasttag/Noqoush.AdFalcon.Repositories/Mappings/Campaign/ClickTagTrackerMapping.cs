using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Performance;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class ClickTagTrackerMapping : ClassMap<ClickTagTracker>
    {
        public ClickTagTrackerMapping()
        {
            Table("adclicktagtrackers");
            Id(p => p.ID).GeneratedBy.Identity();
            Map(p => p.TrackingUrl, "Url");
            Map(p => p.VariableName, "VariableName");
            Map(p => p.IsDeleted);
            References(p => p.Creative, "AdId");
        
        }
    }
}
