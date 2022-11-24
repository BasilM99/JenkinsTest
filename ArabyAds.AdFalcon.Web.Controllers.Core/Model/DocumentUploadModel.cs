using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{
    public class DocumentUploadModel
    {
       public int documentId { get; set; }
        public int parentId { get; set; }
        
        public int typeId { get; set; }
        public int adTypeId { get; set; }
    }
}
