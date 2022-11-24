using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    
    [ProtoContract]
    public class KPIConfigDto : LookupDto
    {

        [ProtoMember(1)]

        public override int ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        [ProtoMember(2)]
        public  bool ForAdvertiser { get; set; }

        [ProtoMember(3)]
        public bool ForDeals { get; set; }

        [ProtoMember(4)]
        public bool ForPublisher { get; set; }

        [ProtoMember(5)]
        public bool ForCampaign { get; set; }

        [ProtoMember(13)]
        public bool ForDataProvider { get; set; }

        [ProtoMember(6)]
        public string DataBaseField { get; set; }

        [ProtoMember(7)]
        public bool IsDefault { get; set; }

        [ProtoMember(8)]
        public string GrowIcon { get; set; }

        [ProtoMember(9)]
        public string GroupKey { get; set; }

        [ProtoMember(10)]
        public string Icon { get; set; }

        [ProtoMember(11)]
        public string DisplayFormat  { get; set; }
                        


        [ProtoMember(12)]
        public string TextDesc { get { return this.Name.Value; } set { } }

    }
}
