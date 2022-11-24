using System;
using System.Collections.Generic;
using System.Linq;


namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    public class FactDto: EntityQBDto
    {
        public string DisplayName { set; get; }
        public string Name { set; get; }
        public bool IsForWeb { set; get; }

        public string WebDisplayName { set; get; }
        public ICollection<DimensionDto> Dimensions { set; get; }
        public ICollection<MeasureDto> Measures { set; get; }


    }
}