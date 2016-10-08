using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AddAgent :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

        }


        /// <summary>
        /// 获取权限编号
        /// </summary>
        /// <returns></returns>
        public string getQxID()
        {
            string CheckString = "";
            foreach (Control ct in form1.Controls)
            {
                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                {
                    CheckBox cb = (CheckBox)ct;
                    if (cb.Checked)
                    {
                        CheckString += cb.ID.Replace("c","")+",";
                    }
                }

            }
            return CheckString;
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
              
                Model.AgentInfo agent = new Model.AgentInfo();
                agent.AgentAccount = txtAgentAccount.Text.Trim() ;
                agent.AgentName = txtAgentName.Text;
                agent.AgentPasswd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtAgentPasswd.Text.Trim(), "MD5");
                agent.AgentQQ = txtAgentQQ.Text.Trim();
                agent.AgentEmail = txtAgentEmail.Text.Trim();
                agent.AgentTel = txtAgentTel.Text;
                agent.RegisterTime = DateTime.Now;
                int OperateType=0;
                if (base.Session["manager"] != null )
                {
                    agent.HigherLevel = 0;
                    OperateType = 1;

                }
                else if (base.Session["Agent"] != null )
                {
                    agent.HigherLevel = ((Model.AgentInfo)Session["Agent"]).AgentID;
                    OperateType = 2;
                }


             
                agent.JurisdictionID = getQxID();
               
                agent.RevenueModel =Convert.ToInt32(dropModel.SelectedValue);

                int result = new BLL.AgentInfo().Add(agent);
                if (result > 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存成功')</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存失败')</script>");
                }

            
            }
            catch (Exception ec)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('"+ec.Message+"')</script>");
                throw;
            }
        }
    }
}