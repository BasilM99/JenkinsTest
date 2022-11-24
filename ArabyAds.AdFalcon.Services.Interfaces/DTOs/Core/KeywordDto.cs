using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class KeywordDto : LookupDto
    {
       [ProtoMember(1)]
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
       [ProtoMember(2)]
        public int Usage { get; set; }

       [ProtoMember(3)]
        public string Code { get; set; }

       [ProtoMember(4)]
        public int Rank { get; set; }

    }
      [ProtoContract]
    public class KeywordSaveDto : LookupDto
    {
        [Required]
        [StringLength(6)]
       [ProtoMember(1)]
        public virtual string Code { get; set; }
    }
}
