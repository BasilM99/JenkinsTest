using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class CostElementViewModel : LookupViewModel
    {
        private CostElementDto costElementDto;
        public override LookupDto LookupDto
        {
            get { return costElementDto; }
            set { costElementDto = (CostElementDto)value; }
        }

        public bool IsDataCostElem { get; set; }
        public bool IsThirdPartyCostElem { get; set; }
        public bool IsPlatformCostElem { get; set; }
        public bool IsAVRCostElem { get; set; }
        public bool IsExchangeDiscrepancyCostElem { get; set; }
        public bool IsAdfalconRevenueCostElem { get; set; }
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

        public bool IsDataFee { get; set; }
        public bool IsThirdPartyFee { get; set; }
        public bool IsPlatformFee { get; set; }
        public bool IsAVRFee { get; set; }
        public bool IsExchangeDiscrepancyFee { get; set; }
        public bool IsAdfalconRevenueFee { get; set; }
    }

    public class FeeSaveModel : LookupSaveModel
    {
        public FeeDto LookupDto { get; set; }
    }
}
