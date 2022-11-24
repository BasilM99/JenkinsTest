using System;
using System.Collections.Generic;
using System.Linq;


namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    public class DimensionDto : EntityQBDto
    {
        public string Name { set; get; }
        public string Source { set; get; }
        public string Attributes { set; get; }
        public string FilterCol { set; get; }
        public string CustomGet { set; get; }

        public bool IsGrouped { set; get; }

        
        public bool IsSql { set; get; }
        public IList<ColumnQBDto> Columns { set; get; }
        public bool IsEnum { set; get; }

        public string TableName { set; get; }
        public string Selector { set; get; }

        public bool IsScoped { set; get; }
        public string ScopeTableName { set; get; }


    }
}