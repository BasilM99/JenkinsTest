using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Threading;
using System.Web;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using System.Web.Mvc.Html;
namespace Noqoush.AdFalcon.Web.Helper
{
    public static class HtmlHelperExtensions
    {
        #region Style and Script Methods

        public static MvcHtmlString RegisterExternalLink<TModel>(this HtmlHelper<TModel> htmlHelper, string url)
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            if (!string.IsNullOrEmpty(url))
            {
                var scriptString = new MvcHtmlString(string.Format("<script src='{0}://ajax.aspnetcdn.com/ajax/{1}'type='text/javascript'></script>", HttpContext.Current.Request.Url.Scheme, url));
                return scriptString;
            }
            throw new ArgumentException("scriptFileName cant be null or empty");
        }

        /// <summary>
        /// Returns script tag with the absolute path for the specified filename.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="scriptFileName">The file name for the src attribute of script tag , the method will build the absolute path for this file.</param>
        /// <returns></returns>
        public static MvcHtmlString RegisterInternalScriptLink<TModel>(this HtmlHelper<TModel> htmlHelper, string scriptFileName)
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;

            if (!string.IsNullOrEmpty(scriptFileName))
            {
                string filePath = VirtualPathUtility.ToAbsolute(string.Format("~/scripts/{0}", scriptFileName));

                var scriptString =
                    new MvcHtmlString(
                        string.Format("<script src=\"{0}\" type=\"text/javascript\" ></script>",
                                      filePath));
                return scriptString;
            }

            throw new ArgumentException("scriptFileName cant be null or empty");
        }

        /// <summary>
        /// Add script code to the page
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public static MvcHtmlString RegisterScriptBlock<TModel>(this HtmlHelper<TModel> htmlHelper, string scriptCode)
        {
            return new MvcHtmlString(string.Format("<script type=\"text/javascript\">{0}</script>", scriptCode));
        }

        /// <summary>
        /// Returns script tag.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="scriptFileName">The value for the src attribute for the script tag.</param>
        /// <returns></returns>
        public static MvcHtmlString RegisterExternalScriptLink<TModel>(this HtmlHelper<TModel> htmlHelper, string scriptFileName)
        {
            if (!string.IsNullOrEmpty(scriptFileName))
            {
                var scriptString =
                    new MvcHtmlString(string.Format("<script src=\"{0}\" type=\"text/javascript\" ></script>", scriptFileName));
                return scriptString;
            }

            throw new ArgumentException("scriptFileName cant be null or empty");
        }

        /// <summary>
        /// Returns link tag with the absolute path for the specified filename.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="styleSheetFileName">The file name for the href attribute of the link tag , the method will build the absolute path for this file</param>
        /// <returns></returns>
        public static MvcHtmlString RegisterInternalStyleSheetLink<TModel>(this HtmlHelper<TModel> htmlHelper, string styleSheetFileName)
        {

            if (!string.IsNullOrEmpty(styleSheetFileName))
            {
                string filePath = VirtualPathUtility.ToAbsolute(string.Format("~/Content/{0}/magenta/style/{1}",
                                                                           Thread.CurrentThread.CurrentUICulture.
                                                                               TwoLetterISOLanguageName,
                                                                           styleSheetFileName));
                var styleString =
                    new MvcHtmlString(string.Format(
                        "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />",
                        filePath));

                return styleString;
            }

            throw new ArgumentException("styleSheetFileName cant be null or empty");
        }

        /// <summary>
        /// Returns link tag 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="styleSheetFileName">The value for the href attribute for the link tag.</param>
        /// <returns></returns>
        public static MvcHtmlString RegisterExternalStyleSheetLink<TModel>(this HtmlHelper<TModel> htmlHelper, string styleSheetFileName)
        {
            if (!string.IsNullOrEmpty(styleSheetFileName))
            {
                var styleString =
                    new MvcHtmlString(string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />",
                                                    styleSheetFileName));
                return styleString;
            }
            throw new ArgumentException("styleSheetFileName cant be null or empty");
        }
        public static MvcHtmlString GetCheckBox<TModel>(this  System.Web.Mvc.HtmlHelper<TModel> htmlHelper, string name, string text, bool isChecked = false, string onChange = "",string customValue="")
        {
            var customOnChange = string.Empty;
            var className = "check-box-uncheck";
            var checkedStatus = false;
            if (isChecked)
            {
                className = "check-box-checked";
                checkedStatus = true;
            }
            if (!string.IsNullOrWhiteSpace(onChange))
            {
                customOnChange = string.Format("customOnChange='{0}'", onChange);
            }

            var html =
                string.Format(
                    @"<div class='check-box-container' {3} onclick='checkBox(this)'>
                            <span class='{0}' id='chbspan'>{1}</span><span class='check-box-text'>{2}</span>
                        </div>",
                    className, htmlHelper.CheckBox(name, checkedStatus, new { Class = "check-box", id = name, customValue = customValue }), text, customOnChange);

            return MvcHtmlString.Create(html);
        }
        public static MvcHtmlString GetRadioButton<TModel>(this  HtmlHelper<TModel> htmlHelper, string name, string text, bool Checked = false, string initClassName = "", string onChange = "", string value = "")
        {
            var className = "radio-button-uncheck";
            var customOnChange = string.Empty;
            var checkedStatus = string.Empty;
            if (Checked)
            {
                className = "radio-button-checked";
                checkedStatus = "checked=checked";
            }
            if (!string.IsNullOrWhiteSpace(onChange))
            {
                customOnChange = string.Format("customOnChange='{0}'", onChange);
            }
            var html =
               string.Format(
                   @"<div name='{2}container' class='radio-button-container {0}' {5} onclick='radioBox(this);'>
                        <span id='rbspan' class='{1}'>
                            <input type='radio' onchange='radioBoxChange(this);' name='{2}' class='radio-box' {3} value='{6}' customValue='{6}' customText='{4}'/></span> <span class='check-box-text'>{4}</span>
                    </div>", initClassName, className, name, checkedStatus, text, customOnChange, value);
            return MvcHtmlString.Create(html);
        }
        #endregion

        #region Resource Methods

        /// <summary>
        /// Get resource strings from ResourceProvider
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceSet"></param>
        /// <returns></returns>
        public static string GetResource<TModel>(this  HtmlHelper<TModel> htmlHelper, string resourceKey, string resourceSet = null)
        {
            return ResourcesUtilities.GetResource(resourceKey, resourceSet);
        }

        #endregion

        #region Control Mehods

        private static string adFlaconTextClass = "text-box";
        private static MvcHtmlString ProcessMvcHtmlString(MvcHtmlString MvcStr)
        {
            var str = MvcStr.ToString();
            if (str.Contains("data-val-required"))
            {
                //this element is required , we need to add the 'required' class to it
                str = str.Replace("class=\"", "class=\"required ").Replace("Class=\"", "Class=\"required ");
                return new MvcHtmlString(str);
            }
            return MvcStr;
        }
        public static MvcHtmlString AdFalconEditor(this HtmlHelper html, string expression)
        {
            var mvcStr = html.Editor(expression);
            return ProcessMvcHtmlString(mvcStr);
        }

        public static MvcHtmlString AdFalconEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var mvcStr = html.EditorFor(expression);
            return ProcessMvcHtmlString(mvcStr);
        }

        public static MvcHtmlString AdFalconEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object additionalViewData)
        {
            var mvcStr = html.EditorFor(expression, additionalViewData);
            return ProcessMvcHtmlString(mvcStr);
        }


        public static MvcHtmlString AdFalconTextBox(this HtmlHelper htmlHelper, string name)
        {
            var mvcStr = htmlHelper.TextBox(name, null, new { Class = adFlaconTextClass });
            return ProcessMvcHtmlString(mvcStr);
        }

        public static MvcHtmlString AdFalconTextBox(this HtmlHelper htmlHelper, string name, object value)
        {
            var mvcStr = htmlHelper.TextBox(name, value, new { Class = adFlaconTextClass });
            return ProcessMvcHtmlString(mvcStr);
        }

        public static MvcHtmlString AdFalconTextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            Dictionary<string, object> dicHtmlAttributes = new Dictionary<string, object>();

            dicHtmlAttributes = GetAnonymousObjectData(htmlAttributes);

            dicHtmlAttributes.Add("class", adFlaconTextClass);

            var mvcStr = htmlHelper.TextBox(name, value, dicHtmlAttributes);
            return ProcessMvcHtmlString(mvcStr);
        }

        public static MvcHtmlString AdFalconTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                               Expression<Func<TModel, TProperty>> expression)
        {


            var mvcStr = htmlHelper.TextBoxFor(expression, new { Class = adFlaconTextClass });
            return ProcessMvcHtmlString(mvcStr);
        }

        public static MvcHtmlString AdFalconTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                  Expression<Func<TModel, TProperty>> expression,
                                                                  object htmlAttributes)
        {
            Dictionary<string, object> dicHtmlAttributes = new Dictionary<string, object>();

            dicHtmlAttributes = GetAnonymousObjectData(htmlAttributes);

            if (!dicHtmlAttributes.ContainsKey("class"))
            {
                dicHtmlAttributes.Add("class", adFlaconTextClass);
            }
            else
            {
                dicHtmlAttributes["class"] = string.Format("{0} {1}", adFlaconTextClass, dicHtmlAttributes["class"]);
            }
            var mvcStr = htmlHelper.TextBoxFor(expression, dicHtmlAttributes);
            return ProcessMvcHtmlString(mvcStr);
        }

        public static MvcHtmlString AdFalconPassword(this HtmlHelper htmlHelper, string name)
        {
            var mvcStr = htmlHelper.Password(name, null, new { Class = adFlaconTextClass });
            mvcStr = ProcessMvcHtmlString(mvcStr);
            return mvcStr;
        }

        public static MvcHtmlString AdFalconPassword(this HtmlHelper htmlHelper, string name, object value)
        {
            var mvcStr = htmlHelper.Password(name, value, new { Class = adFlaconTextClass });
            mvcStr = ProcessMvcHtmlString(mvcStr);
            return mvcStr;
        }

        public static MvcHtmlString AdFalconPassword(this HtmlHelper htmlHelper, string name, object value,
                                        object htmlAttributes)
        {
            Dictionary<string, object> dicHtmlAttributes = new Dictionary<string, object>();

            dicHtmlAttributes = GetAnonymousObjectData(htmlAttributes);

            dicHtmlAttributes.Add("class", adFlaconTextClass);

            var mvcStr = htmlHelper.Password(name, value, dicHtmlAttributes);
            mvcStr = ProcessMvcHtmlString(mvcStr);
            return mvcStr;
        }

        public static MvcHtmlString AdFalconPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                 Expression<Func<TModel, TProperty>> expression)
        {
            var mvcStr = htmlHelper.PasswordFor(expression, new { Class = adFlaconTextClass });
            mvcStr = ProcessMvcHtmlString(mvcStr);
            return mvcStr;
        }

        public static MvcHtmlString AdFalconPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                    object htmlAttributes)
        {
            Dictionary<string, object> dicHtmlAttributes = new Dictionary<string, object>();

            dicHtmlAttributes = GetAnonymousObjectData(htmlAttributes);

            dicHtmlAttributes.Add("class", adFlaconTextClass);

            var mvcStr = htmlHelper.PasswordFor(expression, dicHtmlAttributes);
            mvcStr = ProcessMvcHtmlString(mvcStr);
            return mvcStr;
        }

        public static MvcHtmlString AdFalconValidationMessage(this HtmlHelper htmlHelper, string modelName)
        {
            return htmlHelper.ValidationMessage(modelName, new { Class = "validation-arrow" });
        }

        public static MvcHtmlString AdFalconValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.ValidationMessageFor(expression, null, new { Class = "validation-arrow" });
        }

        public static MvcHtmlString AdFalconDropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            MvcHtmlString dropdown = htmlHelper.DropDownList(name, selectList, htmlAttributes);
            string dropDownFormat = DropDownListTemplate();

            return new MvcHtmlString(string.Format(dropDownFormat, dropdown.ToHtmlString()));
        }

        public static MvcHtmlString AdFalconDropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            MvcHtmlString dropdown = htmlHelper.DropDownList(name, selectList);
            string dropDownFormat = DropDownListTemplate();

            return new MvcHtmlString(string.Format(dropDownFormat, dropdown.ToHtmlString()));
        }


        #endregion

        #region Private Methods

        private static Dictionary<string, object> GetAnonymousObjectData(object htmlAttributes)
        {
            Dictionary<string, object> dicHtmlAttributes = new Dictionary<string, object>();

            foreach (var item in htmlAttributes.GetType().GetProperties())
            {
                dicHtmlAttributes.Add(item.Name, item.GetValue(htmlAttributes, null));
            }

            return dicHtmlAttributes;
        }

        private static string DropDownListTemplate()
        {
            StringBuilder dropDownListBuilder = new StringBuilder();
            //dropDownListBuilder.Append("<div class=\"ddl-container data-row\">");
            dropDownListBuilder.Append("{0}");
            //dropDownListBuilder.Append("<a href=\"#\" class=\"ddl-click\"></a>");
            //dropDownListBuilder.Append("</div>");

            return dropDownListBuilder.ToString();
        }

        #endregion

    }
}
