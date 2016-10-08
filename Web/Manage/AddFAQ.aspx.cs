using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AddFAQ : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }



        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                Model.FAQ faq = new Model.FAQ();
                faq.faqtitle = txtfaqtitle.Text.Trim();
                faq.faqcontent = txtfaqcontent.Text.Trim();
                faq.faqtype = Convert.ToInt32(droptype.SelectedValue);
                faq.operdate = DateTime.Now;
                int result = new BLL.FAQ().Add(faq);
                if (result > 0)
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