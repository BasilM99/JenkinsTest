using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class ThemeDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public LocalizedStringDto Name { get; set; }
        [DataMember]
        [Required]
        public string TextColor { get; set; }
        [DataMember]
        [Required]
        public string BackgroundColor { get; set; }
        [DataMember]
        [Required]
        public bool IsCustom { get; set; }
    }
}
