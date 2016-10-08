﻿#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using MvcSiteMapProvider.Extensibility;
using MvcSiteMapProvider.External;
using Telerik.Web.Mvc.Infrastructure.Implementation;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// AuthorizeAttributeAclModule class
    /// </summary>
    public class AuthorizeAttributeAclModule
        : IAclModule
    {
        #region IAclModule Members

        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="controllerTypeResolver">The controller type resolver.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAccessibleToUser(IControllerTypeResolver controllerTypeResolver, DefaultSiteMapProvider provider, HttpContext context, SiteMapNode node)
        {
            // Is security trimming enabled?
            if (!provider.SecurityTrimmingEnabled)
            {
                return true;
            }

            // Is it an external node?
            if (node.Url.StartsWith("http") || node.Url.StartsWith("ftp"))
            {
                return true;
            }

            // Is it a regular node?
            var mvcNode = node as MvcSiteMapNode;
            if (mvcNode == null)
            {
                throw new AclModuleNotSupportedException(
                    Resources.Messages.AclModuleDoesNotSupportRegularSiteMapNodes);
            }

            // Clickable? Always accessible.
            if (mvcNode.Clickable == false)
            {
                return true;
            }

            // Time to delve into the AuthorizeAttribute defined on the node.
            // Let's start by getting all metadata for the controller...
            var controllerType = controllerTypeResolver.ResolveControllerType(mvcNode.Area, mvcNode.Controller);
            if (controllerType == null)
            {
                return false;
            }

            // Find routes for the sitemap node's url
            HttpContextBase httpContext = new HttpContextWrapper(context);
            string originalPath = httpContext.Request.Path;
            var originalRoutes = RouteTable.Routes.GetRouteData(httpContext);
            httpContext.RewritePath(node.Url, true);

            HttpContextBase httpContext2 = new HttpContext2(context);
            RouteData routes = mvcNode.GetRouteData(httpContext2);
            foreach (var routeValue in mvcNode.RouteValues)
            {
                routes.Values[routeValue.Key] = routeValue.Value;
            }
            if (!routes.Route.Equals(originalRoutes.Route) || originalPath != node.Url || mvcNode.Area == String.Empty)
            {
                routes.DataTokens.Remove("area");
                //routes.DataTokens.Remove("Namespaces");
                //routes.Values.Remove("area");
            }
            var requestContext = new RequestContext(httpContext, routes);

            // Create controller context
            var controllerContext = new ControllerContext();
            controllerContext.RequestContext = requestContext;
            try
            {
                controllerContext.Controller = Activator.CreateInstance(controllerType) as ControllerBase;
            }
            catch
            {
            }

            ControllerDescriptor controllerDescriptor = null;
            if (typeof(IController).IsAssignableFrom(controllerType))
            {
                controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
            }
            else if (typeof(IAsyncController).IsAssignableFrom(controllerType))
            {
                controllerDescriptor = new ReflectedAsyncControllerDescriptor(controllerType);
            }

            ActionDescriptor actionDescriptor = null;
            try
            {
                actionDescriptor = controllerDescriptor.FindAction(controllerContext, mvcNode.Action);
            }
            catch
            {
            }
            if (actionDescriptor == null)
            {
                actionDescriptor = controllerDescriptor.GetCanonicalActions().Where(a => a.ActionName == mvcNode.Action).FirstOrDefault();
            }

            // Verify security
            try
            {
                if (actionDescriptor != null)
                {
#if NET35
                    IEnumerable<AuthorizeAttribute> authorizeAttributesToCheck =
                       actionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true).OfType
                           <AuthorizeAttribute>().ToList()
                           .Union(
                               controllerDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), true).OfType
                                   <AuthorizeAttribute>().ToList());
#else
                    IEnumerable<AuthorizeAttribute> authorizeAttributesToCheck =
                        FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor)
                            .Where(f => typeof(AuthorizeAttribute).IsAssignableFrom(f.Instance.GetType()))
                            .Select(f => f.Instance as AuthorizeAttribute);
#endif

                    // Verify all attributes
                    foreach (var authorizeAttribute in authorizeAttributesToCheck)
                    {
                        try
                        {
                            var currentAuthorizationAttributeType = authorizeAttribute.GetType();

                            var builder = new AuthorizeAttributeBuilder();
                            var subclassedAttribute =
                                currentAuthorizationAttributeType == typeof(AuthorizeAttribute) ?
                                   new InternalAuthorize(authorizeAttribute) : // No need to use Reflection.Emit when ASP.NET MVC built-in attribute is used
                                   (IAuthorizeAttribute)builder.Build(currentAuthorizationAttributeType).Invoke(null);

                            // Copy all properties
                            ObjectCopier.Copy(authorizeAttribute, subclassedAttribute);

                            if (!subclassedAttribute.IsAuthorized(controllerContext.HttpContext))
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            // do not allow on exception
                            return false;
                        }
                    }
                }

                // No objection.
                return true;
            }
            finally
            {
                // Restore HttpContext
                httpContext.RewritePath(originalPath, true);
            }
        }

        #endregion
    }
}
