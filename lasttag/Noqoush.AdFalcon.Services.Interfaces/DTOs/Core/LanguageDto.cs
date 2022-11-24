using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class LanguageDto : LookupDto
    {

        [DataMember]
       
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
        public virtual string Code { get; set; }
        [DataMember]
        public virtual bool ForPortal
        {
            get;
            set;
        }
    }
    [DataContract]
    public class LanguageSaveDto : LookupDto
    {
        [Required]
        [StringLength(5)]
        [DataMember]
        public virtual string Code { get; set; }
        [DataMember]
        public virtual bool ForPortal
        {
            get;
            set;
        }

    }

}
