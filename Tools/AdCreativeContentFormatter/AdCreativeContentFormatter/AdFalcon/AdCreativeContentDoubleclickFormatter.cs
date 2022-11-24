using System.IO;
using HtmlAgilityPack;

namespace AdCreativeContentFormatter.AdFalcon
{
    public enum DoubleClickFormat
    {
        PlainHtml = 1,
        JavaScript = 2,
        Jsonp = 3


    }
    public class AdCreativeContentDoubleclickFormatter : AdCreativeContentFormatterBase
    {
        private DoubleClickFormat _doubleClickFormat;

        public AdCreativeContentDoubleclickFormatter(AdCreativeUnit adCreativeUnit)
            : base(adCreativeUnit)
        {
            ClickRedirectMacro = Configuration.ClickRedirectMacro;
            ClickMacro = Configuration.ClickMacro;

            ImpressionMacro = Configuration.ImpressionMacro;
            GetContentFormat();

        }
        public string ClickRedirectMacro { get; set; }
        public string ClickMacro { get; set; }
        public string ImpressionMacro { get; set; }

        private void GetContentFormat()
        {
            var content = AdCreativeUnit.Content.ToLower();
            if (content.Contains("document.write"))
            {
                _doubleClickFormat = DoubleClickFormat.JavaScript;
            }
            else if (content.Contains("script") && (content.Contains("src='http") || content.Contains("src=\"http")))
            {
                _doubleClickFormat = DoubleClickFormat.Jsonp;
            }
            else
            {
                _doubleClickFormat = DoubleClickFormat.PlainHtml;
            }

        }

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
            if (!AdCreativeUnit.Content.ToLower().Contains(ImpressionMacro))
            {
                HtmlDocument doc = GetHtmlDocument();
                foreach (HtmlNodeNavigator link in doc.CreateNavigator().Select("//a[@href]"))
                {
                    var html = link.CurrentNode.InnerHtml;
                    var newHtml = html + string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro);
                    link.CurrentNode.InnerHtml = newHtml;
                }
                SaveHtmlDocument(doc);
                switch (_doubleClickFormat)
                {
                    case DoubleClickFormat.JavaScript:
                        {
                            InjectImageJavascript();
                            break;
                        }
                    case DoubleClickFormat.Jsonp:
                        {
                            InjectImageJavascriptWithDocumentWrite();
                            break;
                        }
                }
            }
        }
        private void InjectImageJavascript()
        {
            HtmlDocument doc = GetHtmlDocument();
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script[@type]"))
            {
                foreach (HtmlNodeNavigator link in script.Select("//a[@href]"))
                {
                    var scriptText = script.CurrentNode.InnerHtml;
                    var index = scriptText.ToLower().IndexOf("</a>", System.StringComparison.Ordinal);
                    var newScriptText = scriptText.Insert(index, string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                    script.CurrentNode.InnerHtml = newScriptText;
                }
            }
            SaveHtmlDocument(doc);
        }
        private void InjectImageJavascriptWithDocumentWrite()
        {
            HtmlDocument doc = GetHtmlDocument();
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
            {
                var scriptText = script.CurrentNode.InnerHtml;
                var newScriptText = scriptText + string.Format("document.write('{0}');", string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                script.CurrentNode.InnerHtml = newScriptText;
            }
            SaveHtmlDocument(doc);
        }
        private void UpdateLinkInfo()
        {
            if (!AdCreativeUnit.Content.ToLower().Contains(ClickMacro))
            {
                HtmlDocument doc = GetHtmlDocument();
                foreach (HtmlNodeNavigator link in doc.CreateNavigator().Select("//a[@href]"))
                {
                    var hrefString = link.CurrentNode.GetAttributeValue("href", "");
                    var newHref = ClickMacro + hrefString;
                    link.CurrentNode.SetAttributeValue("href", newHref);
                }
                SaveHtmlDocument(doc);
                switch (_doubleClickFormat)
                {
                    case DoubleClickFormat.JavaScript:
                        {
                            UpdateJavascriptLinkInfo();
                            break;
                        }
                }

            }


            /*
            var content = AdCreativeUnit.Content.ToLower();
            var start_index = content.IndexOf("href=")+5;
            var strat_string = content.Substring(start_index,1);
            start_index += 1;
            var end_index = content.IndexOf(strat_string, start_index );

            var href_string = AdCreativeUnit.Content.Substring(start_index, end_index - start_index);
            //TODO:i think we should encode the url in this case using System.Web.HttpUtility.UrlEncode method
            var new_href = ClickMacro + href_string;

            AdCreativeUnit.Content = AdCreativeUnit.Content.Replace(href_string,new_href);*/

        }

        private void UpdateJavascriptLinkInfo()
        {
            HtmlDocument doc = GetHtmlDocument();
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
            {
                /*foreach (HtmlNodeNavigator link in script.Select("//a[@href]"))
                {
                    var hrefString = link.CurrentNode.GetAttributeValue("href", "");
                    var newHref = ClickMacro + hrefString;
                    link.CurrentNode.SetAttributeValue("href", newHref);
                }*/
                var scriptText = script.CurrentNode.InnerHtml;
                var startIndex = scriptText.ToLower().IndexOf("<a", System.StringComparison.Ordinal); ;
                var endIndex = scriptText.ToLower().IndexOf("</a>", System.StringComparison.Ordinal) + 4;
                var linkInfo = scriptText.Substring(startIndex, endIndex - startIndex);

                var subDoc = GetHtmlDocument(linkInfo);
                foreach (HtmlNodeNavigator link in subDoc.CreateNavigator().Select("//a[@href]"))
                {
                    var hrefString = link.CurrentNode.GetAttributeValue("href", "");
                    var newHref = ClickMacro + hrefString;
                    link.CurrentNode.SetAttributeValue("href", newHref);
                }

                var newLinkInfo = GetHtmlDocumentValue(subDoc);
                var newScriptText = scriptText.Replace(linkInfo, newLinkInfo);
                script.CurrentNode.InnerHtml = newScriptText;
            }
            SaveHtmlDocument(doc);
        }
        public override void FormatContent()
        {
            base.FormatContent();

            switch (_doubleClickFormat)
            {

                case DoubleClickFormat.JavaScript:
                    {
                        InjectImage();
                        UpdateLinkInfo();
                        break;
                    }
                case DoubleClickFormat.Jsonp:
                    {
                        if (!AdCreativeUnit.Content.ToLower().Contains(ImpressionMacro))
                        {
                            InjectImage();
                            //InjectImageJavascriptWithDocumentWrite();
                        }
                        if (!AdCreativeUnit.Content.ToLower().Contains(ClickMacro))
                        {
                            AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("click=;", string.Format("click={0};", ClickMacro));
                            AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("ord=", string.Format("ord={0}", "{CACHEBUSTER}"));
                            UpdateLinkInfo();
                        }
                        break;
                    }
                case DoubleClickFormat.PlainHtml:
                    {
                        InjectImage();
                        UpdateLinkInfo();
                        break;
                    }
            }
        }

        //public override void FormatContent()
        //{
        //    base.FormatContent();

        //    switch (_doubleClickFormat)
        //    {

        //        case DoubleClickFormat.JavaScript:
        //            {
        //                if (!AdCreativeUnit.Content.ToLower().Contains(ImpressionMacro))
        //                {
        //                    var content = AdCreativeUnit.Content.ToLower();
        //                    var startIndex = content.IndexOf("</script>", System.StringComparison.Ordinal);
        //                    var documentWriteImageStr = string.Format("document.write('{0}');",
        //                                                                 string.Format(Configuration.ImpressionImageTemplate,
        //                                                                     ImpressionMacro));
        //                    AdCreativeUnit.Content = AdCreativeUnit.Content.Insert(startIndex, documentWriteImageStr);
        //                }
        //                if (!AdCreativeUnit.Content.ToLower().Contains(ClickMacro))
        //                {
        //                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("click=;", string.Format("click={0};", ClickMacro));
        //                }
        //                InjectImage();
        //                UpdateLinkInfo();
        //                break;
        //            }
        //        case DoubleClickFormat.Jsonp:
        //            {
        //                if (!AdCreativeUnit.Content.ToLower().Contains(ImpressionMacro))
        //                {
        //                    var content = AdCreativeUnit.Content.ToLower();
        //                    var startIndex = content.IndexOf("</script>", System.StringComparison.Ordinal);
        //                    var documentWriteImageStr = string.Format("document.write('{0}');",
        //                                                                 string.Format(Configuration.ImpressionImageTemplate,
        //                                                                     ImpressionMacro));
        //                    AdCreativeUnit.Content = AdCreativeUnit.Content.Insert(startIndex, documentWriteImageStr);
        //                }
        //                if (!AdCreativeUnit.Content.ToLower().Contains(ClickMacro))
        //                {
        //                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("click=;", string.Format("click={0};", ClickMacro));
        //                    AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("ord=", string.Format("ord={0}", "{CACHEBUSTER}"));
        //                    UpdateLinkInfo();
        //                }
        //                break;
        //            }
        //        case DoubleClickFormat.PlainHtml:
        //            {
        //                InjectImage();
        //                UpdateLinkInfo();
        //                break;
        //            }
        //    }
        //}
    }
}