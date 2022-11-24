using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class ThemeDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }
       [ProtoMember(2)]
        public LocalizedStringDto Name { get; set; }
       [ProtoMember(3)]
        [Required]
        public string TextColor { get; set; }
       [ProtoMember(4)]
        [Required]
        public string BackgroundColor { get; set; }
       [ProtoMember(5)]
        [Required]
        public bool IsCustom { get; set; }
    }
}
