using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    public class VideoTypeDto : LookupDto
    {
        [DataMember]
        public string Code { get; set; }
    }
}
