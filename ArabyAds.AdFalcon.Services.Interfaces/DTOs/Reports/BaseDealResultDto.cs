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
    [ProtoInclude(100,typeof(DealPerformanceDto))]
    public class BaseDealResultDto
    {
        public bool IsExport = false;

       [ProtoMember(1)]
        public bool  GroupByCampId { get; set; }
       [ProtoMember(2)]
        public int AdgroupId { get; set; }
       [ProtoMember(3)]
        public int CampaignId { get; set; }

       [ProtoMember(4)]
        public long TotalCount { get; set; }
       [ProtoMember(5)]
        public long requests_dcr { get; set; }
       [ProtoMember(6)]
        public long TotalAvailableImpressions { get; set; }
       [ProtoMember(7)]
        public long AvailableImpressions { get; set; }
       [ProtoMember(8)]
        public long FinalAvailableImpressions { get {


                if (!string.IsNullOrEmpty(SubName))
                {
                    return TotalAvailableImpressions;

                }
                else
                {
                    return AvailableImpressions;

                }
            } set {


            } }

       [ProtoMember(9)]
        public long AdResponse { get {

                if (string.IsNullOrEmpty(SubName))
                    return AvailableImpressions - unfilledrequests;
                else
                    return requests_dcr ;
            } set { } }

       [ProtoMember(10)]
        public long WonImpressions { get; set; }
       [ProtoMember(11)]

        public long unfilledrequests { get; set; }


       [ProtoMember(12)]
        public long DisplayedImpressions { get; set; }

        public long getWins()
        {
            return WonImpressions < DisplayedImpressions ? DisplayedImpressions : WonImpressions;
        }

        private string _name = string.Empty;
       [ProtoMember(13)]
        public string DateRange;
        private string _subname = string.Empty;

       [ProtoMember(14)]
        public string SubName
        {
            get
            {

                if (!string.IsNullOrEmpty(_subname))
                {
                    return _subname.Trim();

                }
                return string.Empty;

            }
            set
            {

                _subname = value;
            }
        }
       [ProtoMember(15)]
        public string Name
        {
            get
            {

                if (!string.IsNullOrEmpty(_name))
                {
                    return _name.Trim();

                }
                return string.Empty;

            }
            set
            {

                _name = value;
            }
        }

        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string ResponseRateText
        {
            get {

                if (FinalAvailableImpressions != 0)
                    return FormatHelper.FormatPercentage(((decimal)AdResponse / FinalAvailableImpressions));
                else
                    return FormatHelper.FormatPercentage(0);

             


            }
        }
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string WinRateText
        {
            get {

                if (AdResponse != 0)
                    return FormatHelper.FormatPercentage(((decimal)getWins() / AdResponse));
                else
                    return FormatHelper.FormatPercentage(0);

     

            }
        }
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string DisplayRateText
        {
            get {
                if(WonImpressions!=0)
                return FormatHelper.FormatPercentage(((decimal)DisplayedImpressions / getWins()));
                else
                return FormatHelper.FormatPercentage(0);
            }
        }
    }
}
