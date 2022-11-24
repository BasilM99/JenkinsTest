using ArabyAds.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    [ProtoInclude(100,typeof(AdGeoLocationDto))]
    [ProtoInclude(101,typeof(AdPerformanceDto))]
    [ProtoInclude(102,typeof(PerformanceBaseDto))]
    [ProtoInclude(103,typeof(CampaignCardinalityEstimatorDto))]
    [ProtoInclude(104,typeof(CampaignCommonReportDto))]
    [ProtoInclude(105,typeof(MinHashEstimatorDto))]
    public class BaseCampaignResultDto : BaseReportResult
    {
        private string _secsubname = string.Empty;

       [ProtoMember(1)]
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


       [ProtoMember(2)]
        public long Impress { get; set; }

       [ProtoMember(3)]
        public long Clicks { get; set; }


       [ProtoMember(4)]
        public long UniqueUsers { get; set; }

       [ProtoMember(5)]
        public long NoOfHits { get; set; }
       


       [ProtoMember(6)]
        public long UniqueImp { get; set; }

       [ProtoMember(7)]
        public string ImpUniqueImp { get { return string.Format("{0:n0}", Impress) + "/" + string.Format("{0:n0}", UniqueImp) + "</span>"; } set { } }

       [ProtoMember(8)]
        public string ClicksUniqueClicks { get { return string.Format("{0:n0}", Clicks) + "/" + string.Format("{0:n0}", UniqueClicks) + "</span>";  } set { } }

       [ProtoMember(9)]
        public string ImpressString { get { return string.Format("{0:n0}", Impress); } set { } }

       [ProtoMember(10)]
        public string ClicksString { get { return string.Format("{0:n0}", Clicks); } set { } }

       [ProtoMember(11)]
        public string UniqueImpString { get { return string.Format("{0:n0}", UniqueImp);  } set { } }

       [ProtoMember(12)]
        public string UniqueClicksString { get { return string.Format("{0:n0}", UniqueClicks); } set { } }

       [ProtoMember(13)]
        public long UniqueClicks { get; set; }

       [ProtoMember(14)]
        public decimal Spend { get; set; }
       [ProtoMember(15)]
        public decimal AdjustedNetCost { get; set; }
       [ProtoMember(16)]
        public decimal GrossCost { get; set; }
       [ProtoMember(17)]
        public decimal BillableCost { get; set; }


       [ProtoMember(18)]
        public long conv_pr { get; set; }


       [ProtoMember(19)]
        public long conv_pr_ct { get; set; }
       [ProtoMember(20)]
        public long conv_pr_vt  { get; set; }


       [ProtoMember(21)]
        public long conv_ot { get; set; }
       [ProtoMember(22)]
        public long conv_ot_ct { get; set; }
       [ProtoMember(23)]
        public long conv_ot_vt { get; set; }
       [ProtoMember(24)]
        public decimal conv_pr_rev { get; set; }


        //[DataMember]
        public virtual string conv_pr_revText
        {

            get { return FormatHelper.FormatMoney(conv_pr_rev, IsExport); }
            set { }
        }


       [ProtoMember(25)]
        public decimal conv_pr_ct_rev { get; set; }

        //[DataMember]
        public virtual string conv_pr_ct_revText
        {

            get { return FormatHelper.FormatMoney(conv_pr_ct_rev, IsExport); }
            set { }
        }



       [ProtoMember(26)]
        public decimal conv_pr_vt_rev { get; set; }


       //[ProtoMember()]
        public virtual string conv_pr_vt_revText
        {

            get { return FormatHelper.FormatMoney(conv_pr_vt_rev, IsExport); }
            set { }
        }


       [ProtoMember(27)]
        public decimal conv_ot_rev { get; set; }
       //[ProtoMember()]
        public virtual string conv_ot_revText
        {

            get { return FormatHelper.FormatMoney(conv_ot_rev, IsExport); }
            set { }
        }



       [ProtoMember(28)]
        public decimal conv_ot_ct_rev { get; set; }



        //[DataMember]
        public virtual string conv_ot_ct_revText
        {

            get { return FormatHelper.FormatMoney(conv_ot_ct_rev, IsExport); }
            set { }
        }

       [ProtoMember(29)]
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




       //[ProtoMember()]
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


       //[ProtoMember()]
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


        // [ProtoMember()]
        public string AllBillableCost { get { return (FormatHelper.FormatMoney(AdjustedNetCost, IsExport) + "-"+ FormatHelper.FormatMoney(GrossCost, IsExport) +"-"+ FormatHelper.FormatMoney(BillableCost, IsExport)); } set { } }
       [ProtoMember(30)]
        public decimal TotalSpend { get { return BillableCost; } set { } }
       [ProtoMember(31)]
        public decimal DataFee { get; set; }

       [ProtoMember(32)]
        public decimal PlatformFee { get; set; }

       [ProtoMember(33)]
        public decimal ThirdPartyFee { get; set; }
       [ProtoMember(34)]
        public virtual decimal AgencyRevenue { get; set; }
       [ProtoMember(35)]
        public virtual decimal NetCost { get; set; }
       [ProtoMember(36)]
        public virtual decimal AvgCPC { get; set; }

       [ProtoMember(37)]
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
