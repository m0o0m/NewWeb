#region Using

using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.Ajax.Utilities;
using GL.Data.BLL;
using GL.Data.Model;
using System.Collections.Generic;
using System.Linq.Expressions;


#endregion

namespace MWeb
{
    public static class HtmlHelperExtensions
    {
        private static string _displayVersion;

        /// <summary>
        ///     Retrieves a non-HTML encoded string containing the assembly version as a formatted string.
        ///     <para>If a project name is specified in the application configuration settings it will be prefixed to this value.</para>
        ///     <para>
        ///         e.g.
        ///         <code>1.0 (build 100)</code>
        ///     </para>
        ///     <para>
        ///         e.g.
        ///         <code>ProjectName 1.0 (build 100)</code>
        ///     </para>
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
    

        /// <summary>
        ///     Compares the requested route with the given <paramref name="value" /> value, if a match is found the
        ///     <paramref name="attribute" /> value is returned.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value">The action value to compare to the requested route action.</param>
        /// <param name="attribute">The attribute value to return in the current action matches the given action value.</param>
        /// <returns>A HtmlString containing the given attribute value; otherwise an empty string.</returns>
        public static IHtmlString RouteIf(this HtmlHelper helper, string value, string attribute)
        {
            var currentController =
                (helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();
            var currentAction =
                (helper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty).ToString().UnDash();

            var hasController = value.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            return hasAction || hasController ? new HtmlString(attribute) : new HtmlString(string.Empty);
        }

        public static IHtmlString RouteIf(this HtmlHelper helper, string value, string attribute,bool isCate) {
            if (isCate) {
                string para = value.Split('/')[1];
                var param = ((System.Web.HttpRequestWrapper)((System.Web.HttpContextWrapper)helper.ViewContext.RequestContext.HttpContext).Request).QueryString["tab"];
                if (para == param)
                {
                   return  RouteIf(helper, value.Split('/')[0], attribute);
                }
                else {
                    return new HtmlString(string.Empty);
                }


          
            }
            else {
                return RouteIf(helper, value, attribute);
            }
            //
        }

        /// <summary>
        ///     Renders the specified partial view with the parent's view data and model if the given setting entry is found and
        ///     represents the equivalent of true.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="appSetting">The key value of the entry point to look for.</param>


        /// <summary>
        ///     Renders the specified partial view with the parent's view data and model if the given setting entry is found and
        ///     represents the equivalent of true.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="condition">The boolean value that determines if the partial view should be rendered.</param>
        public static void RenderPartialIf(this HtmlHelper htmlHelper, string partialViewName, bool condition)
        {
            if (!condition)
                return;

            htmlHelper.RenderPartial(partialViewName);
        }

        /// <summary>
        ///     Retrieves a non-HTML encoded string containing the assembly version and the application copyright as a formatted
        ///     string.
        ///     <para>If a company name is specified in the application configuration settings it will be suffixed to this value.</para>
        ///     <para>
        ///         e.g.
        ///         <code>1.0 (build 100) © 2015</code>
        ///     </para>
        ///     <para>
        ///         e.g.
        ///         <code>1.0 (build 100) © 2015 CompanyName</code>
        ///     </para>
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
    

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that utilizes bootstrap markup and styling.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="alertType">The alert type styling rule to apply to the summary element.</param>
        /// <param name="heading">The optional value for the heading of the summary element.</param>
        /// <returns></returns>
        public static HtmlString ValidationBootstrap(this HtmlHelper htmlHelper, string alertType = "danger",
            string heading = "")
        {
            if (htmlHelper.ViewData.ModelState.IsValid)
                return new HtmlString(string.Empty);

            var sb = new StringBuilder();

            sb.AppendFormat("<div class=\"alert alert-{0} alert-block\">", alertType);
            sb.Append("<button class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>");

            if (!heading.IsNullOrWhiteSpace())
            {
                sb.AppendFormat("<h4 class=\"alert-heading\">{0}</h4>", heading);
            }

            sb.Append(htmlHelper.ValidationSummary());
            sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }

        public static MvcHtmlString GetAllAgentGroupList(this HtmlHelper htmlHelper,string name, int selectedValue)
        {
            List<AgentInfoGroup> group = AgentInfoBLL.GetAgentGroupList();
          
            string optgroup = "";
            string option = "";
            string alloptgroup = "";
            for (int i=0;i< group.Count;i++)
            {
                AgentInfoGroup item = group[i];
               
                if (item.ID == -1)
                {
                    if (option != "")
                    {
                        optgroup = optgroup.Replace("@replace", option);
                        alloptgroup += optgroup;
                    }
                    else {
                        optgroup = optgroup.Replace("@replace", "");
                        alloptgroup += optgroup;
                    }
                    optgroup = " <optgroup label = '" + item.AgentName + "' >@replace</ optgroup >";
                    option = "";
                }
                else {
                    if (selectedValue == item.ID)
                    {
                        option += " <option selected='selected' value = '" + item.ID + "' > " + item.AgentName + " </option >";
                    }
                    else {
                        option += " <option value = '" + item.ID + "' > " + item.AgentName + " </option >";
                    }
                }
            }
            optgroup=  optgroup.Replace("@replace", option);
            alloptgroup += optgroup;

            alloptgroup = "<select style = 'width: 100 %;' class='select2' id="+name+" name="+name+">"+
                                "<option value='0'>&nbsp;&nbsp;&nbsp;所有渠道</option>" +
                                alloptgroup+
                          "</select>";
            return new MvcHtmlString(alloptgroup);
        }





        public static MvcHtmlString GetMasterLevel(this HtmlHelper htmlHelper, string name, int selectedValue)
        {

            IEnumerable<Role> group = RoleBLL.GetMasterLevelModels();

            string optgroup = "";
            string option = "";
            string alloptgroup = "";

            foreach (Role item in group)
            {


                if (selectedValue == item.ID)
                {
                    option += " <option selected='selected' value = '" + item.ID + "' > " + item.Account + " </option >";
                }
                else
                {
                    option += " <option value = '" + item.ID + "' > " + item.Account + " </option >";
                }
            }


            alloptgroup += option;

            alloptgroup = "<select style = 'width: 100 %;' class='select2' id=" + name + " name=" + name + ">" +
                                "<option value='-1'>&nbsp;&nbsp;&nbsp;所有用户</option>" +
                                alloptgroup +
                          "</select>";

            return new MvcHtmlString(alloptgroup);
        }



        public static MvcHtmlString GetMasterOper(this HtmlHelper htmlHelper, string name, string selectedValue)
        {

            IEnumerable<CommonIDName> group = ScaleRecordBLL.GetMasterOper();

            string optgroup = "";
            string option = "";
            string alloptgroup = "";


            //if (selectedValue == "登录")
            //{
            //    option += " <option selected='selected' value = '登录' > 登录</option >";
            //}
            //else
            //{
            //    option += " <option value = '登录' > 登录</option >";
            //}
            //if (selectedValue == "登出")
            //{
            //    option += " <option selected='selected' value = '登出' > 登出</option >";
            //}
            //else
            //{
            //    option += " <option value = '登出' > 登出</option >";
            //}



            foreach (CommonIDName item in group)
            {


                if (selectedValue == item.Name)
                {
                    option += " <option selected='selected' value = '" + item.Name + "' > " + item.Name + " </option >";
                }
                else
                {
                    option += " <option value = '" + item.Name + "' > " + item.Name + " </option >";
                }
            }

        



            alloptgroup += option;

            alloptgroup = "<select style = 'width: 100 %;' class='select2' id=" + name + " name=" + name + ">" +
                                "<option value=''>所有操作</option>" +
                               
                                
                                alloptgroup +
                          "</select>";

            return new MvcHtmlString(alloptgroup);
        }



        public static MvcHtmlString GetAllPhoneBoardList(this HtmlHelper htmlHelper, string name, string selectedValue)
        {
            IEnumerable<CommonIDName> group = BaseDataBLL.GetPhoneBoard();

            string option = "";
            string alloptgroup = "";

            foreach (CommonIDName item in group)
            {
             

                if (selectedValue == item.Name)
                {
                    option += " <option selected='selected' value = '" + item.Name + "' > " + item.Name + " </option >";
                }
                else
                {
                    option += " <option value = '" + item.Name + "' > " + item.Name + " </option >";
                }
            }


        
            alloptgroup += option;

            alloptgroup = "<select style = 'width: 100 %;' class='select2' id=" + name + " name=" + name + ">" +
                                "<option value='all_brand'>&nbsp;&nbsp;&nbsp;所有品牌</option>" +
                                alloptgroup +
                          "</select>";
            return new MvcHtmlString(alloptgroup);
        }


        public static MvcHtmlString GetAllUserGroupList(this HtmlHelper htmlHelper, string name, string selectedValue)
        {
            List<UEUser> group = UserEmailBLL.GetUserGroupList();

            string option = "";
            string alloptgroup = "";
            for (int i = 0; i < group.Count; i++)
            {
                UEUser item = group[i];

                if (selectedValue == item.UserName)
                {
                    option += " <option selected='selected' value = '" + item.UserName + "' > " + item.NickName + " </option >";
                }
                else {
                    option += " <option value = '" + item.UserName + "' > " + item.NickName + " </option >";
                }
            }
            alloptgroup += option;

            alloptgroup = "<select style = 'width: 100 %;' class='select2' id=" + name + " name=" + name + ">" +
                                "<option value=''>&nbsp;&nbsp;&nbsp;所有用户</option>" +
                                alloptgroup +
                          "</select>";
            return new MvcHtmlString(alloptgroup);
        }
        public static MvcHtmlString GetAllItemGroupList(this HtmlHelper htmlHelper, string name, int selectedValue)
        {
            List<ItemGroup> group = ExpRecordBLL.GetItemGroupList();

            string option = "";
            string alloptgroup = "";
            for (int i = 0; i < group.Count; i++)
            {
                ItemGroup item = group[i];

                if (selectedValue == item.ItemID)
                {
                    option += " <option selected='selected' value = '" + item.ItemID + "' > " + item.ItemName + " </option >";
                }
                else {
                    option += " <option value = '" + item.ItemID + "' > " + item.ItemName + " </option >";
                }
            }
            alloptgroup += option;

            alloptgroup = "<select style = 'width: 100 %;' class='select2' id=" + name + " name=" + name + ">" +
                                "<option value='0'>&nbsp;&nbsp;&nbsp;所有物品</option>" +
                                alloptgroup +
                          "</select>";
            return new MvcHtmlString(alloptgroup);
        }

        
        public static MvcHtmlString GetOptionsSelectedAttr(this HtmlHelper htmlHelper, int value, int option)
        {
            if (option == value)
            {
                return new MvcHtmlString(" selected='selected'");
            }
            else {
                return new MvcHtmlString(" ");
            }
        }

        public static MvcHtmlString GetOptionsSelectedAttr(this HtmlHelper htmlHelper, string value, string option)
        {
            if (option == value)
            {
                return new MvcHtmlString(" selected='selected'");
            }
            else
            {
                return new MvcHtmlString(" ");
            }
        }


        public static MvcHtmlString GetMVCstring(this HtmlHelper htmlHelper, int value, string option,string html)
        {
            string s = "," + option + ",";

            if (s.Contains(","+ value + ","))
            {
                return new MvcHtmlString(html);
            }
            else
            {
                return new MvcHtmlString(" ");
            }
        }

        public static IHtmlString PartialBarAndLine(this HtmlHelper helper, BarAndLine barAndLine)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(helper, "~/Views/Shared/BarAndLine.cshtml", barAndLine);
        }

        public static IHtmlString PartialSearch(this HtmlHelper helper, Search search)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(helper, "~/Views/Shared/Search.cshtml", search);
        }


        public static IHtmlString PartialTable(this HtmlHelper helper, Table table)
        {


            return System.Web.Mvc.Html.PartialExtensions.Partial(helper, "~/Views/Shared/Table1.cshtml", table);
        }


        public static IHtmlString PartialForm(this HtmlHelper helper, Form form)
        {
            return System.Web.Mvc.Html.PartialExtensions.Partial(helper, "~/Views/Shared/Form.cshtml", form);
        }


    }
}