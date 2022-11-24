using System.IO;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Creative
{
    public enum DoubleClickFormat
    {
        PlainHtml = 1,
        JavaScript = 2,
        Jsonp = 3,
        Jsonp2 = 4


    }
    public class AdCreativeContentDoubleclickFormatter : AdCreativeContentFormatterBase
    {
        private DoubleClickFormat _doubleClickFormat;
       // private ICreativeUnitRepository _CreativeUnitRepository;

        public AdCreativeContentDoubleclickFormatter(AdCreativeUnit adCreativeUnit)
            : base(adCreativeUnit)
        {
            ClickRedirectMacro = Configuration.ClickRedirectMacro;
            ClickMacro = Configuration.ClickMacro;

            ImpressionMacrosList = Configuration.ImpressionMacrosList;
            ClickMacrosList = Configuration.ClickMacrosList;

            ImpressionMacro = Configuration.ImpressionMacro;
            GetContentFormat();

           

        }
        public string ClickRedirectMacro { get; set; }
        public string ClickMacro { get; set; }
        public string ImpressionMacro { get; set; }
        public List<string> ImpressionMacrosList { get; set; }
        public List<string> ClickMacrosList { get; set; }

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
                if (content.Contains("javascript1.1"))
                {
                    _doubleClickFormat = DoubleClickFormat.Jsonp2;

                }
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
            doc.OptionWriteEmptyNodes = true;
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script[@type]"))
            {
                foreach (HtmlNodeNavigator link in script.Select("//a[@href]"))
                {
                    var scriptText = script.CurrentNode.InnerHtml;
                    var index = scriptText.ToLower().IndexOf("</a>", System.StringComparison.Ordinal);
                    var newScriptText = scriptText.Insert(index + 4, string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                    script.CurrentNode.InnerHtml = newScriptText;
                }
            }
            SaveHtmlDocument(doc);
        }
        private void InjectImageJavascriptWithDocumentWrite()
        {
            HtmlDocument doc = GetHtmlDocument();
            doc.OptionWriteEmptyNodes = true;
            int counter = 0;
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
            {
                if (counter == doc.CreateNavigator().Select("//script").Count - 1)
                {
                    var scriptText = script.CurrentNode.InnerHtml;
                    var newScriptText = scriptText + string.Format("document.write('{0}');", string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                    script.CurrentNode.InnerHtml = newScriptText;
                }
                counter++;
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
                switch (_doubleClickFormat)
                {
                    case DoubleClickFormat.JavaScript:
                        {
                            UpdateJavascriptLinkInfo();
                            break;
                        }
                }

            }
        }

        private void UpdateJavascriptLinkInfo()
        {
            HtmlDocument doc = GetHtmlDocument();
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
            {
                var scriptText = script.CurrentNode.InnerHtml;
                var startIndex = scriptText.ToLower().IndexOf("<a", System.StringComparison.Ordinal); ;
                var endIndex = scriptText.ToLower().IndexOf("</a>", System.StringComparison.Ordinal) + 4;
                var linkInfo = scriptText.Substring(startIndex, endIndex - startIndex);

                var subDoc = GetHtmlDocument(linkInfo);
                foreach (HtmlNodeNavigator link in subDoc.CreateNavigator().Select("//a[@href]"))
                {
                    var hrefString = link.CurrentNode.GetAttributeValue("href", "");
                    var newHref = ClickRedirectMacro + hrefString;
                    link.CurrentNode.SetAttributeValue("href", newHref);
                }

                var newLinkInfo = GetHtmlDocumentValue(subDoc);
                var newScriptText = scriptText.Replace(linkInfo, newLinkInfo);
                script.CurrentNode.InnerHtml = newScriptText;
            }
            SaveHtmlDocument(doc);
        }

        public override bool IsFormatted()
        {
            var result = true;

            switch (_doubleClickFormat)
            {

                case DoubleClickFormat.JavaScript:
                    {
                        string upperContent = AdCreativeUnit.Content.ToUpper();
                        result = ImpressionMacrosList.Any(p => upperContent.Contains(p)) && ClickMacrosList.Any(p => upperContent.Contains(p));
                        break;
                    }
                case DoubleClickFormat.Jsonp:
                    {
                        string upperContent = AdCreativeUnit.Content.ToUpper();
                        result = ImpressionMacrosList.Any(p => upperContent.Contains(p)) && ClickMacrosList.Any(p => upperContent.Contains(p));
                        break;
                    }
                case DoubleClickFormat.PlainHtml:
                    {
                        string upperContent = AdCreativeUnit.Content.ToUpper();
                        result = ImpressionMacrosList.Any(p => upperContent.Contains(p)) && ClickMacrosList.Any(p => upperContent.Contains(p));
                        break;
                    }
            }

            return result;
        }

        public override void FormatContent()
        {
            base.FormatContent();
            string upperContent = AdCreativeUnit.Content.ToUpper();
            bool anyImpressionMacro = ImpressionMacrosList.Any(p => upperContent.Contains(p));
            bool anyClickMacro = ClickMacrosList.Any(p => upperContent.Contains(p));

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
                        InjectImage();

                        //string upperContent = AdCreativeUnit.Content.ToUpper();
                        // var anyClickMacro = ClickMacrosList.Any(p => upperContent.Contains(p));

                        if (!anyClickMacro)
                        {
                            AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("click=;", string.Format("click={0};", ClickMacro));
                            AdCreativeUnit.Content = AdCreativeUnit.Content.Replace("ord=", string.Format("ord={0}", "{CACHEBUSTER}"));
                        }
                        UpdateLinkInfo();
                        break;
                    }
                case DoubleClickFormat.PlainHtml:
                    {
                        InjectImage();
                        UpdateLinkInfo();
                        break;
                    }
                case DoubleClickFormat.Jsonp2:
                    {

                        var endgscript = "<script>var adf_imp_trackers = [\"{ IMPRESSION_URL}\"];</script><script src=\"https://cdn01.static.adfalcon.com/static/js/tracking/adfalcon_tracking_manager.js\"></script>";
                        if (!anyClickMacro)
                        {
                            this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("ord=[timestamp]", "ord={CACHEBUSTER};click2={CLICK_URL_RED_ENC};");
                            if (!this.AdCreativeUnit.Content.ToLower().Contains("tfua="))
                                this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("?", "tfua=?");

                            // this.AdCreativeUnit.Content = creativeUnit == null ? this.AdCreativeUnit.Content.Replace("sz=1x1", "sz=320x50") :

                            if (this.AdCreativeUnit.CreativeUnit != null)
                            {
                                this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("sz=1x1", "sz=" + this.AdCreativeUnit.CreativeUnit.Width + "x" + this.AdCreativeUnit.CreativeUnit.Height);
                                this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("sz=widthxheight", "sz=" + this.AdCreativeUnit.CreativeUnit.Width + "x" + this.AdCreativeUnit.CreativeUnit.Height);
                            }
                            this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("data-dcm-https-only", "data-dcm-https-only data-dcm-click-tracker='{CLICK_URL_RED_ENC}'");
                            this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("data-dcm-http-only", "data-dcm-http-only data-dcm-click-tracker='{CLICK_URL_RED_ENC}'");



                        }
                        if (!(upperContent.Contains("SRC=\"{IMPRESSION_URL}\"") || upperContent.Contains("SRC=\'{IMPRESSION_URL}\'")))
                        {
                            AdCreativeUnit.Content = AdCreativeUnit.Content + endgscript;
                        }
                        break;
                    }
            }


        }
    }


    public class AdCreativeContentGoogleTagFormatter : AdCreativeContentFormatterBase
    {
        private DoubleClickFormat _doubleClickFormat;

        public AdCreativeContentGoogleTagFormatter(AdCreativeUnit adCreativeUnit)
            : base(adCreativeUnit)
        {
            ClickRedirectMacro = Configuration.ClickRedirectMacro;
            ClickMacro = Configuration.ClickMacro;

            ImpressionMacrosList = Configuration.ImpressionMacrosList;
            ClickMacrosList = Configuration.ClickMacrosList;

            ImpressionMacro = Configuration.ImpressionMacro;
            GetContentFormat();

        }
        public string ClickRedirectMacro { get; set; }
        public string ClickMacro { get; set; }
        public string ImpressionMacro { get; set; }
        public List<string> ImpressionMacrosList { get; set; }
        public List<string> ClickMacrosList { get; set; }

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
            doc.OptionWriteEmptyNodes = true;
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script[@type]"))
            {
                foreach (HtmlNodeNavigator link in script.Select("//a[@href]"))
                {
                    var scriptText = script.CurrentNode.InnerHtml;
                    var index = scriptText.ToLower().IndexOf("</a>", System.StringComparison.Ordinal);
                    var newScriptText = scriptText.Insert(index + 4, string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                    script.CurrentNode.InnerHtml = newScriptText;
                }
            }
            SaveHtmlDocument(doc);
        }
        private void InjectImageJavascriptWithDocumentWrite()
        {
            HtmlDocument doc = GetHtmlDocument();
            doc.OptionWriteEmptyNodes = true;
            int counter = 0;
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
            {
                if (counter == doc.CreateNavigator().Select("//script").Count - 1)
                {
                    var scriptText = script.CurrentNode.InnerHtml;
                    var newScriptText = scriptText + string.Format("document.write('{0}');", string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro));
                    script.CurrentNode.InnerHtml = newScriptText;
                }
                counter++;
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
                switch (_doubleClickFormat)
                {
                    case DoubleClickFormat.JavaScript:
                        {
                            UpdateJavascriptLinkInfo();
                            break;
                        }
                }

            }
        }

        private void UpdateJavascriptLinkInfo()
        {
            HtmlDocument doc = GetHtmlDocument();
            foreach (HtmlNodeNavigator script in doc.CreateNavigator().Select("//script"))
            {
                var scriptText = script.CurrentNode.InnerHtml;
                var startIndex = scriptText.ToLower().IndexOf("<a", System.StringComparison.Ordinal); ;
                var endIndex = scriptText.ToLower().IndexOf("</a>", System.StringComparison.Ordinal) + 4;
                var linkInfo = scriptText.Substring(startIndex, endIndex - startIndex);

                var subDoc = GetHtmlDocument(linkInfo);
                foreach (HtmlNodeNavigator link in subDoc.CreateNavigator().Select("//a[@href]"))
                {
                    var hrefString = link.CurrentNode.GetAttributeValue("href", "");
                    var newHref = ClickRedirectMacro + hrefString;
                    link.CurrentNode.SetAttributeValue("href", newHref);
                }

                var newLinkInfo = GetHtmlDocumentValue(subDoc);
                var newScriptText = scriptText.Replace(linkInfo, newLinkInfo);
                script.CurrentNode.InnerHtml = newScriptText;
            }
            SaveHtmlDocument(doc);
        }

        public override bool IsFormatted()
        {
            var result = true;

            switch (_doubleClickFormat)
            {

                case DoubleClickFormat.JavaScript:
                    {
                        string upperContent = AdCreativeUnit.Content.ToUpper();
                        result = ImpressionMacrosList.Any(p => upperContent.Contains(p)) && ClickMacrosList.Any(p => upperContent.Contains(p));
                        break;
                    }
                case DoubleClickFormat.Jsonp:
                    {
                        string upperContent = AdCreativeUnit.Content.ToUpper();
                        result = ImpressionMacrosList.Any(p => upperContent.Contains(p)) && ClickMacrosList.Any(p => upperContent.Contains(p));
                        break;
                    }
                case DoubleClickFormat.PlainHtml:
                    {
                        string upperContent = AdCreativeUnit.Content.ToUpper();
                        result = ImpressionMacrosList.Any(p => upperContent.Contains(p)) && ClickMacrosList.Any(p => upperContent.Contains(p));
                        break;
                    }
            }

            return result;
        }

        public override void FormatContent()
        {
            base.FormatContent();

            string upperContent = AdCreativeUnit.Content.ToUpper();
            bool anyImpressionMacro = ImpressionMacrosList.Any(p => upperContent.Contains(p));
            bool anyClickMacro = ClickMacrosList.Any(p => upperContent.Contains(p));

            if (!anyClickMacro)
            {


                this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("data-dcm-https-only", "data-dcm-https-only data-dcm-click-tracker='{CLICK_URL_RED_ENC}'");
                this.AdCreativeUnit.Content = this.AdCreativeUnit.Content.Replace("data-dcm-http-only", "data-dcm-http-only data-dcm-click-tracker='{CLICK_URL_RED_ENC}'");



            }
            if (!(upperContent.Contains("SRC=\"{IMPRESSION_URL}\"") || upperContent.Contains("SRC=\'{IMPRESSION_URL}\'")))
            {
                AdCreativeUnit.Content = AdCreativeUnit.Content + string.Format(Configuration.ImpressionImageTemplate, ImpressionMacro);
            }


        }
    }

}