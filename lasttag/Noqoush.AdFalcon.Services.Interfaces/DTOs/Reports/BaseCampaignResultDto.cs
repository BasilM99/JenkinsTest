using Noqoush.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class BaseCampaignResultDto : BaseReportResult
    {
        private string _secsubname = string.Empty;

        [DataMember]
        public string SecondSubName
        {
            get
            {

                if (!string.IsNullOrEmpty(_secsubname))
                {
                    return _secsubname.Trim();

                }
                return string.Empty;

            }
            set
            {

                _secsubname = value;
            }
        }

        public string DisplayName
        {
            get
            {

                if (!string.IsNullOrEmpty(SecondSubName))
                {
                    return SecondSubName.Trim();

                }
                return Name;

            }
        }


        [DataMember]
        public long Impress { get; set; }

        [DataMember]
        public long Clicks { get; set; }


        [DataMember]
        public long UniqueUsers { get; set; }

        [DataMember]
        public long NoOfHits { get; set; }
       


        [DataMember]
        public long UniqueImp { get; set; }

        [DataMember]
        public string ImpUniqueImp { get { return string.Format("{0:n0}", Impress) + "/" + string.Format("{0:n0}", UniqueImp) + "</span>"; } set { } }

        [DataMember]
        public string ClicksUniqueClicks { get { return string.Format("{0:n0}", Clicks) + "/" + string.Format("{0:n0}", UniqueClicks) + "</span>";  } set { } }

        [DataMember]
        public string ImpressString { get { return string.Format("{0:n0}", Impress); } set { } }

        [DataMember]
        public string ClicksString { get { return string.Format("{0:n0}", Clicks); } set { } }

        [DataMember]
        public string UniqueImpString { get { return string.Format("{0:n0}", UniqueImp);  } set { } }

        [DataMember]
        public string UniqueClicksString { get { return string.Format("{0:n0}", UniqueClicks); } set { } }

        [DataMember]
        public long UniqueClicks { get; set; }

        [DataMember]
        public decimal Spend { get; set; }
        [DataMember]
        public decimal AdjustedNetCost { get; set; }
        [DataMember]
        public decimal GrossCost { get; set; }
        [DataMember]
        public decimal BillableCost { get; set; }


        [DataMember]
        public long conv_pr { get; set; }


        [DataMember]
        public long conv_pr_ct { get; set; }
        [DataMember]
        public long conv_pr_vt  { get; set; }


        [DataMember]
        public long conv_ot { get; set; }
        [DataMember]
        public long conv_ot_ct { get; set; }
        [DataMember]
        public long conv_ot_vt { get; set; }
        [DataMember]
        public decimal conv_pr_rev { get; set; }


        //[DataMember]
        public virtual string conv_pr_revText
        {

            get { return FormatHelper.FormatMoney(conv_pr_rev, IsExport); }
            set { }
        }


        [DataMember]
        public decimal conv_pr_ct_rev { get; set; }

        //[DataMember]
        public virtual string conv_pr_ct_revText
        {

            get { return FormatHelper.FormatMoney(conv_pr_ct_rev, IsExport); }
            set { }
        }



        [DataMember]
        public decimal conv_pr_vt_rev { get; set; }


       // [DataMember]
        public virtual string conv_pr_vt_revText
        {

            get { return FormatHelper.FormatMoney(conv_pr_vt_rev, IsExport); }
            set { }
        }


        [DataMember]
        public decimal conv_ot_rev { get; set; }
       // [DataMember]
        public virtual string conv_ot_revText
        {

            get { return FormatHelper.FormatMoney(conv_ot_rev, IsExport); }
            set { }
        }



        [DataMember]
        public decimal conv_ot_ct_rev { get; set; }



        //[DataMember]
        public virtual string conv_ot_ct_revText
        {

            get { return FormatHelper.FormatMoney(conv_ot_ct_rev, IsExport); }
            set { }
        }

        [DataMember]
        public decimal conv_ot_vt_rev { get; set; }

        //[DataMember]
        public virtual string conv_ot_vt_revText
        {

            get { return FormatHelper.FormatMoney(conv_ot_vt_rev, IsExport); }
            set { }
        }



        //[DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string conv_pr_ECPA
        {
            get
            {
                if (conv_pr > 0)


                    return FormatHelper.FormatPercentage(((decimal)BillableCost / conv_pr));
                else
                    return FormatHelper.FormatPercentage(0);
            }
            set { }
        }




       // [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string conv_pr_ct_ECPA
        {
            get
            {
                if (conv_pr_ct > 0)


                    return FormatHelper.FormatPercentage(((decimal)BillableCost / conv_pr_ct));
                else
                    return FormatHelper.FormatPercentage(0);
            }
            set { }
        }


       // [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string conv_pr_vt_ECPA
        {
            get
            {
                if (conv_pr_vt > 0)


                    return FormatHelper.FormatPercentage(((decimal)BillableCost / conv_pr_vt));
                else
                    return FormatHelper.FormatPercentage(0);
            }
            set { }
        }


        //  [DataMember]
        public string AllBillableCost { get { return (FormatHelper.FormatMoney(AdjustedNetCost, IsExport) + "-"+ FormatHelper.FormatMoney(GrossCost, IsExport) +"-"+ FormatHelper.FormatMoney(BillableCost, IsExport)); } set { } }
        [DataMember]
        public decimal TotalSpend { get { return BillableCost; } set { } }
        [DataMember]
        public decimal DataFee { get; set; }

        [DataMember]
        public decimal PlatformFee { get; set; }

        [DataMember]
        public decimal ThirdPartyFee { get; set; }
        [DataMember]
        public virtual decimal AgencyRevenue { get; set; }
        [DataMember]
        public virtual decimal NetCost { get; set; }
        [DataMember]
        public virtual decimal AvgCPC { get; set; }

        [DataMember]
        public virtual decimal CTR { get; set; }

        public virtual string CtrText
        {
            get { return FormatHelper.FormatPercentage(CTR); }
        
        }
        public virtual string SpendText
        {
            get { return FormatHelper.FormatMoney(Spend, IsExport); }
        }
        public virtual string AvgCPCText
        {
            get { return FormatHelper.FormatMoney(AvgCPC, IsExport); }
        }

        public virtual string TotalDataPriceText
        {
            get { return FormatHelper.FormatMoney(DataFee, IsExport); }
        }
        public virtual string TotalSpendText
        {

            get { return FormatHelper.FormatMoney(TotalSpend, IsExport); }
        }

        public virtual string BillableCostText
        {
            get { return FormatHelper.FormatMoney(BillableCost, IsExport);  }
            set { }
        }
        public virtual string GrossCostText
        {
            get { return FormatHelper.FormatMoney(GrossCost, IsExport); }
        }
        public virtual string AdjustedNetCostText
        {
            get { return FormatHelper.FormatMoney(AdjustedNetCost, IsExport); }
        }
        public virtual string NetCostText
        {
            get { return FormatHelper.FormatMoney(NetCost, IsExport); }
        }
        public virtual string AgencyRevenueText
        {
            get { return FormatHelper.FormatMoney(AgencyRevenue, IsExport); }
        }
    }


}
