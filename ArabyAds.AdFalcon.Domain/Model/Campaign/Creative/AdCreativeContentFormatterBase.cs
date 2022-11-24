using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Creative
{
    public class AdCreativeContentFormatterBase
    {


        private const string HTTP_PREFIX = "{0}HTTP:";
        public AdCreativeUnit AdCreativeUnit { get; set; }

        public AdCreativeContentFormatterBase(AdCreativeUnit adCreativeUnit)
        {
            AdCreativeUnit = adCreativeUnit;
        }

        public virtual void FormatContent()
        {
        }

      
        public virtual bool IsFormatted()
        {
            string upperContent = AdCreativeUnit.Content.ToUpper();

            return Configuration.ImpressionMacrosList.Any(p => upperContent.Contains(p)) && Configuration.ClickMacrosList.Any(p => upperContent.Contains(p));

        }
        public static AdCreativeContentFormatterBase GetAdCreativeContentFormatter(AdCreativeUnit adCreativeUnit)
        {
            var adCreativeContentFormatter = new AdCreativeContentFormatterBase(adCreativeUnit);
            if (!string.IsNullOrWhiteSpace(adCreativeUnit.Content))
            {

                if (adCreativeUnit.Content.ToLower().Contains(Configuration.Celtra))
                {
                    adCreativeContentFormatter = new AdCreativeContentCeltraFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.Doubleclick))
                {
                    adCreativeContentFormatter = new AdCreativeContentDoubleclickFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.MediaPlex))
                {
                    adCreativeContentFormatter = new AdCreativeContentMediaPlexFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.GoogleTag.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentGoogleTagFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.Crisp))
                {
                    adCreativeContentFormatter = new AdCreativeContentCrispFormatter(adCreativeUnit);
                }

                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.Sizmik.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentSizmikFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.FlashTalking.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentFlashTalkingFormatter(adCreativeUnit);
                }

                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.Adlit.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentAdTileFormatter(adCreativeUnit);
                }

                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.AtlaSoft.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentAtalaSoftFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.Vicinity.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentVicinitySoftFormatter(adCreativeUnit);
                }
                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.OpenX.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentOpenXFormatter(adCreativeUnit);
                }

                else if (adCreativeUnit.Content.ToLower().Contains(Configuration.AdForm.ToLower()))
                {
                    adCreativeContentFormatter = new AdCreativeContentAdFormFormatter(adCreativeUnit);
                }
            }
            return adCreativeContentFormatter;
        }

        public int getFirstAppearance(int FoundStartIndex,  int startIndex, char[] seek, out char foundSeek)
        {
            if (!(FoundStartIndex >= 0))
            {
                foundSeek = ' ';
                return -1;
            }
            string lowerrContent = AdCreativeUnit.Content.ToLower();
            for (int i = startIndex; i < lowerrContent.Length; i++)
            {
                for (int z = 0; z < seek.Count(); z++)
                {
                    if (lowerrContent[i] == seek[z])
                    {
                        foundSeek = seek[z];
                        return i;
                    }

                }

            }
            foundSeek = ' ';
            return -1;
        }
    }
    
}
