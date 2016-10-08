using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class UpdateCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int custId=Convert.ToInt32(Request.QueryString["id"]);
                Model.CustomerInfo cust = new BLL.CustomerInfo().GetModel(custId);
               

                txtCustomerAccount.Text = cust.CustomerAccount;
                txtCustomerName.Text = cust.CustomerName;
                txtCustomerPwd.Text = cust.CustomerPwd;
            }
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                Model.CustomerInfo cust = new Model.CustomerInfo();
                int custId = Convert.ToInt32(Request.QueryString["id"]);
                cust.CustomerAccount = txtCustomerAccount.Text;
                Model.CustomerInfo custs = new BLL.CustomerInfo().GetModel(custId);
                if (txtCustomerPwd.Text.Trim() != "")
                {
                    cust.CustomerPwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtCustomerPwd.Text, "MD5");
                }
                else
                {
                    cust.CustomerPwd = custs.CustomerPwd;
                }
                cust.CustomerName = txtCustomerName.Text;
                cust.CustomerState = custs.CustomerState;
                cust.CreateDate = custs.CreateDate;
                cust.CustomerID = custId;
                bool result = new BLL.CustomerInfo().Update(cust);
                if (result)
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