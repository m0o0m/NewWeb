using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class login : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
    
        }
        protected override void OnInit(EventArgs O)
        {

        }
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            Session.Clear();
            string username = Request.Form["username"].ToString();
            string passwd = Request.Form["password"].ToString();

            string md5pass = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(passwd, "MD5");
            try
            {
                if (dropModel.SelectedValue == "0")
                {
                    Model.ManagerInfo manager = new BLL.ManagerInfo().GetModel(username);
                    if (manager == null || string.IsNullOrEmpty(manager.AdminAccount))
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('帐号不存在,请重新输入')</script>");
                    }
                    else if (md5pass != manager.AdminPasswd)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('密码不正确,请重新输入')</script>");
                    }
                    else
                    {
                        Session["manager"] = manager;
                       
                        AddLog(manager.AdminID, 1, DateTime.Now, "在Ip为:" + Request.UserHostAddress + " 的PC端登录");
                        Response.Redirect("Manage\\Management.aspx");

                    }
                }
                else if(dropModel.SelectedValue=="1")
                {
                    Model.AgentInfo agent = new BLL.AgentInfo().GetModel(username);
                    if (agent == null || string.IsNullOrEmpty(agent.AgentAccount))
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('帐号不存在,请重新输入')</script>");
                    }
                    else if (md5pass != agent.AgentPasswd)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('密码不正确,请重新输入')</script>");
                    }
                    else
                    {
                        Session["Agent"] = agent;
                      
                        Model.AgentInfo agents = new Model.AgentInfo();
                        agents.AgentID = agent.AgentID;
                        agents.LoginIP = Request.UserHostAddress;
                        agents.LoginTime = DateTime.Now;
                        agents.OnlineState = 1;
                        new BLL.AgentInfo().UpdateIPonTime(agents);

                        AddLog(agent.AgentID, 2, DateTime.Now, "在IP为：" + Request.UserHostAddress + " 的PC端登录");
                        Response.Redirect("Manage\\AgentInfo.aspx");

                    }


                }
                else if (dropModel.SelectedValue == "2")
                {
                    Model.CustomerInfo cust = new BLL.CustomerInfo().GetModel(username);
                    if (cust == null || string.IsNullOrEmpty(cust.CustomerAccount))
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('帐号不存在,请重新输入')</script>");
                    }
                    else if (md5pass != cust.CustomerPwd)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('密码不正确,请重新输入')</script>");
                    }
                    else
                    {
                        Session["Cust"] = cust;
                        Model.CustomerInfo Customer = new Model.CustomerInfo();
                        Customer.CustomerState = 1;
                        Customer.CustomerAccount = cust.CustomerAccount;
                        new BLL.CustomerInfo().UpdateState(Customer);
                        Response.Redirect("Manage\\OnlineProblem.aspx");

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