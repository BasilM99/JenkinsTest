using System;
using System.Collections.Generic;
using System.Text;
//using Gallio.Framework;
//using MbUnit.Framework;
//using MbUnit.Framework.ContractVerifiers;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [Serializable]
    [ProtoContract]
    public class LanguageDto : LookupDto
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
        public virtual string Code { get; set; }
       [ProtoMember(3)]
        public virtual bool ForPortal
        {
            get;
            set;
        }
    }
    [ProtoContract]
    public class LanguageSaveDto : LookupDto
    {
        [Required]
        [StringLength(5)]
       [ProtoMember(1)]
        public virtual string Code { get; set; }
       [ProtoMember(2)]
        public virtual bool ForPortal
        {
            get;
            set;
        }

    }

}
