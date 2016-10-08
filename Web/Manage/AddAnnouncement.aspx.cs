using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AddAnnouncement :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }


        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                Model.Announcement ann = new Model.Announcement();
                ann.ATitle = txtATitle.Text.Trim();
                ann.AContent = txtAContent.Text.Trim();
                ann.ATime = GetConversion(DateTime.Now.ToString());
                ann.AType =Convert.ToInt32(droptype.SelectedValue);
                int result = new BLL.Announcement().Add(ann);
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