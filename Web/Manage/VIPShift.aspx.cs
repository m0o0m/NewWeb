using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class VIPShift : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int vipId = Convert.ToInt32(Request.QueryString["id"]);
                    Model.AgentInfo ag = new BLL.AgentInfo().GetModel(txtAgentAccount.Text.Trim());
                    if (ag == null)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('亲，转移的代理不存在，请重新输入帐号')</script>");
                    }
                    else
                    {
                        Model.GameUserInfo gu = new Model.GameUserInfo();
                        gu.GUUserID = vipId;
                        gu.GUParentUserID = ag.AgentID;
                        bool result = new BLL.GameUserInfo().VIPShift(gu);
                        if (result)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('会员转移成功')</script>");

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('会员转移失败')</script>");

                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }
    }
}