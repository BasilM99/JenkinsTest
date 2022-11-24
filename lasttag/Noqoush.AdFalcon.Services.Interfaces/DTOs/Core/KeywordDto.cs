using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class KeywordDto : LookupDto
    {
        [DataMember]
        [Required(ResourceName = "Targeting_InvalidKeywordId" , ResourceSet = "Error")]
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
        [DataMember]
        public int Usage { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public int Rank { get; set; }

    }
      [DataContract]
    public class KeywordSaveDto : LookupDto
    {
        [Required]
        [StringLength(6)]
        [DataMember]
        public virtual string Code { get; set; }
    }
}
