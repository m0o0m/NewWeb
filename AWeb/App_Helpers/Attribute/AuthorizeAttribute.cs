using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace AWeb
{
    /// <summary>Specifies that access to a controller or action method is restricted to users who meet the authorization requirement.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly char[] _splitParameter = new char[]
        {
            ','
        };
        private readonly object _typeId = new object();
        private string _roles;
        private string[] _rolesSplit = new string[0];
        private string _users;
        private string[] _usersSplit = new string[0];
        /// <summary>Gets or sets the user roles that are authorized to access the controller or action method.</summary>
        /// <returns>The user roles that are authorized to access the controller or action method.</returns>
        public string Roles
        {
            get
            {
                return this._roles ?? string.Empty;
            }
            set
            {
                this._roles = value;
                this._rolesSplit = AuthorizeAttribute.SplitString(value);
            }
        }
        /// <summary>Gets the unique identifier for this attribute.</summary>
        /// <returns>The unique identifier for this attribute.</returns>
        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }
        /// <summary>Gets or sets the users that are authorized to access the controller or action method.</summary>
        /// <returns>The users that are authorized to access the controller or action method.</returns>
        public string Users
        {
            get
            {
                return this._users ?? string.Empty;
            }
            set
            {
                this._users = value;
                this._usersSplit = AuthorizeAttribute.SplitString(value);
            }
        }
        /// <summary>When overridden, provides an entry point for custom authorization checks.</summary>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="httpContext" /> parameter is null.</exception>
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            IPrincipal user = httpContext.User;


            var aa = this._rolesSplit.Any(new Func<string, bool>(user.IsInRole));
            var aaaa = user.IsInRole("CPA");

            var aaa = user.Identity.Name;

            var aaaaa = this._usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase);



            return user.Identity.IsAuthenticated && (this._usersSplit.Length <= 0 || this._usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase)) && (this._rolesSplit.Length <= 0 || this._rolesSplit.Any(new Func<string, bool>(user.IsInRole)));
        }
        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context));
        }
        /// <summary>Called when a process requests authorization.</summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext" /> parameter is null.</exception>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("AuthorizeAttribute_CannotUseWithinChildActionCache");
            }
            bool flag = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (flag)
            {
                return;
            }
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), null);
                return;
            }
            this.HandleUnauthorizedRequest(filterContext);
        }
        /// <summary>Processes HTTP requests that fail authorization.</summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
        /// <summary>Called when the caching module requests authorization.</summary>
        /// <returns>A reference to the validation status.</returns>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="httpContext" /> parameter is null.</exception>
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!this.AuthorizeCore(httpContext))
            {
                return HttpValidationStatus.IgnoreThisRequest;
            }
            return HttpValidationStatus.Valid;
        }
        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            IEnumerable<string> source =
                from piece in original.Split(AuthorizeAttribute._splitParameter)
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed;
            return source.ToArray<string>();
        }
    }
}