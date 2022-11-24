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
    public class BaseDealResultDto
    {
        public bool IsExport = false;

        [DataMember]
        public bool  GroupByCampId { get; set; }
        [DataMember]
        public int AdgroupId { get; set; }
        [DataMember]
        public int CampaignId { get; set; }

        [DataMember]
        public long TotalCount { get; set; }
        [DataMember]
        public long requests_dcr { get; set; }
        [DataMember]
        public long TotalAvailableImpressions { get; set; }
        [DataMember]
        public long AvailableImpressions { get; set; }
        [DataMember]
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

        [DataMember]
        public long AdResponse { get {

                if (string.IsNullOrEmpty(SubName))
                    return AvailableImpressions - unfilledrequests;
                else
                    return requests_dcr ;
            } set { } }

        [DataMember]
        public long WonImpressions { get; set; }
        [DataMember]

        public long unfilledrequests { get; set; }


        [DataMember]
        public long DisplayedImpressions { get; set; }

        public long getWins()
        {
            return WonImpressions < DisplayedImpressions ? DisplayedImpressions : WonImpressions;
        }

        private string _name = string.Empty;
        [DataMember]
        public string DateRange;
        private string _subname = string.Empty;

        [DataMember]
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
        [DataMember]
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
