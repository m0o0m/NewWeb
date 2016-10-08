using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class UpdateAgent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int agId = Convert.ToInt32(Request.QueryString["id"]);
               Model.AgentInfo agent   = new BLL.AgentInfo().GetModel(agId);
              txtAgentAccount.Text                   =     agent.AgentAccount;                 
              txtAgentName.Text                   =     agent.AgentName ;                        
              txtAgentPasswd.Text                 =     agent.AgentPasswd ;                      
               txtAgentQQ.Text                    =    agent.AgentQQ ;                          
              txtAgentTel.Text                   =     agent.AgentTel;                          
              txtAgentEmail.Text                     =     agent.AgentEmail;                        
              dropModel.SelectedValue      =  Convert.ToString(agent.RevenueModel) ;                     
                string [] s=agent.JurisdictionID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < s.Length; i++)
			   {
			            if(s[i]=="1"){
                            c1.Checked=true;
                        }else if(s[i]=="2"){
                        c2.Checked=true;
                        }else if(s[i]=="3"){
                        c3.Checked=true;
                        }
			    }

            }
            
        }

         
     

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int agId = Convert.ToInt32(Request.QueryString["id"]);
                    Model.AgentInfo agent = new Model.AgentInfo();
                    agent.AgentAccount = txtAgentAccount.Text.Trim();
                    agent.AgentName = txtAgentName.Text.Trim();
                    if (txtAgentPasswd.Text.Trim() != "")
                    {
                        agent.AgentPasswd = txtAgentPasswd.Text.Trim();
                    }
                    else
                    {
                        Model.AgentInfo ag = new BLL.AgentInfo().GetModel(agId);
                        agent.AgentPasswd = ag.AgentPasswd;
                    }
                    agent.AgentQQ = txtAgentQQ.Text.Trim();
                    agent.AgentTel = txtAgentTel.Text.Trim();
                    agent.AgentID = agId;
                    bool result = new BLL.AgentInfo().UpdateAg(agent);
                    if (result)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存成功')</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存失败')</script>");
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