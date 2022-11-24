using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [Serializable]
    [DataContract]
    public class PgwDto
    {

        [DataMember]
        public virtual string Code
        {
            get;
            set;
        }

        [DataMember]
        public virtual string ApiResolver
        {
            get;
            set;
        }

        [DataMember]
        public virtual string Realm
        {
            get;
            set;
        }

        [DataMember]
        public virtual string OutletId
        {
            get;
            set;
        }

        [DataMember]
        public virtual string apiref
        {
            get;
            set;
        }  
        [DataMember]
        public virtual string IntegrationPageUrl
        {
            get;
            set;
        }

        [DataMember]
        public virtual string ReturnPageUrl
        {
            get;
            set;
        }
        
        [DataMember]
        public virtual Dictionary<string,string> Settings
        {
            get;
            set;

        }
      
        [DataMember]
        public virtual int ID
        {
            get;
            set;
        }
       
    }

}
