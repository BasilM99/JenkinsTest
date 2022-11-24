using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.Core
{
    [DataContract]
    public class metriceColumnDto
    {
        [DataMember]

        public int Id { get; set; }
        [DataMember]

        public bool Advertiser { get; set; }
        [DataMember]

        public bool Publisher { get; set; }
        [DataMember]

        public bool DSP { get; set; }
        [DataMember]

        public bool IsSelected { get; set; }

        [DataMember]

        public string Header { get; set; }


        [DataMember]

        public string HeaderResourceKey { get; set; }

        [DataMember]

        public string HeaderResourceSet { get; set; }
        [DataMember]

        public string GroupKey { get; set; }
        [DataMember]

        public string DataBaseFieldName { get; set; }
        [DataMember]

        public bool Hide { get; set; }

        [DataMember]

        public string AppFieldName { get; set; }
        [DataMember]

        public string Format { get; set; }

        [DataMember]

        public int Order { get; set; }
    }
}
