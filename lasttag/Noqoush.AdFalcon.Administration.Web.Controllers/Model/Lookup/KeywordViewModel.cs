using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class KeywordViewModel : LookupViewModel
    {
        private KeywordSaveDto keywordDto;
        public override LookupDto LookupDto
        {
            get { return keywordDto; }
            set { keywordDto = (KeywordSaveDto)value; }
        }
    }

    public class KeywordSaveModel : LookupSaveModel
    {
        public KeywordSaveDto LookupDto { get; set; }
    }
}
