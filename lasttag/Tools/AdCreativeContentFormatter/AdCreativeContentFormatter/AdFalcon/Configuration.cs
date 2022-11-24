namespace AdCreativeContentFormatter.AdFalcon
{
    public static class Configuration
    {
        static Configuration()
        {
            ClickMacro = "{CLICK_URL}";
            ImpressionMacro = "{IMPRESSION_URL}";
            ClickRedirectMacro = "{CLICK_URL_RED}";
            ImpressionRedirectMacro = "{IMPRESSION_URL_RED}";
            ImpressionImageTemplate = "<img src=\"{0}\" width=\"1\" height=\"1\"/>";


        }
        public static string ClickMacro { get; set; }
        public static string ImpressionMacro { get; set; }
        public static string ImpressionImageTemplate { get; set; }
        public static string ClickRedirectMacro { get; set; }
        public static string ImpressionRedirectMacro { get; set; }
    }
}