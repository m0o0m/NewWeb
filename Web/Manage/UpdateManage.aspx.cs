using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class UpdateManage :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

        
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                Model.ManagerInfo manager = (Model.ManagerInfo)Session["manager"];
        
                txtAdminAccount.Text = manager.AdminAccount;
                txtAdminName.Text = manager.AdminName;
                txtAdminPasswd.Text = manager.AdminPasswd;
                hiddenPw.Value = manager.AdminPasswd;
            }
            else
            {
                int sid = Convert.ToInt32(Request.QueryString["id"]);
                Model.ManagerInfo managers = new BLL.ManagerInfo().GetModel(sid);
                txtAdminAccount.Text = managers.AdminAccount;
                txtAdminName.Text = managers.AdminName;
                txtAdminPasswd.Text = managers.AdminPasswd;
                hiddenPw.Value = managers.AdminPasswd;

                

            }
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                Model.ManagerInfo man = new Model.ManagerInfo();
               
                if (txtAdminPasswd.Text.Trim() != "")
                {
                    man.AdminPasswd =System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtAdminPasswd.Text.Trim(), "MD5");
                }
                else
                {
                    man.AdminPasswd = hiddenPw.Value;
                }
                man.AdminAccount = txtAdminAccount.Text.Trim();
                man.AdminName = txtAdminName.Text.Trim();
               

                if (string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Model.ManagerInfo manager = (Model.ManagerInfo)Session["manager"];
                    man.AdminID = manager.AdminID;
                  
                }
                else
                {
                    man.AdminID = Convert.ToInt32(Request.QueryString["id"]);

                }
                bool result = new BLL.ManagerInfo().Update(man);
                if (result)
                {
                    AddLog(1,1,DateTime.Now,"修改了ID为："+man.AdminID+" 的管理员信息");
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存成功')</script>");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存失败')</script>");
                }
            }
            catch (Exception ec)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert("+ec.Message+")</script>");
                
                throw;
            }
        }
    }
}