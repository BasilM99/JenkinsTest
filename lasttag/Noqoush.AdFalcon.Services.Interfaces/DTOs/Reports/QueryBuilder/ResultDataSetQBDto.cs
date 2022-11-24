using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder
{

    [DataContract]
    public class ResultDataSetQBDto 
    {




        [DataMember]
        public long Count { get; set; }


        [DataMember]
        public IList<string> Columns { get; set; }

        [DataMember]
        public IList<IDictionary<string,object>> Rows { get; set; }
    }
}
