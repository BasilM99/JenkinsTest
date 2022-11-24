﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class CountryDto : LookupDto
    {
       [ProtoMember(1)]
        public virtual string Code { get; set; }
    }
}
