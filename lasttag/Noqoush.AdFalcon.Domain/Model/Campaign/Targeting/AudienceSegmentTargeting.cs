using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.EventDTOs;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Targeting;
namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class OperationResult
    {
        public List<AudienceSegment> UsedSegments { get; set; } = new List<AudienceSegment>();
        public List<AudienceSegment> BilledSegments { get; set; } = new List<AudienceSegment>();
        public decimal TotalPrice { get; set; }
        public void CalculateTotalPrice()
        {
            TotalPrice = 0;
            if (BilledSegments!=null)
            {
                for (int i = 0; i < BilledSegments.Count; i++)
                {
                    TotalPrice += BilledSegments[i].Price;
                }
            }
        }

        public static OperationResult MergeAnd(OperationResult left, OperationResult right)
        {
            var operationResult = new OperationResult();
            for (int i = 0; i < left.BilledSegments.Count; i++)
            {
                var lseg = left.BilledSegments[i];
                var rseg = right.BilledSegments.Find((x) => x.Provider.ID == lseg.Provider.ID);

                if (rseg != null)
                {
                    right.BilledSegments.Remove(rseg);
                    operationResult.BilledSegments.Add(MaxPrice(lseg, rseg));
                }
                else
                {
                    operationResult.BilledSegments.Add(lseg);
                }
            }
            // remaining segments in right side
            operationResult.BilledSegments.AddRange(right.BilledSegments);

            // add all left used segments
            operationResult.UsedSegments.AddRange(left.UsedSegments);

            if (right.UsedSegments!=null)
            {
                // add only right segments not exists in left segments
                for (int i = 0; i < right.UsedSegments.Count; i++)
                {
                    var rseg = right.UsedSegments[i];
                    var lseg = left.UsedSegments.Find((x) => x.Code == rseg.Code);
                    if (lseg == null)
                    {
                        operationResult.UsedSegments.Add(rseg);
                    }
                }
            }
            operationResult.CalculateTotalPrice();

            return operationResult;
        }
        public static OperationResult MergeOr(OperationResult result1, OperationResult result2)
        {
            return result1.TotalPrice <= result2.TotalPrice ? result1 : result2;
        }

        public static OperationResult MergeOrMax(OperationResult result1, OperationResult result2)
        {
            return result1.TotalPrice >= result2.TotalPrice ? result1 : result2;
        }
        private static AudienceSegment MaxPrice(AudienceSegment seg1, AudienceSegment seg2)
        {
            return seg1.Price >= seg2.Price ? seg1 : seg2;
        }
    }
    [Serializable]
    [DataContract()]
    [Flags]
    public enum AudienceSegmentTargetingCategroyFlags

    {
        [EnumMember]
        Undefined = 0,

        [EnumMember]
        
        FirstPart = 1,
        [EnumMember]

        ThirdParty  = 2,
        [EnumMember]

        ExternalParty = 4,
 
    

    }
   
       


        public class AudienceSegmentTargeting : TargetingBase
    {
        public static IAudienceSegmentRepository AudienceSegmentRepository = IoC.Instance.Resolve<IAudienceSegmentRepository>();
        public static ICampaignRepository CampaignRepository = IoC.Instance.Resolve<ICampaignRepository>();
        public static IAccountCostElementRepository AccountCostElementRepository = IoC.Instance.Resolve<IAccountCostElementRepository>();
        private string codeForFirstParty = "fpaud";
        public AudienceSegmentTargeting()
        {

        }
        public AudienceSegmentTargeting(string RulesJson)
        {
            this.RulesJson = RulesJson;
        }
        public virtual int Category { get; set; }
        public virtual bool LogAdMarkup { get; set; }
        public virtual bool IsExternal { get; set; }
        public virtual DPPartner DataProvider { get; set; }
        public virtual AudienceSegment AudienceSegment { get; set; }
        public virtual string RulesJson { get; set; }
        public override string GetDescription()
        {
            return "Query View";
        }
        public virtual string GetComputed(string RulesJson)
        {

            var groupJson = GetRulesJsonForGroup(RulesJson);
            return string.Format("{0}", computed(groupJson));

        }
        public override TargetingBase Copy()
        {
            var cloneObj = new AudienceSegmentTargeting()
            {
                AudienceSegment = this.AudienceSegment,
                RulesJson = this.RulesJson,
                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted,
                DataBid=this.DataBid,
                MaxDataBid=this.MaxDataBid,

                IsExternal = this.IsExternal,
                DataProvider = this.DataProvider
                ,

                LogAdMarkup =this.LogAdMarkup,
            };
            return cloneObj;
        }


        public virtual string GetRulesJson()
        {
            try
            {



                group obj = JsonConvert.DeserializeObject<group>(RulesJson);

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        rulegroup.Name = AudiencePath(tempInt); ;
                                    }
                                }
                            }


                        }
                    }

                }

                string json = JsonConvert.SerializeObject(obj);
                return json.ToString();

            }
            catch (Exception EX)
            {
                return "";

            }


        }
        public virtual bool CheckIfDataProviderCostElementAdded(AdGroup groupObj, string GroupStrRule)
        {
            try
            {



                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);
                IList<int> dpProviders = new List<int>();
                IDictionary<int, int> dpProviderAccounts = new Dictionary<int, int>();

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var dp = audienceSegment.Provider;
                                        dpProviders.Add(dp.Account.ID);
                                        if (!dpProviderAccounts.ContainsKey(dp.Account.ID))
                                            dpProviderAccounts.Add(dp.Account.ID, dp.ID);
                                     
                                        //var dpPartnerCostElement= AccountCostElementRepository.GetAccountCostElements(dp.Account.ID);


                                    }
                                }
                            }


                        }
                    }

                }
                dpProviders = dpProviders.Distinct().ToList();
                bool anyAdded = false;
                bool eachAdded = false;
                bool feeAdded = false;
                foreach (var dpProv in dpProviders)
                {

                    eachAdded = groupObj.SetAccountCostElmentsSaved(dpProv, dpProviderAccounts[dpProv]);

                    eachAdded = groupObj.SetAccountCostElmentsSavedForDataProv(Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value, dpProviderAccounts[dpProv]);
                    //feeAdded = groupObj.SetAccountFeesSaved(dpProv, dpProviderAccounts[dpProv]);

                    if (eachAdded)
                    {
                        anyAdded = true;
                    }
                }
                return anyAdded;

            }
            catch (Exception EX)
            {
                return false;

            }


        }


        public virtual bool CheckIfDataProviderImpressionAllowed(AdGroup groupObj, string GroupStrRule)
        {
            try
            {



                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);
                IList<int> dpProviders = new List<int>();
                IDictionary<int, int> dpProviderAccounts = new Dictionary<int, int>();

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var dp = audienceSegment.Provider;

                                        if (!dp.AllowImpressionTrackers)
                                        {
                                            return false;
                                        }


                                    }
                                }
                            }


                        }
                    }

                }
           
                return true;

            }
            catch (Exception ex)
            {
                return true;

            }


        }

        public virtual bool CheckIfDataProviderUsingFirstParty(AdGroup groupObj, string GroupStrRule)
        {
            try
            {



                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);
                IList<int> dpProviders = new List<int>();
                IDictionary<int, int> dpProviderAccounts = new Dictionary<int, int>();

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var dp = audienceSegment.Provider;

                                        if (dp.Code== codeForFirstParty)
                                        {
                                            return true;
                                        }


                                    }
                                }
                            }


                        }
                    }

                }

                return false;

            }
            catch (Exception ex)
            {
                return false;

            }


        }

        public virtual bool CheckIfDataProviderFeeAdded(AdGroup groupObj, string GroupStrRule)
        {
            try
            {



                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);
                IList<int> dpProviders = new List<int>();
                IDictionary<int, int> dpProviderAccounts = new Dictionary<int, int>();

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var dp = audienceSegment.Provider;
                                        dpProviders.Add(dp.Account.ID);
                                        if (!dpProviderAccounts.ContainsKey(dp.Account.ID))
                                            dpProviderAccounts.Add(dp.Account.ID, dp.ID);

                                        //var dpPartnerCostElement= AccountCostElementRepository.GetAccountCostElements(dp.Account.ID);


                                    }
                                }
                            }


                        }
                    }

                }
                dpProviders = dpProviders.Distinct().ToList();
                bool anyAdded = false;
                bool eachAdded = false;
                bool feeAdded = false;
                foreach (var dpProv in dpProviders)
                {

                   // eachAdded = groupObj.SetAccountCostElmentsSaved(dpProv, dpProviderAccounts[dpProv]);
                    feeAdded = groupObj.SetAccountFeesSaved(dpProv, dpProviderAccounts[dpProv]);

                    if (feeAdded)
                    {
                        anyAdded = true;
                    }
                }
                return anyAdded;

            }
            catch (Exception EX)
            {
                return false;

            }


        }


       
        public virtual List<int> GetAudienceSegmentIds()
        {
            List<int> audSegmIds = new List<int>();
            string RulesJson = this.RulesJson;
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            RuleVersion rversion = JsonConvert.DeserializeObject<RuleVersion>(RulesJson, new RuleConverterConverter()); ;
            var childss = getGroupForBinaryConditions(rversion.Version_1, null);
            group obj = new group();
            obj.Operator = "AND";
            if (rversion.Version_1 != null && rversion.Version_1 is CompoundRule)
            {
                if ((rversion.Version_1 as CompoundRule).Operator == OperatorRule.Or)
                {
                    obj.Operator = "OR";
                }
            }
            obj.rules = childss.ToArray();
            foreach (var item in obj.rules)
            {

                if (item != null)
                {
                    var groupItem = item.group;
                    if (groupItem != null)
                    {

                        //if (groupItem.rules.Length>1)
                        //{
                        //    rversion.Version_1 = new CompoundRule();
                        //}

                        foreach (var rulegroup in groupItem.rules)
                        {
                            if (rulegroup != null)
                            {
                                if (!string.IsNullOrEmpty(rulegroup.id))
                                {
                                    int tempInt = Convert.ToInt32(rulegroup.id);
                                    if (tempInt > 0 &&!audSegmIds.Contains(tempInt))
                                    {
                                        //var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        //rulegroup.Name = AudiencePath(tempInt);
                                        //rulegroup.Price = audienceSegment.Price.ToString();

                                        audSegmIds.Add(tempInt);
                                    }
                                }
                            }
                        }


                    }
                }

            }


           
            return audSegmIds;

        }


        public virtual string  getAudienceIntegrationList(string GroupStrRule)
        {
            try
            {

                List<int> Integrationids = new List<int>();

                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var id = audienceSegment.OperatorSegmentCode.Split(new char[] { ':' });
                                        Integrationids.Add(Convert.ToInt32(id[1]));




                                    }
                                }
                            }


                        }
                    }

                }

                return string.Join(",", Integrationids) ;

            }
            catch (Exception EX)
            {
                return string.Empty;

            }


        }
        public virtual string getAudienceIntegrationListActive(string GroupStrRule)
        {
            try
            {

                List<int> Integrationids = new List<int>();

                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var id = audienceSegment.OperatorSegmentCode.Split(new char[] { ':' });
                                       
                                        if(audienceSegment.Activated)
                                        Integrationids.Add(Convert.ToInt32(id[1]));




                                    }
                                }
                            }


                        }
                    }

                }

                return string.Join(",", Integrationids);

            }
            catch (Exception EX)
            {
                return string.Empty;

            }


        }
        public virtual int getCountAudienceIntegrationList(string GroupStrRule)
        {
            try
            {

                List<int> Integrationids = new List<int>();

                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var id = audienceSegment.OperatorSegmentCode.Split(new char[] { ':' });
                                        Integrationids.Add(Convert.ToInt32(id[1]));




                                    }
                                }
                            }


                        }
                    }

                }

                return Integrationids.Count;

            }
            catch (Exception EX)
            {
                return 0;

            }


        }
        public virtual bool CheckIfRulesHaveAdvertiserBlocker(int CampAdvertiserId, string GroupStrRule)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"] != null && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"]))
                {

                    codeForFirstParty = System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"];

                }
                if (this.Category > 0)
                {
                    this.Category =0;

                }
            
                AudienceSegmentTargetingCategroyFlags catEnum = AudienceSegmentTargetingCategroyFlags.FirstPart;
                AudienceSegmentTargetingCategroyFlags catEnumExternalParty = AudienceSegmentTargetingCategroyFlags.ExternalParty;
                AudienceSegmentTargetingCategroyFlags catEnumThirdParty = AudienceSegmentTargetingCategroyFlags.ThirdParty;
                group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);

                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var dp = audienceSegment.Provider;
                                        AudienceSegmentTargetingCategroyFlags catValue = (AudienceSegmentTargetingCategroyFlags)this.Category;
                                        if (dp.Code == codeForFirstParty && !catValue.HasFlag(AudienceSegmentTargetingCategroyFlags.FirstPart))
                                        {
                                            this.Category = Convert.ToInt16(catEnum | catValue);
                                        }
                                        else if (dp.IsExternalProvider && !catValue.HasFlag(AudienceSegmentTargetingCategroyFlags.ExternalParty))
                                        {
                                            this.Category = Convert.ToInt16(catEnumExternalParty | catValue);
                                        }
                                        else if(!catValue.HasFlag(AudienceSegmentTargetingCategroyFlags.ThirdParty))
                                        {
                                            this.Category = Convert.ToInt16(catEnumThirdParty | catValue);

                                        }
                                        var advBlocks = dp.AdvertiserBlockList;
                                        if (advBlocks != null && advBlocks.Count > 0 && CampAdvertiserId > 0)
                                        {
                                            var advRes = advBlocks.Where(M => M.Advertiser.ID == CampAdvertiserId).SingleOrDefault();

                                            if (advRes != null)
                                                return true;
                                        }
                                        if (dp.AdMarkupLogRequired)
                                        { this.LogAdMarkup = true; }

                                    }
                                }
                            }


                        }
                    }

                }

                return false;

            }
            catch (Exception EX)
            {
                return false;

            }


        }

        public virtual IList<string> GetDomainURLForAdvertiserBlocker(int CampAdvertiserId, string GroupStrRule,int AccountId)
        {

            if (System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"] != null && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"]))
            {

                codeForFirstParty = System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"];

            }

            group obj = JsonConvert.DeserializeObject<group>(GroupStrRule);
            IList<string> resuts = new List<string>();
                foreach (var item in obj.rules)
                {

                    if (item != null)
                    {
                        var groupItem = item.group;
                        if (groupItem != null)
                        {
                            foreach (var rulegroup in groupItem.rules)
                            {
                                if (rulegroup != null)
                                {
                                    if (!string.IsNullOrEmpty(rulegroup.id))
                                    {
                                        int tempInt = Convert.ToInt32(rulegroup.id);
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        var dp = audienceSegment.Provider;

                                        var advBlocks = dp.AdvertiserBlockList;
                                    var accountWhiteList = dp.AccountWhiteList;
                                    if (accountWhiteList != null && accountWhiteList.Count > 0)
                                    {
                                        var accountList = accountWhiteList.Where(M => M.Account.ID == AccountId).SingleOrDefault();
                                        if ((accountList == null))
                                        {
                                            var error = new BusinessException();
                                            error.Errors.Add(new ErrorData { ID = "audienceSegmentTargetingNotValid" });
                                            throw error;
                                        }
                                        
                                    }
                                    else if(dp.Code!=codeForFirstParty)
                                    {
                                        var error = new BusinessException();
                                        error.Errors.Add(new ErrorData { ID = "audienceSegmentTargetingNotValid" });
                                        throw error;

                                    }
                                    if (advBlocks != null && advBlocks.Count > 0 && CampAdvertiserId > 0)
                                        {
                                            var advRes = advBlocks.Where(M => M.Advertiser.ID == CampAdvertiserId).SingleOrDefault();

                                        if (advRes != null)
                                        {       if(resuts.Where(M=>M==advRes.Advertiser.DomainURL).SingleOrDefault()==null)
                                            resuts.Add(advRes.Advertiser.DomainURL);

                                        }

                                        }


                                   // if (!string.IsNullOrWhiteSpace(dp.BlockedDomains))
                                   // {
                                        
                                       

                                        var arrString = dp.DomainBlockList;
                                        if (arrString!=null)
                                        {
                                            foreach (var ur in arrString)
                                            {

                                                if (resuts.Where(M => M.Trim().ToLower() == ur.Domain.Trim().ToLower()).SingleOrDefault() == null)
                                                    resuts.Add(ur.Domain.Trim());

                                            }
                                        }

                                    //}


                                }
                                }
                            }


                        }
                    }

                }

            return resuts;




        }

        public virtual Rule getBinaryCondtionsForRules(child[] rules, OperatorRule op)
        {
            var rule = new Rule { };


            if (rules != null && rules.Length > 1)
            {
                rule = new CompoundRule();
                (rule as CompoundRule).Operator = op;
                if (rules[0].group == null)
                {
                    int? recency=null ;
                   var SegById= AudienceSegmentRepository.Get(Convert.ToInt32(rules[0].id));
                    if (!string.IsNullOrEmpty(rules[0].recency) /*&& SegById.Provider.Code == codeForFirstParty*/ )
                        recency = Convert.ToInt32(rules[0].recency);
                    (rule as CompoundRule).LRule = new Rule { frequency = 0, Segment = SegById.Code,recency= recency, Not = rules[0].condition == "Exclude" };

                }
                else
                    (rule as CompoundRule).LRule = getBinaryCondtionsForGroup(rules[0].group);




                child[] b = new child[rules.Length - 1];
                Array.Copy(rules, 1, b, 0, rules.Length - 1);

                (rule as CompoundRule).RRule = getBinaryCondtionsForRules(b, op);
                return (rule as CompoundRule);


            }
            if (rules[0].group == null)
            {
                int? recency2 = null;
                var SegById2 = AudienceSegmentRepository.Get(Convert.ToInt32(rules[0].id));
                if (!string.IsNullOrEmpty(rules[0].recency) /*&& SegById2.Provider.Code== codeForFirstParty*/)
                    recency2 = Convert.ToInt32(rules[0].recency);
                rule = new Rule { frequency =0, Segment = SegById2.Code, recency = recency2, Not = rules[0].condition == "Exclude" };
              
                
                
            }
            else
                rule = getBinaryCondtionsForGroup(rules[0].group);

            return rule;
        }
        public virtual Rule getBinaryCondtionsForGroup(group group)
        {


            OperatorRule op = OperatorRule.Or;
            if (group.Operator == "AND")
            {
                op = OperatorRule.And;

            }
            var rule = getBinaryCondtionsForRules(group.rules, op);
            rule.Group = true;
            return rule;

        }
        public virtual List<child> getGroupForBinaryConditions(Rule rule, List<child> groupChild)
        {
            OperatorRule op = OperatorRule.And;
            child ch = new child();
            List<child> childsAll = new List<child>();
            if (groupChild == null)
                groupChild = new List<child>();

            List<child> groupchildsAll = new List<child>();
            child groupch = new child();

            if ((rule is CompoundRule))
            {
                var childs1 = new List<child>();
                op = (rule as CompoundRule).Operator;
                if ((rule as CompoundRule).LRule.Group)
                    childs1 = getGroupForBinaryConditions((rule as CompoundRule).LRule, null);
                else
                    childs1 = getGroupForBinaryConditions((rule as CompoundRule).LRule, groupChild);



                //var childs1 = getGroupForBinaryConditions((rule as CompoundRule).LRule, groupChild);
                var childs2 = new List<child>();
                if ((rule as CompoundRule).RRule.Group)
                    childs2 = getGroupForBinaryConditions((rule as CompoundRule).RRule, null);
                else
                    childs2 = getGroupForBinaryConditions((rule as CompoundRule).RRule, groupChild);
                groupchildsAll.AddRange(childs1);
                groupchildsAll.AddRange(childs2);
            }
            else

            {
                var segm=AudienceSegmentRepository.GetSegmentByCode(rule.Segment);
                ch.id = AudienceSegmentRepository.GetSegmentIdByCode(rule.Segment).ToString();
                if (rule.recency >= 0)
                {
                    ch.recency = rule.recency.ToString();

                    if (segm.Provider.Code == codeForFirstParty)
                    {
                        ch.showrecency = true;

                    }

                }
                ch.condition = "Target";
                if (rule.Not)
                {

                    ch.condition = "Exclude";
                }
                groupChild.Add(ch);

            }
            if (rule.Group)
            {

                groupch.group = new group();
                groupch.group.Operator = "AND";

                if (op == OperatorRule.Or)
                {
                    groupch.group.Operator = "OR";
                }
                groupch.group.rules = groupChild.ToArray();
                if (groupChild.Count > 0)
                    groupchildsAll.Add(groupch);
            }

            return groupchildsAll;




        }
        public virtual RuleVersion GetRulesForExpression(int lastVersion = 0, bool UseOrg = false)
        {
            RuleVersion rversion = new RuleVersion();
            rversion.Version = 1 + lastVersion;
            //rversion.Version_1 = new Rule();
            group obj = null;
            if (UseOrg)
            {
                string OrgRulesJson = GetRulesJsonForGroup(RulesJson);
                obj = JsonConvert.DeserializeObject<group>(OrgRulesJson);
            }
            else
            {
                obj = JsonConvert.DeserializeObject<group>(RulesJson);

            }
            OperatorRule op = OperatorRule.Or;
            if (obj.Operator == "AND")
            {
                op = OperatorRule.And;

            }
            rversion.Version_1 = getBinaryCondtionsForRules(obj.rules, op);

            rversion.Version_1.Group = true;

            return rversion;

        }

        public virtual string GetRulesJsonForExpression(string groupJson, int lastVersion = 0)
        {
            RuleVersion rversion = new RuleVersion();
            rversion.Version = 1 + lastVersion;
            //rversion.Version_1 = new Rule();
            group obj = JsonConvert.DeserializeObject<group>(groupJson);

            OperatorRule op = OperatorRule.Or;
            if (obj.Operator == "AND")
            {
                op = OperatorRule.And;

            }
            rversion.Version_1 = getBinaryCondtionsForRules(obj.rules, op);

            rversion.Version_1.Group = true;
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            string json = JsonConvert.SerializeObject(rversion);
            return json.ToString();

        }
        public virtual string GetRulesJsonForGroup(string RulesJson)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            RuleVersion rversion = JsonConvert.DeserializeObject<RuleVersion>(RulesJson, new RuleConverterConverter()); ;
            var childss = getGroupForBinaryConditions(rversion.Version_1, null);
            group obj = new group();
            obj.Operator = "AND";
            if (rversion.Version_1 != null && rversion.Version_1 is CompoundRule)
            {
                if ((rversion.Version_1 as CompoundRule).Operator == OperatorRule.Or)
                {
                    obj.Operator = "OR";
                }
            }
            obj.rules = childss.ToArray();
            foreach (var item in obj.rules)
            {

                if (item != null)
                {
                    var groupItem = item.group;
                    if (groupItem != null)
                    {

                        //if (groupItem.rules.Length>1)
                        //{
                        //    rversion.Version_1 = new CompoundRule();
                        //}

                        foreach (var rulegroup in groupItem.rules)
                        {
                            if (rulegroup != null)
                            {
                                if (!string.IsNullOrEmpty(rulegroup.id))
                                {
                                    int tempInt = Convert.ToInt32(rulegroup.id);
                                    if (tempInt > 0)
                                    {
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        rulegroup.Name = AudiencePath(tempInt);
                                        rulegroup.Price = audienceSegment.Price.ToString();
                                    }
                                }
                            }
                        }


                    }
                }

            }


            string json = JsonConvert.SerializeObject(obj);
            return json.ToString();

        }
        public virtual string GetRulesJsonForGroup()
        {
            string RulesJson = this.RulesJson;
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            RuleVersion rversion = JsonConvert.DeserializeObject<RuleVersion>(RulesJson, new RuleConverterConverter()); ;
            var childss = getGroupForBinaryConditions(rversion.Version_1, null);
            group obj = new group();
            obj.Operator = "AND";
            if (rversion.Version_1 != null && rversion.Version_1 is CompoundRule)
            {
                if ((rversion.Version_1 as CompoundRule).Operator == OperatorRule.Or)
                {
                    obj.Operator = "OR";
                }
            }
            obj.rules = childss.ToArray();
            foreach (var item in obj.rules)
            {

                if (item != null)
                {
                    var groupItem = item.group;
                    if (groupItem != null)
                    {

                        //if (groupItem.rules.Length>1)
                        //{
                        //    rversion.Version_1 = new CompoundRule();
                        //}

                        foreach (var rulegroup in groupItem.rules)
                        {
                            if (rulegroup != null)
                            {
                                if (!string.IsNullOrEmpty(rulegroup.id))
                                {
                                    int tempInt = Convert.ToInt32(rulegroup.id);
                                    if (tempInt > 0)
                                    {
                                        var audienceSegment = AudienceSegmentRepository.Get(tempInt);
                                        rulegroup.Name = AudiencePath(tempInt);
                                        rulegroup.Price = audienceSegment.Price.ToString();
                                    }
                                }
                            }
                        }


                    }
                }

            }


            string json = JsonConvert.SerializeObject(obj);
            return json.ToString();

        }
        public virtual int GetLastVersionForRuleJson(string RulesJson)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            RuleVersion rversion = JsonConvert.DeserializeObject<RuleVersion>(RulesJson, new RuleConverterConverter());


            return rversion.Version;
        }

        public virtual string AudiencePath(int id)
        {
            var audianceSeq = AudienceSegmentRepository.Get(id);
            string AudiencePathStr = string.Empty;
            if (audianceSeq.Parent != null)
            {
                AudiencePathStr = AudiencePath(audianceSeq.Parent.ID) + ">";



            }
            AudiencePathStr = AudiencePathStr + (audianceSeq.Name!=null&& audianceSeq.Name.Value!=null ? audianceSeq.Name.Value:string.Empty);
            return AudiencePathStr;
        }
        public static string computed(string json)
        {
            string item = "";
            if (!string.IsNullOrEmpty(json))
            {

                group results = JsonConvert.DeserializeObject<group>(json);
                if (results != null)
                {
                    item += "(";
                    foreach (child child in results.rules)
                    {
                        item += "(";
                        child last = child.group.rules.Last();

                        foreach (child Subchild in child.group.rules)
                        {
                            item += Subchild.Name + " " + Subchild.condition + " " + Subchild.Price;
                            if (!Subchild.Equals(last))
                            {
                                item += " " + child.group.Operator + " ";
                            }

                        }
                        item += ")";
                        last = results.rules.Last();

                        if (!child.Equals(last))
                        {
                            item += " " + results.Operator + " ";
                        }

                    }
                }
                item += ")";
            }
            return item;
        }

        public virtual bool CalculateBillInternalMin(Rule node, Stack<OperationResult> operationResults)
        {
            bool match = false;

            if (!(node is CompoundRule))
            {
                match = true;

                var userSegmentDefinition = AudienceSegmentRepository.GetSegmentByCode(node.Segment);

                //if (node.Not)
                //{
                //    match = !match &&true;
                //}

                if (match)
                {
                    var opResult = new OperationResult() { UsedSegments = { userSegmentDefinition }, BilledSegments = { userSegmentDefinition } };
                    opResult.CalculateTotalPrice();
                    operationResults.Push(opResult);
                }
            }
            else
            {
                bool leftMatch = CalculateBillInternalMin((node as CompoundRule).LRule, operationResults);
                bool rightMatch = CalculateBillInternalMin((node as CompoundRule).RRule, operationResults);

                var rightOpResult = operationResults.Pop();
                var leftOpResult = operationResults.Pop();

                if ((node as CompoundRule).Operator == OperatorRule.And)
                {
                    match = leftMatch && rightMatch;
                    if (match)
                    {
                        operationResults.Push(OperationResult.MergeAnd(leftOpResult, rightOpResult));
                    }
                }
                else
                {
                    match = leftMatch || rightMatch;
                    if (leftMatch && rightMatch)
                    {
                        operationResults.Push(OperationResult.MergeOr(leftOpResult, rightOpResult));
                    }
                    else if (leftMatch)
                    {
                        operationResults.Push(leftOpResult);
                    }
                    else if (rightMatch)
                    {
                        operationResults.Push(rightOpResult);
                    }
                }
            }

            if (!match)
            {
                operationResults.Push(null);
            }
            return match;
        }

        public virtual bool CalculateBillInternalMax(Rule node, Stack<OperationResult> operationResults)
        {
            bool match = false;

            if (!(node is CompoundRule))
            {
                match = true;

                var userSegmentDefinition = AudienceSegmentRepository.GetSegmentByCode(node.Segment);

                //if (node.Not)
                //{
                //    match = !match && true;
                //}

                if (match)
                {
                    var opResult = new OperationResult() { UsedSegments = { userSegmentDefinition }, BilledSegments = { userSegmentDefinition } };
                    opResult.CalculateTotalPrice();
                    operationResults.Push(opResult);
                }
            }
            else
            {
                bool leftMatch = CalculateBillInternalMax((node as CompoundRule).LRule, operationResults);
                bool rightMatch = CalculateBillInternalMax((node as CompoundRule).RRule, operationResults);

                var rightOpResult = operationResults.Pop();
                var leftOpResult = operationResults.Pop();

                if ((node as CompoundRule).Operator == OperatorRule.And)
                {
                    match = leftMatch && rightMatch;
                    if (match)
                    {
                        operationResults.Push(OperationResult.MergeAnd(leftOpResult, rightOpResult));
                    }
                }
                else
                {
                    match = leftMatch || rightMatch;
                    if (leftMatch && rightMatch)
                    {
                        operationResults.Push(OperationResult.MergeOrMax(leftOpResult, rightOpResult));
                    }
                    else if (leftMatch)
                    {
                        operationResults.Push(leftOpResult);
                    }
                    else if (rightMatch)
                    {
                        operationResults.Push(rightOpResult);
                    }
                }
            }

            if (!match)
            {
                operationResults.Push(null);
            }
            return match;
        }

        public virtual AudicanceBillSummary CalculateBillInternal(Rule node)
        {
            AudicanceBillSummary sum = new AudicanceBillSummary();

            var operationResults = new Stack<OperationResult>();

            CalculateBillInternalMin(node, operationResults);

            var finalResult = operationResults.Pop();

            if (finalResult == null)
            {
                return null;
            }
            sum.MinValue = finalResult.TotalPrice;


            var operationResultsMax = new Stack<OperationResult>();

            CalculateBillInternalMax(node, operationResultsMax);

            var finalResultMax = operationResultsMax.Pop();

            if (finalResultMax == null)
            {
                return null;
            }
            sum.MaxValue = finalResultMax.TotalPrice;


            return sum;


        }

        public virtual decimal? DataBid { get; set; }
        public virtual decimal? MaxDataBid { get; set; }

        public virtual void PublishkafkaforCheck(List<int> Ids,int campId , string Action)
        {

            //foreach (var Id in Ids)
            //{
                Noqoush.AdFalcon.Domain.Configuration.KafkaEventPublisher.Publish(new AudienceListCheck { Ids = Ids, CampaignId= campId, Action= Action });
            
            
            //}
        }
    }
 

    public class child
    {
        public string ParentId { get; set; }
        public string id { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
        public string condition { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public group group { get; set; }
        [JsonProperty("$$hashKey")]
        public string hashKey { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore )]
        public string recency { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string frequency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool showrecency { get; set; }

    }

    public class group
    {
        [JsonProperty("operator")]
        public string Operator { get; set; }
        public child[] rules { get; set; }

    }

    public enum OperatorRule { Or, And }
    public class CompoundRule : Rule
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Left")]
        public Rule LRule { get; set; }
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OperatorRule Operator { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Right")]
        public Rule RRule { get; set; }
    }

    public class Rule
    {
        [System.ComponentModel.DefaultValue(false)]
        public bool Group { get; set; }
        public bool Not { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.ComponentModel.DefaultValue(0)]
        public int Segment { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        //[System.ComponentModel.DefaultValue(0)]
        public int? recency { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        //[System.ComponentModel.DefaultValue(0)]
        public int? frequency { get; set; }
  

        
    }

    public class RuleVersion
    {
        public int Version { get; set; }
        [JsonProperty("Expression")]
        public Rule Version_1 { get; set; }
    }


    public class RuleConverterConverter : JsonCreationConverter<Rule>
    {
        protected override Rule Create(Type objectType, JObject jObject)
        {
            if (FieldExists("Operator", jObject))
            {
                return new CompoundRule();
            }


            else
            {
                return new Rule();
            }
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>Serializes to the specified type</summary>
        /// <param name="writer">Newtonsoft.Json.JsonWriter</param>
        /// <param name="value">Object to serialize.</param>
        /// <param name="serializer">Newtonsoft.Json.JsonSerializer to use.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }


        /// <summary>Creates a new reader for the specified jObject by copying the settings
        /// from an existing reader.</summary>
        /// <param name="reader">The reader whose settings should be copied.</param>
        /// <param name="jObject">The jObject to create a new reader for.</param>
        /// <returns>The new disposable reader.</returns>
        public static JsonReader CopyReaderForObject(JsonReader reader, JObject jObject)
        {
            JsonReader jObjectReader = jObject.CreateReader();
            jObjectReader.Culture = reader.Culture;
            jObjectReader.DateFormatString = reader.DateFormatString;
            jObjectReader.DateParseHandling = reader.DateParseHandling;
            jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jObjectReader.FloatParseHandling = reader.FloatParseHandling;
            jObjectReader.MaxDepth = reader.MaxDepth;
            jObjectReader.SupportMultipleContent = reader.SupportMultipleContent;
            return jObjectReader;
        }
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);
            // Create target object based on JObject
            T target = Create(objectType, jObject);
            // Populate the object properties
            using (JsonReader jObjectReader = CopyReaderForObject(reader, jObject))
            {
                // serializer.Populate(jObjectReader, target);

                if (objectType != typeof(RuleVersion))
                    serializer.Populate(jObjectReader, target);
                else
                    serializer.Populate(jObjectReader, objectType);

            }
            return target;



        }
    }
}
