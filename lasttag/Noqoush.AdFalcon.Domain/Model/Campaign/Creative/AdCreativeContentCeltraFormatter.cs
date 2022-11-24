using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Creative
{
    public class AdCreativeContentCeltraFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentCeltraFormatter(AdCreativeUnit adCreativeUnit)
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
                AdCreativeUnit.Content += string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro);
            }
            if (!anyClickMacro)
            {
            

                int startIndex = AdCreativeUnit.Content.ToLower().IndexOf("var c");
                int endIndex = getFirstAppearance(startIndex,startIndex + "var c".Length, new char[] { ';', 'v' }, out char foundSeek);
                if (endIndex > 0 && startIndex > 0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    string replacement = string.Format("var c = \"{0}\"", ClickMacro);
                    if (foundSeek == 'v')
                    {
                        replacement = string.Format("var c = \"{0}\";" + System.Environment.NewLine, ClickMacro);
                    }

                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);
                   
                }



                startIndex = AdCreativeUnit.Content.ToLower().IndexOf("'clickurl'");
                endIndex = getFirstAppearance(startIndex,startIndex + "'clickUrl'".Length, new char[] { ',', '\'', '}' }, out foundSeek);
                if (endIndex > 0 && startIndex > 0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    string replacement = string.Format("'clickUrl':'{0}'", ClickMacro);
                    if (foundSeek == '\'')
                    {
                        replacement = string.Format("'clickUrl':'{0}',", ClickMacro);
                    }

                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);
                    replace = true;
                }

                startIndex = AdCreativeUnit.Content.ToLower().IndexOf("\"clickurl\"");
                endIndex = getFirstAppearance(startIndex,startIndex + "\"clickUrl\"".Length, new char[] { ',', '\'','}' }, out foundSeek);
                if (endIndex > 0 && startIndex > 0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    string replacement = string.Format("\"clickUrl\":\"{0}\"", ClickMacro);
                    if (foundSeek == '\'')
                    {
                        replacement = string.Format("\"clickUrl\":\"{0}\",", ClickMacro);
                    }

                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);
                    replace = true;
                }
                if (replace==false)
                {
                    startIndex = AdCreativeUnit.Content.ToLower().IndexOf("clickurl");
                    endIndex = getFirstAppearance(startIndex, startIndex + "clickUrl".Length, new char[] { ',', '\'', '}' }, out foundSeek);
                    if (endIndex > 0 && startIndex > 0)
                    {
                        string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                        string replacement = string.Format("clickUrl:'{0}'", ClickMacro);
                        if (foundSeek == '\'')
                        {
                            replacement = string.Format("clickUrl:'{0}',", ClickMacro);
                        }

                        AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);

                    }
                }

            }
        }
    }
}