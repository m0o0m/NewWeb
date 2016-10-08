using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AgShift : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"])) 
            {
                try
                {
                    int agId = Convert.ToInt32(Request.QueryString["id"]);
                    string agAccount = txtAgentAccount.Text.Trim();
                    Model.AgentInfo agentL = new BLL.AgentInfo().GetModel(agAccount);
                    if (agentL == null)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('亲，转移的代理不存在，请重新输入帐号')</script>");
                    }
                    else
                    {
                        Model.AgentInfo agentinfo = new Model.AgentInfo();
                        agentinfo.HigherLevel = agentL.AgentID;
                        agentinfo.AgentID = agId;
                        bool result = new BLL.AgentInfo().UpdateHigherLevel(agentinfo);
                        if (result)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('转移代理成功')</script>");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('转移代理失败')</script>");
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
}