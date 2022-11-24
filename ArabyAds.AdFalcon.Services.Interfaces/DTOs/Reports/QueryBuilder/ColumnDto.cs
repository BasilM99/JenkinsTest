
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB
{
    [ProtoContract]
    public class ColumnQBDto : TreeQBDto
    {
        [ProtoMember(1)]
        public string homeIdSelector { set; get; }
        [ProtoMember(2)]
        public bool IsSql { set; get; }
        [ProtoMember(3)]
        public string Source { set; get; }
        [ProtoMember(4)]

        public bool IsDuplicated { set; get; }
        [ProtoMember(5)]
        public string TableName { set; get; }
        [ProtoMember(6)]

        public string FkSelector { set; get; }
        [ProtoMember(7)]

        public string formatSQL { set; get; }

    }
    [ProtoContract]
    public class DataQBDto
    {
        [ProtoMember(1)]
        public long TotalCount { set; get; }

        [ProtoMember(2)]
        public int Id { set; get; }
        private string _name;
        [ProtoMember(3)]
        public string Name
        {
            set
            {
                _name = value;
            }
            get
            {
                if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ar")
                {
                    return Name_Ar ?? _name;
                }
                return Name_En ?? _name;
            }
        }
        [ProtoMember(4)]
        public string ParentName { set; get; }
        [ProtoMember(5)]
        public string SuperParentName { set; get; }

        [ProtoMember(6)]
        public string Name_Ar { set; get; }
        [ProtoMember(7)]
        public string Name_En { set; get; }
    }
}