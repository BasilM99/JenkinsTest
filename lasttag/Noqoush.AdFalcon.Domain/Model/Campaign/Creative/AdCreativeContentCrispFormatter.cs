using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Creative
{
    public class AdCreativeContentCrispFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentCrispFormatter(AdCreativeUnit adCreativeUnit)
            : base(adCreativeUnit)
        {
            ClickMacro = Configuration.ClickMacro;
            ImpressionMacro = Configuration.ImpressionMacro;
            ImpressionMacrosList = Configuration.ImpressionMacrosList;
            ClickMacrosList = Configuration.ClickMacrosList;
        }

        public string ClickMacro { get; set; }

        public string ImpressionMacro { get; set; }
        public List<String> ImpressionMacrosList { get; set; }
        public List<String> ClickMacrosList { get; set; }

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

            if (!anyImpressionMacro)
            {
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("%VIEWURL%", ImpressionMacro);
            }
            if (!anyClickMacro)
            {
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("%ACTURL%", ClickMacro);
            }
        }
    }


}
