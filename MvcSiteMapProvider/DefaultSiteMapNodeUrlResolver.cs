﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Extensibility;
using MvcSiteMapProvider.Resources;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Default SiteMapNode URL resolver.
    /// </summary>
    public class DefaultSiteMapNodeUrlResolver
        : ISiteMapNodeUrlResolver
    {
        /// <summary>
        /// UrlHelperCacheKey
        /// </summary>
        private const string UrlHelperCacheKey = "6F0F34DE-2981-454E-888D-28080283EF65";

        /// <summary>
        /// Gets the URL helper.
        /// </summary>
        /// <value>The URL helper.</value>
        protected UrlHelper UrlHelper
        {
            get
            {
                if (HttpContext.Current.Items[UrlHelperCacheKey] == null)
                {
                    RequestContext ctx;
                    if (HttpContext.Current.Handler is MvcHandler)
                        ctx = ((MvcHandler)HttpContext.Current.Handler).RequestContext;
                    else
                        ctx = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());

                    HttpContext.Current.Items[UrlHelperCacheKey] = new UrlHelper(ctx);
                }
                return (UrlHelper)HttpContext.Current.Items[UrlHelperCacheKey];
            }
        }

        #region ISiteMapNodeUrlResolver Members

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="mvcSiteMapNode">The MVC site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        public virtual string ResolveUrl(MvcSiteMapNode mvcSiteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            if (mvcSiteMapNode["url"] != null)
            {
                if (mvcSiteMapNode["url"].StartsWith("~"))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute(mvcSiteMapNode["url"]);
                }
                else
                {
                    return mvcSiteMapNode["url"];
                }
            }

            if (!string.IsNullOrEmpty(mvcSiteMapNode.PreservedRouteParameters))
            {
                foreach (var item in mvcSiteMapNode.PreservedRouteParameters.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var preservedParameterName = item.Trim();
                    routeValues[preservedParameterName] = UrlHelper.RequestContext.RouteData.Values[preservedParameterName];
                }
            }


            string returnValue = null;
            if (!string.IsNullOrEmpty(mvcSiteMapNode.Route))
            {
                returnValue = UrlHelper.RouteUrl(mvcSiteMapNode.Route, new RouteValueDictionary(routeValues));
            }
            else
            {
                returnValue = UrlHelper.Action(action, controller, new RouteValueDictionary(routeValues));
            }

            if (string.IsNullOrEmpty(returnValue))
            {
                throw new UrlResolverException(string.Format(Messages.CouldNotResolve, mvcSiteMapNode.Title, action, controller, mvcSiteMapNode.Route ?? ""));
            }

            return returnValue;
        }

        #endregion
    }
}
