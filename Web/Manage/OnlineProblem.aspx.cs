using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class OnlineProblem : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CustomerBind();
        }

        private void CustomerBind()
        {
            AspNetPager1.RecordCount = new BLL.CustomerServCenter().GetRecordCount("  c.GUUserID=g.GUUserID and CSCSubId=0 ");
            Repeater1.DataSource = new BLL.CustomerServCenter().GetListByPage(AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, AspNetPager1.RecordCount);
            Repeater1.DataBind();
        }




        public string getzh(string tmp, string flag)
        {
            string value = string.Empty;
            switch (flag)
            {
                case"1":
                    DateTime dt = new DateTime(1970, 1, 1);
                    DateTime date = new DateTime(dt.Ticks + (long.Parse(tmp) * 10000000));
                    value = date.ToString("yyyy-MM-dd HH:mm:ss");

                    break;

                case"2":
                    switch (tmp)
	                   {
                              case"1":
                               value = "程序错误";
                               break;
                              case "2":
                               value = "改进意见";
                               break;
                              case "3":
                               value = "账号问题";
                               break;
                              case "4":
                               value = "支付问题";
                               break;
                              case "5":
                               value = "活动问题";
                               break;
                              //case "4":
                              // value = "其他问题";
                              // break;                        
                              //<option value="1">程序错误</option>
                        //<option value="2">改进意见</option>
                        //<option value="3">账号问题</option>
                        //<option value="4">支付问题</option>
                        //<option value="5">活动问题</option>

	                         }
                    break;
                case "3":
                    switch (tmp)
                    {
                        case "1":
                            value = "未处理";
                            break;
                        case "2":
                            value = "已回复";
                            break;
                        case "3":
                            value = "已解决";
                            break;
                    }
                    break;
            }

            return value;
        }


        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DateTime zhtime(long time)
        {
            DateTime dt = new DateTime(1970, 1, 1);

            DateTime date = new DateTime(dt.Ticks + (time * 10000000));

            return date;

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Search":
                    object[] arg = e.CommandArgument.ToString().Split(',');
                    int svid = Convert.ToInt32(arg[0]);
                    int smid = Convert.ToInt32(arg[1]);

                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('对话查看','DialogueInfo.aspx?vid=" + svid + "&mid=" + smid + "',800,600)</script>)");
                    break;

                case"Del":
                    int dmid = Convert.ToInt32(e.CommandArgument);
                    new BLL.CustomerServCenter().Delete(" CSCSubId =" + dmid + "");
                    new BLL.CustomerServCenter().Delete(" CSCMainID=" + dmid + "");
                    CustomerBind();
                    break;
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            CustomerBind();
        }
    }
}