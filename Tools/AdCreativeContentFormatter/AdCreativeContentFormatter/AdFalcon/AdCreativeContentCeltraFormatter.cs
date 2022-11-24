namespace AdCreativeContentFormatter.AdFalcon
{
    public class AdCreativeContentCeltraFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentCeltraFormatter(AdCreativeUnit adCreativeUnit)
            : base(adCreativeUnit)
        {
            ClickMacro = Configuration.ClickMacro;
            ImpressionMacro = Configuration.ImpressionMacro;

        }

        public string ClickMacro { get; set; }

        public string ImpressionMacro { get; set; }

        public override void FormatContent()
        {
            base.FormatContent();
            if (!AdCreativeUnit.Content.ToLower().Contains(ImpressionMacro))
            {
                AdCreativeUnit.Content += string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro);
            }
            if (!AdCreativeUnit.Content.ToLower().Contains(ClickMacro))
            {
                //Celtra 2
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("var c = \"\";", string.Format("var c = \"{0}\";", ClickMacro));
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("var c = '';", string.Format("var c = '{0}';", ClickMacro));
                
                //Celtra 3
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("'clickUrl':''", string.Format("'clickUrl':'{0}'", ClickMacro));
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("\"clickUrl\":\"\"", string.Format("\"clickUrl\":\"{0}\"", ClickMacro));

            }
        }
    }
}