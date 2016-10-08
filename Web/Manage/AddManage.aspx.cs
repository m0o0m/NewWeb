using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AddManage : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    int qx =Convert.ToInt32(((Model.ManagerInfo)Session["manager"]).AdminMasterRight);
                    if (qx == 1)
                    {

                        Model.ManagerInfo manager = new Model.ManagerInfo();
                        manager.AdminAccount = txtAdminAccount.Text.Trim();
                        manager.AdminName = txtAdminName.Text.Trim();
                        manager.AdminPasswd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtAdminPasswd.Text.Trim(), "MD5");
                        manager.OperDate = DateTime.Now;
                        int result = new BLL.ManagerInfo().Add(manager);
                        if (result > 0)
                        {

                            AddLog(1, 1, DateTime.Now, "添加帐号为:" + txtAdminAccount.Text + "；昵称为：" + txtAdminName.Text + " 的管理员");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('添加成功！');</script>");

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('添加失败！');</script>");
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('当前用户没权限操作');</script>");
                    }


                }
                
            }
            catch (Exception ec)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('"+ec.Message+"');</script>");
                throw;
            }
        }
    }
}