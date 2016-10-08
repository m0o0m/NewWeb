using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class Management :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            AspNetPager1.RecordCount = new BLL.ManagerInfo().GetRecordCount("");
            ManageBind();
        }

        public void ManageBind()
        {
            Model.ManagerInfo manager = ((Model.ManagerInfo)Session["manager"]);
            if (manager.AdminMasterRight != 1)
            {
                Repeater1.DataSource = new BLL.ManagerInfo().GetListByPage(" AdminID='" +manager.AdminID+ "'", " OperDate desc ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.DataSource = new BLL.ManagerInfo().GetListByPage("", " OperDate desc ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
                Repeater1.DataBind();
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ManageBind();
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Del":
                    int sid = ((Model.ManagerInfo)Session["manager"]).AdminID;
                    int id = Convert.ToInt32(e.CommandArgument);
                    if (id == sid)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('当前用户不能删除，只能做修改操作')</script>");
                    }
                    else
                    {

                        new BLL.ManagerInfo().Delete(id);
                        AddLog(1, 1, DateTime.Now, "删除了管理员ID为："+id+"的管理员");
                        ManageBind();
                    }

                    break;
                case"Update":
                    Model.ManagerInfo manager = (Model.ManagerInfo)Session["manager"];
                    if (manager.AdminMasterRight == 1)
                    {
                        int uid = Convert.ToInt32(e.CommandArgument);
                        Response.Redirect("UpdateManage.aspx?id=" + uid + "");
                    }
                    else
                    {
                        Response.Redirect("UpdateManage.aspx");
                    }

                    break;
            }
        }
    }
}