using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Core
{
    [ProtoContract]
    public class GetKPIRequest
    {
        [ProtoMember(1)]
        public DateTime FromDate { get; set; }

        [ProtoMember(2)]
        public DateTime ToDate { get; set; }

        [ProtoMember(3)]
        public int[] Ids { get; set; }

        [ProtoMember(4)]
        public KPIScope KPIScope { get; set; }

        [ProtoMember(5)]
        public IList<string> DataBaseFields { get; set; }

        [ProtoMember(6)]
        public int? AccountId { get; set; }

        public int? KpiConfigId { get; set; }
    }

    public class GetBulkKPIRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int[] Filters { get; set; }

        public KPIScope KPIScope { get; set; }
        public int? KpiConfigId { get; set; }
        public int[] Ids { get; set; }


        public IList<string> DataBaseFields { get; set; }


        public int? AccountId { get; set; }
    }
    }

   
