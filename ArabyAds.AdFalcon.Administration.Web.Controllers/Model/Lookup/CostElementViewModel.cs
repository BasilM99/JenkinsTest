using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class CostElementViewModel : LookupViewModel
    {
        private CostElementDto costElementDto;
        public override LookupDto LookupDto
        {
            get { return costElementDto; }
            set { costElementDto = (CostElementDto)value; }
        }
    }

    public class CostElementSaveModel : LookupSaveModel
    {
        public CostElementDto LookupDto { get; set; }
    }


    public class FeeViewModel : LookupViewModel
    {
        private FeeDto costElementDto;
        public override LookupDto LookupDto
        {
            get { return costElementDto; }
            set { costElementDto = (FeeDto)value; }
        }
    }

    public class FeeSaveModel : LookupSaveModel
    {
        public FeeDto LookupDto { get; set; }
    }
}
