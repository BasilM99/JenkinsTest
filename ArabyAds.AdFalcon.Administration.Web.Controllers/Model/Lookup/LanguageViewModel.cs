using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class LanguageViewModel : LookupViewModel
    {
        private LanguageSaveDto LanguageDto;
        public override LookupDto LookupDto
        {
            get { return LanguageDto; }
            set { LanguageDto = (LanguageSaveDto)value; }
        }
    }

    public class LanguageSaveModel : LookupSaveModel
    {
        public LanguageSaveDto LookupDto { get; set; }
    }
}
