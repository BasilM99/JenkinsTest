using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder
{

    [ProtoContract]
    public class ResultDataSetQBDto 
    {




       [ProtoMember(1)]
        public long Count { get; set; }


       [ProtoMember(2)]
        public IList<string> Columns { get; set; }

       [ProtoMember(3)]
        public IList<ValueMessageWrapper<IDictionary<string,string>>> Rows { get; set; }


        [ProtoMember(4)]
        public IList<int> minWidthColumns { get; set; }
    }
}
