﻿#region Using directives

using System.Web.Mvc;
using MvcSiteMapProvider.Internal;
using System.Web;

#endregion

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper class
    /// </summary>
    public class MvcSiteMapHtmlHelper
    {
        /// <summary>
        /// Gets or sets the HTML helper.
        /// </summary>
        /// <value>The HTML helper.</value>
        public HtmlHelper HtmlHelper { get; protected set; }

        /// <summary>
        /// Gets or sets the sitemap provider.
        /// </summary>
        /// <value>The sitemap provider.</value>
        public SiteMapProvider Provider { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcSiteMapHtmlHelper"/> class.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="provider">The sitemap provider.</param>
        public MvcSiteMapHtmlHelper(HtmlHelper htmlHelper, SiteMapProvider provider)
        {
            MvcSiteMapProviderViewEngine.Register();
            HtmlHelper = htmlHelper;
            Provider = provider;
        }

        /// <summary>
        /// Creates the HTML helper for model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public HtmlHelper<TModel> CreateHtmlHelperForModel<TModel>(TModel model)
        {
            return new HtmlHelper<TModel>(HtmlHelper.ViewContext, new ViewDataContainer<TModel>(model));
        }
    }
}
