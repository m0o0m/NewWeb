using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AddCustomer : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                Model.CustomerInfo cust = new Model.CustomerInfo();
                cust.CustomerAccount = txtCustomerAccount.Text.Trim();
                cust.CustomerPwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtCustomerAccount.Text.Trim(), "MD5");
                cust.CustomerName = txtCustomerName.Text.Trim();
                cust.CustomerState = 0;
                cust.CreateDate = DateTime.Now;
                int result = new BLL.CustomerInfo().Add(cust);
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

                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert("+ec.Message+")</script>");
                throw;
            }
        }
    }
}