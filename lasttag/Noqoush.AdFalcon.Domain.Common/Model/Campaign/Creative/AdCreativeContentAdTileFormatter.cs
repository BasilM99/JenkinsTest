 using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Creative
{
    public class AdCreativeContentAdTileFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentAdTileFormatter(AdCreativeUnit adCreativeUnit)
            : base(adCreativeUnit)
        {
            ClickMacro = Configuration.ClickMacro;
            ImpressionMacro = Configuration.ImpressionMacro;

            ImpressionMacrosList = Configuration.ImpressionMacrosList;
            ClickMacrosList = Configuration.ClickMacrosList;
        }

        public string ClickMacro { get; set; }

        public string ImpressionMacro { get; set; }
        public List<string> ImpressionMacrosList { get; set; }
        public List<string> ClickMacrosList { get; set; }

        public override bool IsFormatted()
        {
            string upperContent = AdCreativeUnit.Content.ToUpper();
            return ImpressionMacrosList.Any(p => upperContent.Contains(p)) && ClickMacrosList.Any(p => upperContent.Contains(p));

            //return AdCreativeUnit.Content.ToUpper().Contains(ImpressionMacro) && AdCreativeUnit.Content.ToUpper().Contains(ClickMacro);
        }

        public override void FormatContent()
        {
            base.FormatContent();

            string upperContent = AdCreativeUnit.Content.ToUpper();
            bool anyImpressionMacro = ImpressionMacrosList.Any(p => upperContent.Contains(p));
            bool anyClickMacro = ClickMacrosList.Any(p => upperContent.Contains(p));
            bool replace = false;

            if (!anyImpressionMacro)
            {




             
                int startIndex = AdCreativeUnit.Content.ToLower().IndexOf("'impression'");
                int endIndex = getFirstAppearance(startIndex,startIndex + "'impression'".Length, new char[] { ',', '}' }, out  char foundSeek);
                if (endIndex > 0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, string.Format("'impression':'{0}'", ImpressionMacro));
                    replace = true;
                }
                 startIndex = AdCreativeUnit.Content.ToLower().IndexOf("\"impression\"");
                 endIndex = getFirstAppearance(startIndex,startIndex + "\"impression\"".Length, new char[] { ',', '}' }, out  foundSeek);
                if (endIndex > 0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, string.Format("\"impression\":\"{0}\"", ImpressionMacro));
                    replace = true;
                }

                if(replace==false)
                {

                    startIndex = AdCreativeUnit.Content.ToLower().IndexOf("impression");
                    endIndex = getFirstAppearance(startIndex, startIndex + "impression".Length, new char[] { ',', '}' }, out foundSeek);
                    if (endIndex > 0)
                    {
                        string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                        AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, string.Format("impression:\"{0}\"", ImpressionMacro));

                    }
                }

            }
            if (!anyClickMacro)
            {



                replace = false;
                int startIndex = AdCreativeUnit.Content.ToLower().IndexOf("'viewclickmacro'");
                int endIndex = getFirstAppearance(startIndex,startIndex + "'viewClickMacro'".Length, new char[] { ',', 's', '}' }, out char foundSeek);
                if (endIndex > 0 && startIndex>=0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    string replacement = string.Format("'viewClickMacro':'{0}'", "{CLICK_URL_RED}");

                    if (foundSeek == 's')
                    {
                        replacement = string.Format("'viewClickMacro':'{0}'," + System.Environment.NewLine, "{CLICK_URL_RED}");
                    }

                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);
                    replace = true;
                }



                startIndex = AdCreativeUnit.Content.ToLower().IndexOf("\"viewclickmacro\"");
                endIndex = getFirstAppearance(startIndex,startIndex + "\"viewClickMacro\"".Length, new char[] { ',', 's', '}' }, out  foundSeek);
                if (endIndex > 0 && startIndex >= 0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    string replacement = string.Format("\"viewClickMacro\":\"{0}\"", "{CLICK_URL_RED}");

                    if (foundSeek == 's')
                    {
                        replacement = string.Format("\"viewClickMacro\":\"{0}\"," + System.Environment.NewLine, "{CLICK_URL_RED}");
                    }

                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);
                    replace = true;
                }


                if (replace==false)
                {

                    startIndex = AdCreativeUnit.Content.ToLower().IndexOf("viewclickmacro");
                    endIndex = getFirstAppearance(startIndex, startIndex + "viewClickMacro".Length, new char[] { ',', 's', '}' }, out foundSeek);
                    if (endIndex > 0)
                    {
                        string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                        string replacement = string.Format("viewClickMacro:\"{0}\"", "{CLICK_URL_RED}");

                        if (foundSeek == 's')
                        {
                            replacement = string.Format("viewClickMacro:\"{0}\"," + System.Environment.NewLine, "{CLICK_URL_RED}");
                        }

                        AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);

                    }
                }
               



            }

        }
       
    }
}