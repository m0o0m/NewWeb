using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Properties;
namespace AWeb
{

    public static class LiExtensions
    {

        private static readonly char[] _splitParameter = new char[]
        {
                    ','
        };

        /// <summary>
        ///     Compares the requested route with the given <paramref name="value" /> value, if a match is found the
        ///     <paramref name="attribute" /> value is returned.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value">The action value to compare to the requested route action.</param>
        /// <param name="attribute">The attribute value to return in the current action matches the given action value.</param>
        /// <returns>A HtmlString containing the given attribute value; otherwise an empty string.</returns>
        public static MvcHtmlString Li(this HtmlHelper htmlHelper, string value, string attribute, string url, string title, string roles = "", string users = "")
        {
            StringBuilder stringBuilder = new StringBuilder();

            var currentController = (htmlHelper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();
            var currentAction = (htmlHelper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty).ToString().UnDash();

            var hasController = value.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            string[] rolesSplit = SplitString(roles);
            string[] usersSplit = SplitString(users);
            IPrincipal user = htmlHelper.ViewContext.HttpContext.User;


            //var aa = rolesSplit.Any(new Func<string, bool>(user.IsInRole));
            //var aaaa = user.IsInRole("CPA");

            //var aaa = user.Identity.Name;

            //var aaaaa = usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase);


            if ( user.Identity.IsAuthenticated && (usersSplit.Length <= 0 || usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase)) && (rolesSplit.Length <= 0 || rolesSplit.Any(new Func<string, bool>(user.IsInRole))))
            {
                stringBuilder.AppendFormat("<li class=\"{0}\"><a href=\"{1}\">{2}</a></li>", hasAction || hasController ? new HtmlString(attribute) : new HtmlString(string.Empty), url, title);
            }
            return MvcHtmlString.Create(stringBuilder.ToString());

        }



        public static MvcHtmlString NoView(this HtmlHelper htmlHelper, string roles = "")
        {
            StringBuilder stringBuilder = new StringBuilder();

         
            string[] rolesSplit = SplitString(roles);
          
            IPrincipal user = htmlHelper.ViewContext.HttpContext.User;


            //var aa = rolesSplit.Any(new Func<string, bool>(user.IsInRole));
            //var aaaa = user.IsInRole("CPA");

            //var aaa = user.Identity.Name;

            //var aaaaa = usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase);


            if ((rolesSplit.Length <= 0 || rolesSplit.Any(new Func<string, bool>(user.IsInRole))))
            {
                stringBuilder.AppendFormat("display:none");
            }
            return MvcHtmlString.Create(stringBuilder.ToString());

        }



     
        /// <summary>
        ///     Compares the requested route with the given <paramref name="value" /> value, if a match is found the
        ///     <paramref name="attribute" /> value is returned.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value">The action value to compare to the requested route action.</param>
        /// <param name="attribute">The attribute value to return in the current action matches the given action value.</param>
        /// <returns>A HtmlString containing the given attribute value; otherwise an empty string.</returns>
        public static MvcContainer LiContainer(this HtmlHelper htmlHelper, string value, string attribute)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var currentController = (htmlHelper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();
            var currentAction = (htmlHelper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty).ToString().UnDash();
            var hasController = value.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            stringBuilder.AppendFormat("<li class=\"{0}\">", hasAction || hasController ? new HtmlString(attribute) : new HtmlString(string.Empty));
            htmlHelper.ViewContext.Writer.Write(stringBuilder);
            MvcContainer result = new MvcContainer(htmlHelper.ViewContext);

            return result;


        }
        public static void EndForm(this HtmlHelper htmlHelper)
        {
            EndForm(htmlHelper.ViewContext);
        }
        internal static void EndForm(ViewContext viewContext)
        {
            viewContext.Writer.Write("</li>");
            viewContext.OutputClientValidation();
            viewContext.FormContext = null;
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            IEnumerable<string> source =
                from piece in original.Split(_splitParameter)
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed;
            return source.ToArray<string>();
        }

    }


    public class MvcContainer : IDisposable
    {
        private readonly ViewContext _viewContext;
        private bool _disposed;
        /// <summary>Initializes a new instance of the <see cref="T:System.Web.Mvc.Html.MvcForm" /> class using the specified HTTP response object.</summary>
        /// <param name="httpResponse">The HTTP response object.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="httpResponse " />parameter is null.</exception>
        [Obsolete("This constructor is obsolete, because its functionality has been moved to MvcForm(ViewContext) now.", true)]
        public MvcContainer(HttpResponseBase httpResponse)
        {
            throw new InvalidOperationException("MvcMvcContainer_ConstructorObsolete");
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Web.Mvc.Html.MvcForm" /> class using the specified view context.</summary>
        /// <param name="viewContext">An object that encapsulates the information that is required in order to render a view.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="viewContext" /> parameter is null.</exception>
        public MvcContainer(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            this._viewContext = viewContext;
            this._viewContext.FormContext = new FormContext();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>Releases unmanaged and, optionally, managed resources used by the current instance of the <see cref="T:System.Web.Mvc.Html.MvcForm" /> class.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;
                LiExtensions.EndForm(_viewContext);
            }
        }
        /// <summary>Ends the form and disposes of all form resources.</summary>
        public void EndForm()
        {
            this.Dispose(true);
        }
    }
}