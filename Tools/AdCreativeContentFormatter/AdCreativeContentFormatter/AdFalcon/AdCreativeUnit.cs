using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCreativeContentFormatter.AdFalcon
{
   /* public enum AdSubType
    {
        ExpandableRichMedia = 1,
        JavaScriptRichMedia = 2,
        JavaScriptInterstitial = 3,
        ExternalUrlInterstitial = 4
    }
    public enum AdTypeIds
    {
        Banner = 1,
        Text = 2,
        PlainHTML = 3,
        RichMedia = 4
    }
    public class AdCreative
    {
        public AdTypeIds TypeId { get; set; }
        public AdSubType? AdSubType { get; set; }
    }*/
    public class AdCreativeUnit
    {
        public string Content { get; set; }
        //public AdCreative AdCreative { get; set; }
    }
}
