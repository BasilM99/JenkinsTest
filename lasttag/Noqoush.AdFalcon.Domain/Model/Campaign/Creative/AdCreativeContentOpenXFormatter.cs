using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Creative
{
    public class AdCreativeContentOpenXFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentOpenXFormatter(AdCreativeUnit adCreativeUnit)
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
            return ClickMacrosList.Any(p => upperContent.Contains(p));
        }

        public override void FormatContent()
        {
            base.FormatContent();

            string upperContent = AdCreativeUnit.Content.ToUpper();
            bool anyImpressionMacro = ImpressionMacrosList.Any(p => upperContent.Contains(p));
            bool anyClickMacro = ClickMacrosList.Any(p => upperContent.Contains(p));


            if (!anyImpressionMacro)
            {

                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("INSERT_RANDOM_NUMBER_HERE", "CACHEBUSTER");

            }


            int startIndex = AdCreativeUnit.Content.ToLower().IndexOf("<a href");
            int endIndex = getFirstAppearance(startIndex,startIndex + "<a href".Length, new char[] { 'h' }, out char foundSeek);
            if (endIndex > 0 && startIndex < endIndex)
            {
                string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                string replacement = "<a href='" + ClickMacro;

                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);

            }

        }

    }
}