using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AnnouncementInfo : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            AspNetPager1.RecordCount = new BLL.Announcement().GetRecordCount("");

            AnnBind();
        }


        public string GetTypeName(string tmp)
        {
            string value = string.Empty;
            switch (tmp)
            {
                case"0":
                    value = "游戏普通";
                    break;
                case "1":
                    value = "游戏左栏置顶";
                    break;
                case "2":
                    value = "游戏滚动";
                    break;
                case "3":
                    value = "代理公告";
                    break;
             
            }
            return value;
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        public void AnnBind()
        {
            Repeater1.DataSource = new BLL.Announcement().GetListByPage("", " ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
            Repeater1.DataBind();

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case"Del":
                    int did = Convert.ToInt32(e.CommandArgument);
                    new BLL.Announcement().Delete(did);
                    AnnBind();
                    break;

                case"Update":
                    int uid=Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("UpdateAnn.aspx?id=" + uid + "");
                    break;
                        
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {

        }
    }
}