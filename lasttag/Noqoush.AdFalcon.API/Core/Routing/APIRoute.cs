using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Noqoush.AdFalcon.API.Core.Routing
{
    public class APIRoute : Route
    {
        private Regex domainRegex;
        private Regex pathRegex;

        public string Domain { get; set; }

        public APIRoute(string domain, string url, RouteValueDictionary defaults)
            : base(url, defaults, new MvcRouteHandler())
        {
            Domain = domain;
        }

        public APIRoute(string domain, string url, RouteValueDictionary constraints, RouteValueDictionary defaults)
            : base(url, defaults,constraints, new MvcRouteHandler())
        {
            Domain = domain;
        }

        public APIRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            Domain = domain;
        }

        public APIRoute(string domain, string url, object defaults)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            Domain = domain;
        }

        public APIRoute(string domain, string url, object constraints, object defaults)
            : base(url, new RouteValueDictionary(defaults),new RouteValueDictionary(constraints), new MvcRouteHandler())
        {
            Domain = domain;
        }

        public APIRoute(string domain, string url, object defaults, IRouteHandler routeHandler)
            : base(url, new RouteValueDictionary(defaults), routeHandler)
        {
            Domain = domain;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Build regex
            domainRegex = CreateRegex(Domain, true);
            pathRegex = CreateRegex(Url, false);

            // Request information
            string requestDomain = httpContext.Request.Headers["host"];
            if (!string.IsNullOrEmpty(requestDomain))
            {
                if (requestDomain.IndexOf(":") > 0)
                {
                    requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"));
                }
            }
            else
            {
                requestDomain = httpContext.Request.Url.Host;
            }

            string requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

            // Match domain and route
            Match domainMatch = domainRegex.Match(requestDomain.ToLower());
            Match pathMatch = pathRegex.Match(requestPath.ToLower());

            // Route data
            RouteData data = null;


            if (domainMatch.Success && pathMatch.Success)
            {
                data = new RouteData(this, RouteHandler);

                // Add defaults first
                if (Defaults != null)
                {
                    foreach (KeyValuePair<string, object> item in Defaults)
                    {
                        data.Values[item.Key] = item.Value;
                    }
                }

                // Iterate matching domain groups
                for (int i = 1; i < domainMatch.Groups.Count; i++)
                {
                    Group group = domainMatch.Groups[i];
                    if (group.Success)
                    {
                        string key = domainRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                            {
                                data.Values[key] = group.Value;
                            }
                        }
                    }
                }

                // Iterate matching path groups
                for (int i = 1; i < pathMatch.Groups.Count; i++)
                {
                    Group group = pathMatch.Groups[i];
                    if (group.Success)
                    {
                        string key = pathRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                            {
                                data.Values[key] = group.Value;
                            }
                            else
                            {
                                if (data.Values[key] == null)
                                {
                                    return null;
                                }
                            }
                        }
                    }
                }
            }

            return data;
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