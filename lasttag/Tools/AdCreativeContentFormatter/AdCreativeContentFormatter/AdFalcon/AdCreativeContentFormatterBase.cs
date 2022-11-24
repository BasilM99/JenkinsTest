using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdCreativeContentFormatter.AdFalcon
{
    public class AdCreativeContentFormatterBase
    {
        private const string Doubleclick = "doubleclick";
        private const string Celtra = "celtra";
        private const string Crisp = "crispadvertising";


        public AdCreativeUnit AdCreativeUnit { get; set; }

        public AdCreativeContentFormatterBase(AdCreativeUnit adCreativeUnit)
        {
            AdCreativeUnit = adCreativeUnit;
        }

        public virtual void FormatContent()
        {
        }
        public static AdCreativeContentFormatterBase GetAdCreativeContentFormatter(AdCreativeUnit adCreativeUnit)
        {
            var adCreativeContentFormatter = new AdCreativeContentFormatterBase(adCreativeUnit);
            if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
            {

                if (adCreativeUnit.Content.ToLower().Contains(Celtra))
                {
                    adCreativeContentFormatter = new AdCreativeContentCeltraFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Doubleclick))
                {
                    adCreativeContentFormatter = new AdCreativeContentDoubleclickFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Crisp))
                {
                    adCreativeContentFormatter = new AdCreativeContentCrispFormatter(adCreativeUnit);
                }

            }
            return adCreativeContentFormatter;
        }
    }
}
