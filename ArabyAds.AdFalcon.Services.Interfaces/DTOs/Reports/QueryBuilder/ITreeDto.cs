using ArabyAds.AdFalcon.Domain.Common.Model.QueryBuilder;

using System;
using System.Collections.Generic;
using System.Linq;


namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    public interface ITreeQBDto
    {
        string Name { set; get; }
        string text { set; get; }

        string Attribute { set; get; }

        DataTypeQB DataType { set; get; }

         int OrderNumber { get; set; }
         int ParentId { get; set; }
         string DisplayName { set; get; }
        bool @checked { get; set; }

        bool hasChildren { get; set; }

        List<TreeQBDto> children { get; set; }
    }
}