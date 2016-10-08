#region Using directives

using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;

#endregion

namespace MvcSiteMapProvider.Filters
{
    /// <summary>
    /// SiteMapTitle attribute
    /// Can be used for setting sitemap title on an action method.
    /// Credits go to Kenny Eliasson - http://mvcsitemap.codeplex.com/Thread/View.aspx?ThreadId=67056
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SiteMapTitleAttribute
        : ActionFilterAttribute
    {
        private const string OriginalTitleKey = "F6409AA4-2870-407D-BC2A-31AC3A64F87A";

        /// <summary>
        /// Gets or sets the original title.
        /// </summary>
        /// <value>The original title.</value>
        protected string OriginalTitle
        {
            get
            {
                if (HttpContext.Current.Items[OriginalTitleKey] != null)
                {
                    return HttpContext.Current.Items[OriginalTitleKey].ToString();
                }
                return "";
            }
            set { HttpContext.Current.Items[OriginalTitleKey] = value; }
        }

        /// <summary>
        /// Property name of ViewData to look in
        /// </summary>
        protected readonly string PropertyName;

        /// <summary>
        /// Gets or sets the site map title target.
        /// </summary>
        /// <value>The site map title target.</value>
        public SiteMapTitleTarget SiteMapTitleTarget { get; set; }

        /// <summary>
        /// Creates a new SiteMapTitleAttribute instance.
        /// </summary>
        /// <param name="propertyName">Property in ViewData used as the SiteMap.CurrentNode.Title</param>
        public SiteMapTitleAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
            this.SiteMapTitleTarget = SiteMapTitleTarget.CurrentNode;
        }

        /// <summary>
        /// Called by the MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                var result = (ViewResult)filterContext.Result;

                var target = ResolveTarget(result.ViewData.Model, PropertyName);
                if (target == null)
                {
                    target = ResolveTarget(result.ViewData, PropertyName);
                }

                if (target != null)
                {
                    if (SiteMap.CurrentNode != null)
                    {
                        if (SiteMapTitleTarget == SiteMapTitleTarget.ParentNode && SiteMap.CurrentNode.ParentNode != null)
                        {
                            OriginalTitle = SiteMap.CurrentNode.ParentNode.Title;
                            SiteMap.CurrentNode.ParentNode.Title = target.ToString();
                        }
                        else
                        {
                            OriginalTitle = SiteMap.CurrentNode.Title;
                            SiteMap.CurrentNode.Title = target.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called by the MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                if (!string.IsNullOrEmpty(OriginalTitle))
                {
                    if (SiteMap.CurrentNode != null)
                    {
                        if (SiteMapTitleTarget == SiteMapTitleTarget.ParentNode && SiteMap.CurrentNode.ParentNode != null)
                        {
                            SiteMap.CurrentNode.ParentNode.Title = OriginalTitle;
                        }
                        else
                        {
                            SiteMap.CurrentNode.Title = OriginalTitle;
                        }
                        OriginalTitle = "";
                    }
                }
            }
        }

        /// <summary>
        /// Resolve target
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="expression">Target expression</param>
        /// <returns>
        /// A target represented as a <see cref="object"/> instance 
        /// </returns>
        internal static object ResolveTarget(object target, string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return target;

            var left = expression;
            var right = "";

            if (expression.IndexOf(".") > -1)
            {
                left = expression.Substring(0, expression.IndexOf("."));
                right = expression.Substring(expression.IndexOf(".") + 1);
            }
            object result;
            try
            {
                if (target is ViewDataDictionary)
                {
                    result = ((ViewDataDictionary)target).Eval(expression);
                }
                else if (target is IDictionary)
                {
                    result = result = ResolveTarget(
                        ((IDictionary)target)[left],
                        right);
                }
                else if (target is IList)
                {
                    result = result = ResolveTarget(
                        ((IList)target)[int.Parse(left)],
                        right);
                }
                else
                {
                    var propertyInfo = target.GetType().GetProperty(left);
                    result = ResolveTarget(
                        propertyInfo.GetValue(target, null),
                        right);
                }
            }
            catch (NullReferenceException)
            {
                result = null;
            }

            return result;
        }
    }

}