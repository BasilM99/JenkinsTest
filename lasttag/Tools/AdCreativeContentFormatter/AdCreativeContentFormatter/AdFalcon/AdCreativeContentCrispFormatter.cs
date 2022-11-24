namespace AdCreativeContentFormatter.AdFalcon
{
    public class AdCreativeContentCrispFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentCrispFormatter(AdCreativeUnit adCreativeUnit)
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
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("%VIEWURL%", ImpressionMacro);
            }
            if (!AdCreativeUnit.Content.ToLower().Contains(ClickMacro))
            {
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("%CLICKURL%", ClickMacro);
            }
        }
    }
}