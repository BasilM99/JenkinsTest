using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Creative
{

    public class AdCreativeContentVicinitySoftFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentVicinitySoftFormatter(AdCreativeUnit adCreativeUnit)
            : base(adCreativeUnit)
        {
            ClickMacro = Configuration.ClickMacro;
            ImpressionMacro = Configuration.ImpressionMacro;
            ImpressionMacrosList = Configuration.ImpressionMacrosList;
            ClickMacrosList = Configuration.ClickMacrosList;
            ClickRedirectMacro = Configuration.ClickRedirectMacro;
        }
        public string ClickRedirectMacro { get; set; }
        public string ClickMacro { get; set; }

        public string ImpressionMacro { get; set; }
        public List<String> ImpressionMacrosList { get; set; }
        public List<String> ClickMacrosList { get; set; }


        private HtmlDocument GetHtmlDocument(string content = null)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(string.IsNullOrWhiteSpace(content) ? AdCreativeUnit.Content : content);
            return doc;
        }
        private void SaveHtmlDocument(HtmlDocument doc)
        {
            AdCreativeUnit.Content = GetHtmlDocumentValue(doc);
        }
        private static string GetHtmlDocumentValue(HtmlDocument doc)
        {
            string result = null;
            using (var writer = new StringWriter())
            {
                doc.Save(writer);
                result = writer.GetStringBuilder().ToString();
            }
            return result;
        }

        private void InjectImage()
        {
            string upperContent = AdCreativeUnit.Content.ToUpper();
            var anyImpressionMacro = ImpressionMacrosList.Any(p => upperContent.Contains(p));

            if (!anyImpressionMacro)
            {
                HtmlDocument doc = GetHtmlDocument();
                doc.OptionWriteEmptyNodes = true;

                var rootNavigator = doc.CreateNavigator();
                rootNavigator.MoveToRoot();
                foreach (HtmlNodeNavigator item in rootNavigator.Select("/"))
                {
                    foreach (HtmlNodeNavigator link in item.CreateNavigator().Select("//a[@href]"))
                    {
                        HtmlDocument imageDoc = new HtmlDocument();

                        imageDoc.LoadHtml(string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                        imageDoc.OptionWriteEmptyNodes = true;

                        var imageNavigator = imageDoc.CreateNavigator();
                        imageNavigator.MoveToRoot();

                        foreach (HtmlNodeNavigator item1 in imageNavigator.Select("/"))
                        {
                            var linkParent = link.CurrentNode.ParentNode;
                            if (linkParent != null)
                            {
                                linkParent.InsertAfter(item1.CurrentNode, link.CurrentNode);
                            }
                            else
                            {
                                item.CurrentNode.InsertAfter(item1.CurrentNode, link.CurrentNode);
                            }
                        }
                    }

                }
                SaveHtmlDocument(doc);

                InjectImageJavascript();


            }
        }
        private void InjectImageJavascript()
        {
            HtmlDocument doc = GetHtmlDocument();
            doc.OptionWriteEmptyNodes = true;
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//noscript"))
            {
                foreach (HtmlNodeNavigator link in script.Select("//a[@href]"))
                {
                    var scriptText = script.CurrentNode.InnerHtml;
                    var index = scriptText.ToLower().IndexOf("</a>", System.StringComparison.Ordinal);
                    if (!scriptText.Contains("<image"))
                    {
                        var newScriptText = scriptText.Insert(index + 4, string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                        script.CurrentNode.InnerHtml = newScriptText;
                    }
                }
            }
            SaveHtmlDocument(doc);
        }

        private void UpdateLinkInfo()
        {
            string upperContent = AdCreativeUnit.Content.ToUpper();
            var anyClickMacro = ClickMacrosList.Any(p => upperContent.Contains(p));

            if (!anyClickMacro)
            {
                HtmlDocument doc = GetHtmlDocument();
                foreach (HtmlNodeNavigator link in doc.CreateNavigator().Select("//a[@href]"))
                {
                    var hrefString = link.CurrentNode.GetAttributeValue("href", "");
                    var newHref = ClickRedirectMacro + hrefString;
                    link.CurrentNode.SetAttributeValue("href", newHref);
                }
                SaveHtmlDocument(doc);

                //UpdateJavascriptLinkInfo();


            }

        }

        private void UpdateLinkInfoCacheBuster()
        {
            HtmlDocument doc = GetHtmlDocument();
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
            {
                var srcString = script.CurrentNode.GetAttributeValue("src", "");
                if (!srcString.Contains("CACHEBUSTER"))
                    srcString = srcString.Replace("ord=", string.Format("ord={0}", "{CACHEBUSTER}"));
                if (!srcString.ToUpper().Contains("NCU=$$"))
                    srcString = srcString + "ncu=$${CLICK_URL_ENC}$$";

                if (!srcString.ToUpper().Contains("NPU"))
                    srcString = srcString + "npui=1";

                script.CurrentNode.SetAttributeValue("src", srcString);
            }
            SaveHtmlDocument(doc);
        }
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
            string beginScript = "var OA_vscript_t = OA_vscript_t || null, OA_vscript_e = OA_vscript_e || null;";
            if (!upperContent.Contains("VAR OA_VSCRIPT_T"))
            {

                AdCreativeUnit.Content = AdCreativeUnit.Content.Insert(AdCreativeUnit.Content.IndexOf("var vicinityTag"), beginScript);
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("document.getElementsByTagName('script')[0]", " OA_vscript_e || document.getElementsByTagName('script')[0]");
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("document.createElement('script')", " OA_vscript_t || document.createElement('script')");
            }


            if (!anyClickMacro)
            {
            
                int startIndex = AdCreativeUnit.Content.ToLower().IndexOf("click_macro");
                int endIndex = getFirstAppearance(startIndex,startIndex + "click_macro".Length, new char[] { ';', '/', 'v' }, out char foundSeek);
                if (endIndex > 0)
                {
                    string orString = AdCreativeUnit.Content.Substring(startIndex, endIndex - startIndex);
                    string replacement = string.Format("click_macro = '{0}'", "{CLICK_URL_RED_ENC}");
                    if (foundSeek == 'v' || foundSeek == '/')
                    {
                        replacement = string.Format("click_macro = '{0}';" + System.Environment.NewLine, "{CLICK_URL_RED_ENC}");
                    }

                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(orString, replacement);

                }

            }
            if (!(upperContent.Contains("SRC=\"{IMPRESSION_URL}\"") || upperContent.Contains("SRC=\'{IMPRESSION_URL}\'")))
            {


                AdCreativeUnit.Content = AdCreativeUnit.Content + "<div id=\"banner_560bb3192e01251\"></div>" + string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro);
            }


        }
    }
}