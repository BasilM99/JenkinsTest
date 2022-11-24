using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using System.Collections.Specialized;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [Serializable]
    [ProtoContract]
    public class PgwDto
    {

       [ProtoMember(1)]
        public virtual string Code
        {
            get;
            set;
        }

       [ProtoMember(2)]
        public virtual string ApiResolver
        {
            get;
            set;
        }

       [ProtoMember(3)]
        public virtual string Realm
        {
            get;
            set;
        }

       [ProtoMember(4)]
        public virtual string OutletId
        {
            get;
            set;
        }

       [ProtoMember(5)]
        public virtual string apiref
        {
            get;
            set;
        }  
       [ProtoMember(6)]
        public virtual string IntegrationPageUrl
        {
            get;
            set;
        }

       [ProtoMember(7)]
        public virtual string ReturnPageUrl
        {
            get;
            set;
        }

        [ProtoMember(8)]
        public virtual Dictionary<string, string> Settings
        {
            get;
            set;

        } = new Dictionary<string, string>();
      
       [ProtoMember(9)]
        public virtual int ID
        {
            get;
            set;
        }
       
    }

}
