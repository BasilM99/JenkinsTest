using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Creative
{

    public class AdCreativeContentAtalaSoftFormatter : AdCreativeContentFormatterBase
    {
        public AdCreativeContentAtalaSoftFormatter(AdCreativeUnit adCreativeUnit)
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

            if (!anyClickMacro)
            {
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("idfa=", "idfa={DEVICE_IDFA}");
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("aaid=", "aaid={DEVICE_IDFA}");
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("cache=", "cache={CACHEBUSTER}");
                AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("click=", "click={CLICK_URL_RED_ENC}");
            }
            if (!(upperContent.Contains("SRC=\"{IMPRESSION_URL}\"") || upperContent.Contains("SRC=\'{IMPRESSION_URL}\'")))
            {


                AdCreativeUnit.Content = AdCreativeUnit.Content + string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro);
            }


        }
    }
}