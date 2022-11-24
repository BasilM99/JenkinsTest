using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{

    public class ObjectTypeDto
    {
        public int ID { get; set; }
        public int RootID { get; set; }
        public string ObjectTypeName { get; set; }
    }
    [DataContract]

    public class TrialDto
    {
        [DataMember]
        public string ObjectName { get; set; }
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public DateTime ActionTime { get; set; }
        [DataMember]
        public string ActionTimeString { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int ObjectActionID { get; set; }
        [DataMember]
        public int RootId { get; set; }

        [DataMember]
        public IList<TrialDto>  Childs { get; set; }
        [DataMember]
        public int ObjectRootTypeId { get; set; }
        [DataMember]
        public int ObjectId { get; set; }
        [DataMember]
        public int ObjectTypeId { get; set; }
        [DataMember]
        public string Details { get; set; }


        [DataMember]
        public string ObjectActionString { get; set; }

        [DataMember]
        public string ObjectActionConstantString { get; set; }
       
        [DataMember]
        public string PropertyName { get; set; }
        [DataMember]
        public string NewValue { get; set; }
        [DataMember]
        public string OldValue { get; set; }


    }
}
