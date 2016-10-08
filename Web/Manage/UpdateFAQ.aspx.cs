using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class UpdateFAQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                Model.FAQ faq = new BLL.FAQ().GetModel(id);
                txtfaqtitle.Text = faq.faqtitle;
                txtfaqcontent.Text = faq.faqcontent;
                droptype.SelectedValue = Convert.ToString(faq.faqtype);

            }
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                Model.FAQ faq = new Model.FAQ();
                 int id = Convert.ToInt32(Request.QueryString["id"]);
                faq.faqtitle = txtfaqtitle.Text.Trim();
                faq.faqcontent = txtfaqcontent.Text.Trim();
                faq.faqtype = Convert.ToInt32(droptype.SelectedValue);
                faq.operdate = DateTime.Now;
                faq.Id = id;
                bool result = new BLL.FAQ().Update(faq);
                if (result)
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