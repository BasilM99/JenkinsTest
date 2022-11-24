using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.UserInfo;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Services
{
   
    public class BidManager : IBidManager
    {
        const int DEFAULT_VALUE = 0;

        private static object _LockObject = new Object();
        private static ICostModelWrapperRepository _CostModelWrapperRepository;
        public static ICostModelWrapperRepository CostModelWrapperRepository
        {
            get
            {
                if (_CostModelWrapperRepository == null)
                {
                    lock (_LockObject)
                    {
                        if (_CostModelWrapperRepository == null)
                        {
                            _CostModelWrapperRepository = IoC.Instance.Resolve<ICostModelWrapperRepository>();
                        }
                    }
                }

                return _CostModelWrapperRepository;
            }
        }

        private static List<CostModelWrapper> _CostModelWrappers;
        public static List<CostModelWrapper> CostModelWrappers
        {
            get
            {
                if (_CostModelWrappers == null)
                {
                    _CostModelWrappers = CostModelWrapperRepository.GetAll().ToList();
                }

                return _CostModelWrappers;
            }
        }


        const BidConfigCalculationMode DEFAULT_CALCULATION_MODE = BidConfigCalculationMode.Maximum;
        private IBidConfigRepository bidConfigRepository = null;
        private ArabyAds.Framework.ConfigurationSetting.IConfigurationManager configurationManager = null;
        private IList<BidConfig> bidConfigList = null;
        static readonly object lockObj = new object();

        public BidManager(IBidConfigRepository bidConfigRepository, ArabyAds.Framework.ConfigurationSetting.IConfigurationManager configurationManager)
        {
            this.bidConfigRepository = bidConfigRepository;
            this.configurationManager = configurationManager;

        }
        private IEnumerable<BidConfig> BidConfigList
        {
            get
            {
                if (bidConfigList == null)
                {
                    lock (lockObj)
                    {
                        if (bidConfigList == null)
                        {
                            bidConfigList = bidConfigRepository.GetAll().ToList();
                        }
                    }
                }

                return bidConfigList;
            }
        }
        public ReturnBid GetBid(BidParameter parameters)
        {
            //add default CostModel bid values
            var bid = new ReturnBid(GetDefaultBidValues());

            //add the bid for the Action Type
            bid.Add(CalculateActionType(parameters));

            ////add the bid for the Adtype
            bid.Add(CalculateAdType(parameters));

            //add the bid for the Device Targeting
            bid.Add(CalculateDeviceTargeting(parameters));

            //add the bid for the Geographic Targeting
            bid.Add(CalculateGeographicTargeting(parameters));

            //add the bid for the Operator Targeting
            bid.Add(CalculateOperatorTargeting(parameters));

            //add the bid for the Keyword Targeting
            bid.Add(CalculateKeywordTargeting(parameters));

            //add the bid for the Demographic Targeting
            bid.Add(CalculateDemographicTargeting(parameters));

            return bid;
        }

        private IDictionary<int, int> GetDefaultBidValues()
        {
            IDictionary<int, int> costModelBidValues = new Dictionary<int, int>();
            var AppScope = ArabyAds.AdFalcon.Domain.Common.Model.Core.AppScope.Network;

            if (OperationContext.Current.UserInfo<IUserInfo>().AccountRole == (int)Common.Model.Account.AccountRole.DSP)
            {
                AppScope = ArabyAds.AdFalcon.Domain.Common.Model.Core.AppScope.DSP;

            }
            foreach (var item in CostModelWrappers)
            {
                if(AppScope == ArabyAds.AdFalcon.Domain.Common.Model.Core.AppScope.DSP)
                costModelBidValues.Add(item.ID, item.DefaultDSPBidValue);
                else
                    costModelBidValues.Add(item.ID, item.DefaultBidValue);
            }

            return costModelBidValues;
        }

        private IDictionary<int, int> CalculateActionType(BidParameter parameters)
        {
            IDictionary<int, int> actionTypesBidValues = GetZeroBidValues();

            foreach (var item in CostModelWrappers)
            {
                int bidValue = actionTypesBidValues[item.ID];
                bidValue += GetBidConfigValue(BidConfigType.ActionType, parameters.ActionType, (CostModelWrapperEnum)item.ID);
                actionTypesBidValues[item.ID] = bidValue;
            }

            return actionTypesBidValues;
        }
        private IDictionary<int, int> CalculateAdType(BidParameter parameters)
        {
            IDictionary<int, int> adTypesBidValues = GetZeroBidValues();

            foreach (var item in CostModelWrappers)
            {
                int bidValue = adTypesBidValues[item.ID];

                if (parameters.AdTypeId.HasValue)
                {
                    bidValue += GetBidConfigValue(BidConfigType.AdType, parameters.AdTypeId.Value, (CostModelWrapperEnum)item.ID);
                }

                adTypesBidValues[item.ID] = bidValue;
            }

            return adTypesBidValues;

        }
        private IDictionary<int, int> CalculateDeviceTargeting(BidParameter parameters)
        {
            // Set Default Values
            IDictionary<int, int> deviceTargetingBidValues = GetZeroBidValues();

            switch (parameters.DeviceTargetingTypeId)
            {
                case DeviceTargetingType.PlatformTypeId:
                    {
                        if (parameters.Platforms != null)
                        {
                            foreach (var item in CostModelWrappers)
                            {
                                int bidValue = deviceTargetingBidValues[item.ID];
                                bidValue += GetBidConfigValue(BidConfigType.Platform, parameters.Platforms, DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                                deviceTargetingBidValues[item.ID] = bidValue;
                            }
                        }
                        break;
                    }
                case DeviceTargetingType.ManufacturerTypeId:
                    {
                        if (parameters.Manufacturers != null)
                        {
                            foreach (var item in CostModelWrappers)
                            {
                                int bidValue = deviceTargetingBidValues[item.ID];
                                bidValue += GetBidConfigValue(BidConfigType.Manufacturer, parameters.Manufacturers, DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                                deviceTargetingBidValues[item.ID] = bidValue;
                            }
                        }
                        break;
                    }
                case DeviceTargetingType.ModelTargetingTypeId:
                case DeviceTargetingType.ActionTypeTargetingTypeId:
                    {
                        //get the paltform targeting bid
                        IDictionary<int, int> platformBidValues = new Dictionary<int, int>();
                        foreach (var item in CostModelWrappers)
                        {
                            int bidValue = GetBidConfigValue(BidConfigType.Platform, parameters.Platforms, DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                            platformBidValues.Add(item.ID, bidValue);
                        }

                        //get the manufacturer bid value
                        IDictionary<int, int> manufacturerBidValues = new Dictionary<int, int>();
                        foreach (var item in CostModelWrappers)
                        {
                            int bidValue = GetBidConfigValue(BidConfigType.Manufacturer, parameters.Manufacturers, DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                            manufacturerBidValues.Add(item.ID, bidValue);
                        }

                        deviceTargetingBidValues = GetBidConfigValue(platformBidValues, manufacturerBidValues, DEFAULT_CALCULATION_MODE);
                        break;
                    }
                case DeviceTargetingType.DeviceCapabilityTypeId:
                    {
                        foreach (var item in CostModelWrappers)
                        {
                            int bidValue = deviceTargetingBidValues[item.ID];
                            bidValue += GetBidConfigValue(BidConfigType.DeviceCapability, parameters.DeviceCapabilities, DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                            deviceTargetingBidValues[item.ID] = bidValue;
                        }

                        break;
                    }
            }
            return deviceTargetingBidValues;
        }
        private IDictionary<int, int> CalculateGeographicTargeting(BidParameter parameters)
        {
            IDictionary<int, int> geoGraphicBidValues = GetZeroBidValues();
            var geographicTargetings = parameters.Geographies;
            if (geographicTargetings != null)
            {
                foreach (var item in CostModelWrappers)
                {
                    int bidValue = geoGraphicBidValues[item.ID];
                    bidValue += GetBidConfigValue(BidConfigType.Geographic, geographicTargetings, DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                    geoGraphicBidValues[item.ID] = bidValue;
                }
            }
            return geoGraphicBidValues;
        }
        private IDictionary<int, int> CalculateOperatorTargeting(BidParameter parameters)
        {
            IDictionary<int, int> operaterTargetingBid = GetZeroBidValues();
            var operaterTargetings = parameters.Operators;
            if (operaterTargetings != null)
            {
                foreach (var item in CostModelWrappers)
                {
                    int bidValue = operaterTargetingBid[item.ID];
                    bidValue += GetBidConfigValue(BidConfigType.Operator, operaterTargetings,DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                    operaterTargetingBid[item.ID] = bidValue;
                }
            }
            return operaterTargetingBid;
        }
        private IDictionary<int, int> CalculateKeywordTargeting(BidParameter parameters)
        {
            IDictionary<int, int> keywordTargetingBid = GetZeroBidValues();
            var keywordTargetings = parameters.Keywords;
            if (keywordTargetings != null)
            {
                foreach (var item in CostModelWrappers)
                {
                    int bidValue = keywordTargetingBid[item.ID];
                    bidValue += GetBidConfigValue(BidConfigType.Keyword, keywordTargetings, DEFAULT_CALCULATION_MODE, (CostModelWrapperEnum)item.ID);
                    keywordTargetingBid[item.ID] = bidValue;
                }
            }
            return keywordTargetingBid;
        }
        private IDictionary<int, int> CalculateDemographicTargeting(BidParameter parameters)
        {
            IDictionary<int, int> demographicTargetingBid = GetZeroBidValues();
            if (parameters.Demographic.HasValue)
            {
                foreach (var item in CostModelWrappers)
                {
                    int bidValue = demographicTargetingBid[item.ID];
                    bidValue += GetBidConfigValue(BidConfigType.Demographic, parameters.Demographic.Value, (CostModelWrapperEnum)item.ID);
                    demographicTargetingBid[item.ID] = bidValue;
                }

            }
            return demographicTargetingBid;
        }

        private int GetBidConfigValue(BidConfigType type, IList<int> targetingIds, BidConfigCalculationMode calculationMode, CostModelWrapperEnum costModelWrapper)
        {
            //var AppScope = ArabyAds.AdFalcon.Domain.Model.Core.AppScope.Network;

            //if (OperationContext.Current.UserInfo<IUserInfo>().AccountRole == (int)Model.Account.AccountRole.DSP)
            //{
            //    AppScope = ArabyAds.AdFalcon.Domain.Model.Core.AppScope.DSP;

            //}
            var returnValue = DEFAULT_VALUE;
            var values = targetingIds.Select(targetingId => GetBidConfigValue(type, targetingId, costModelWrapper)).ToList();
            if (values.Count > 0)
            {
                switch (calculationMode)
                {
                    case BidConfigCalculationMode.Minimum:
                        {
                            returnValue = values.Min();
                            break;
                        }
                    case BidConfigCalculationMode.Maximum:
                        {
                            returnValue = values.Max();
                            break;
                        }
                    case BidConfigCalculationMode.Avarage:
                        {
                            //TODO:Osaleh to check this function
                            returnValue = (short)values.Average(item => item);
                            break;
                        }
                }
            }
            return returnValue;
        }
        private int GetBidConfigValue(BidConfigType type, int targetingId, CostModelWrapperEnum costModelWrapper)
        {
            var AppScope = ArabyAds.AdFalcon.Domain.Common.Model.Core.AppScope.Network;
          
            if (OperationContext.Current.UserInfo<IUserInfo>().AccountRole == (int)Common.Model.Account.AccountRole.DSP)
            {
                AppScope = ArabyAds.AdFalcon.Domain.Common.Model.Core.AppScope.DSP;

            }
            var config = BidConfigList.FirstOrDefault(item => item.Type == type && item.TargetingId == targetingId && item.CostModelWrapperEnum == costModelWrapper&& item.AppScope== AppScope);
            if (config != null)
            {
                return config.Value;
            }
            else
            {
                //get the default Config for this type
                config = bidConfigList.FirstOrDefault(item => item.Type == type && item.TargetingId == -1 && item.CostModelWrapperEnum == costModelWrapper && item.AppScope == AppScope);
                if (config != null)
                {
                    return config.Value;
                }

            }
            return DEFAULT_VALUE;
        }
        private IDictionary<int, int> GetBidConfigValue(IDictionary<int, int> firstBidValues, IDictionary<int, int> secondBidValues, BidConfigCalculationMode calculationMode)
        {
            IDictionary<int, int> costModelWrapperBidValues = new Dictionary<int, int>();

            if(firstBidValues == null || secondBidValues == null)
                throw new ArgumentNullException();

            if(firstBidValues.Count != CostModelWrappers.Count || secondBidValues.Count != CostModelWrappers.Count)
                throw new ArgumentException();

            foreach (var item in CostModelWrappers)
            {
                var firstValue = firstBidValues.Where(p => p.Key == item.ID).SingleOrDefault();
                var secondValue = secondBidValues.Where(p => p.Key == item.ID).SingleOrDefault();

                var bidValue = GetValue(firstValue.Value,secondValue.Value,calculationMode);

                costModelWrapperBidValues.Add(item.ID, bidValue);
            }

            return costModelWrapperBidValues;
        }

        private int GetValue(int bidValue1, int bidValue2, BidConfigCalculationMode calculationMode)
        {
            var returnValue = DEFAULT_VALUE;
            var values = new List<int> { bidValue1, bidValue2 };
            switch (calculationMode)
            {
                case BidConfigCalculationMode.Minimum:
                    {
                        returnValue = values.Min();
                        break;
                    }
                case BidConfigCalculationMode.Maximum:
                    {
                        returnValue = values.Max();
                        break;
                    }
                case BidConfigCalculationMode.Avarage:
                    {
                        //TODO:Osaleh to check this function
                        returnValue = (short)values.Average(item => item);
                        break;
                    }
            }
            return returnValue;
        }

        private IDictionary<int, int> GetZeroBidValues()
        {
            IDictionary<int, int> costModelBidValues = new Dictionary<int, int>();

            foreach (var item in CostModelWrappers)
            {
                int bidValue = GetDefualt(item);
                costModelBidValues.Add(item.ID, bidValue);
            }

            return costModelBidValues;
        }

        private int GetDefualt(CostModelWrapper item)
        {
            return 0;
        }

    }
}
