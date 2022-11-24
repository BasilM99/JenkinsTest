using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.API.Core
{
    public class APIRoute : Route
    {

        private Regex _domainRegex;
        private string _domainTemplate { get; set; }

        public APIRoute(IRouter target, string domainTemplate, string routeTemplate, IInlineConstraintResolver inlineConstraintResolver) : base(target, routeTemplate, inlineConstraintResolver)
        {
            _domainTemplate = domainTemplate;
        }

        public APIRoute(IRouter target, string domainTemplate, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints, RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver) : base(target, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _domainTemplate = domainTemplate;
        }

        public APIRoute(IRouter target, string domainTemplate, string routeName, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints, RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver) : base(target, routeName, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _domainTemplate = domainTemplate;
        }

      

        public override VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return base.GetVirtualPath(context);
        }

        protected override VirtualPathData OnVirtualPathGenerated(VirtualPathContext context)
        {
            return base.OnVirtualPathGenerated(context);
        }


        protected override Task OnRouteMatched(RouteContext context)
        {
            _domainRegex = CreateRegex(_domainTemplate, true);
            var httpContext = context.HttpContext;
            // Request information
            string requestDomain = httpContext.Request.Headers["host"];
            /*if (!string.IsNullOrEmpty(requestDomain))
            {
                if (requestDomain.IndexOf(":") > 0)
                {
                    requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"));
                }
            }
            else
            {
                requestDomain = httpContext.Request.Host.ToString();
            }*/

            Match domainMatch = _domainRegex.Match(requestDomain.ToLower());
            if (domainMatch.Success)
            {
                for (int i = 0; i < domainMatch.Groups.Count; i++)
                {
                    var group = domainMatch.Groups[i];
                    if (group.Name == "apitype")
                    {
                        context.RouteData.Values["controller"] = group.Value + context.RouteData.Values["controller"];
                    }
                }

            }
            var task = base.OnRouteMatched(context);
            return task;
        }
           

        private Regex CreateRegex(string source, bool domain)
        {
            string pattern = (domain ? "([a-zA-Z0-9,-]*)" : "([a-zA-Z0-9,.]*)");

            // Perform replacements
            source = source.Replace("/", @"\/?");
            source = source.Replace(".", @"\.?");
            source = source.Replace("-", @"\-?");

            MatchCollection collections = Regex.Matches(source, @"{\w+}");

            if (Constraints != null)
            {
                foreach (Match item in collections)
                {
                    string element = Constraints.Keys.Where(p => p.ToLower() == item.Value.Replace("}", "").Replace("{", "").ToLower()).SingleOrDefault();

                    if (element != null)
                    {
                        source = source.Replace("{" + element + "}", "(?<" + element + ">(" + Constraints[element] + "))");
                    }
                    else
                    {
                        source = source.Replace(item.Value, "(?<" + item.Value.Replace("}", "").Replace("{", "") + ">" + pattern + ")");
                    }
                }
            }
            else
            {
                source = source.Replace("{", "(?<");
                source = source.Replace("}", ">" + pattern + ")");
            }

            return new Regex("^" + source + "$");
        }
    }
}
