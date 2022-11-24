using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class BasePagingCriteriaDto : BaseCriteriaDto
    {
    


        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int ItemsPerPage { get; set; }

        [DataMember]
        public string OrderColumn { get; set; }

        [DataMember]
        public string OrderType { get; set; }

        [DataMember]
        public string Culture
        {
            get { return System.Threading.Thread.CurrentThread.CurrentCulture.Name; }
            set { ; }
        }
    }
}
