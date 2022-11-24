
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;


namespace Noqoush.AdFalcon.Domain.Common.Model.Account.PMP
{

    [DataContract(Name = "DealType")]
    public enum DealType
    {
        [EnumMember]

        Undefined = 0,
        [EnumMember]
        [EnumText("PrivateAuction", "PMPDeal")]
        PrivateAuction = 1,

        [EnumMember]
        [EnumText("Fixed", "PMPDeal")]
        Fixed = 2
    }


    [DataContract(Name = "AdGroupTargetingDealType ")]
    public enum AdGroupTargetingDealType
    {
        [EnumMember]

        Undefined = 0,

        [EnumMember]
        [EnumText("PMP", "PMPDeal")]
        PMP = 1,
        [EnumMember]
        [EnumText("OpenInventorytargeting", "PMPDeal")]
        OpenInventorytargeting = 2


    }



}
