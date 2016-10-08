using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class UpdateAnn : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int aid = Convert.ToInt32(Request.QueryString["id"]);
                Model.Announcement ann = new BLL.Announcement().GetModel(aid);
                txtATitle.Text = ann.ATitle;
                txtAContent.Text = ann.AContent;
                droptype.SelectedValue =Convert.ToString(ann.AType);

            }
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                Model.Announcement ann = new Model.Announcement();
                ann.ATitle = txtATitle.Text.Trim();
                ann.AContent = txtAContent.Text.Trim();
                ann.ATime = GetConversion(DateTime.Now.ToString());
                ann.AID = Convert.ToInt32(Request.QueryString["id"]);
                bool result = new BLL.Announcement().Update(ann);
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