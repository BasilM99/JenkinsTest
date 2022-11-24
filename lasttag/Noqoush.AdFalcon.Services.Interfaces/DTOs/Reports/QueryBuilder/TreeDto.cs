using Noqoush.AdFalcon.Domain.Common.Model.QueryBuilder;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
 
    public class TreeQBDto : EntityQBDto, ITreeQBDto
    {
    
        public string id { set; get; }
    
        public string Name { set; get; }

     
        public string data { set; get; }
           
        public string text { set; get; }
      
        public string Attribute { set; get; }
        public string SubstituteAttribute { set; get; }

        public string RawAttribute { set; get; }


        public int OrderNumber { get; set; }
      
        public int ParentId { get; set; }
      
        public string DisplayName { set; get; }
      

        public DataTypeQB DataType { set; get; }

     
        public bool @checked { get; set; }
       

        public bool hasChildren { get; set; }
    

        public List<TreeQBDto> children { get; set; }

       
        public int CustomValue { get; set; }
      
        public string CustomValue1 { get; set; }
     
        public string CustomValue2 { get; set; }
        

        public string Key { get; set; }

      
        public List<TreeQBDto> Childs { get; set; }
      

        public string state { get; set; }
     

        public string style { get; set; }


        public string requestsmapping { get; set; }


        public string dealsrequestsmapping { get; set; }
    }
}