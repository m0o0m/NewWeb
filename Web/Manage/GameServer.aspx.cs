using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class GameServer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                int sid = Convert.ToInt32(Request.QueryString["sid"]);
                Model.GameServerInfo gs = new BLL.GameServerInfo().GetModel(sid);
                txtServerName.Text = gs.ServerName;
                txtMaxUserCount.Text =Convert.ToString(gs.MaxUserCount);
                txtSortID.Text = Convert.ToString(gs.SortID);



            }
        }

        protected void btnSubServer_Click(object sender, EventArgs e)
        {
            try
            {
                int kid = Convert.ToInt32(Request.QueryString["kid"]);
                int tid = Convert.ToInt32(Request.QueryString["tid"]);
                Model.GameServerInfo gserver = new Model.GameServerInfo();
                gserver.KindID = kid;
                gserver.TypeID = tid;
                gserver.ServerName = txtServerName.Text.Trim();
                gserver.SortID =Convert.ToInt32( txtSortID.Text.Trim());
                gserver.MaxUserCount = Convert.ToInt32(txtMaxUserCount.Text.Trim());
                gserver.Enable = true;
                int result = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
                {
                    int sid = Convert.ToInt32(Request.QueryString["sid"]);
                    gserver.ServerID=sid;
                    result = new BLL.GameServerInfo().Update(gserver);
                }
                else
                {
                    result = new BLL.GameServerInfo().Add(gserver);
                }
                if (result > 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存成功')</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存失败')</script>");
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}