using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class GameSpecies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["tid"]))
            {
                int tid = Convert.ToInt32(Request.QueryString["tid"]);

                Model.GameTypeInfo gt = new BLL.GameTypeInfo().GetModel(tid);
                dropImgID.SelectedValue =Convert.ToString(gt.ImageID);
                txtTypeName.Text = gt.TypeName;
                txtSortID.Text =Convert.ToString(gt.SortID);

            }
        }

        protected void btnSubSpecies_Click(object sender, EventArgs e)
        {
            try
            {
                     Model.GameTypeInfo gt = new Model.GameTypeInfo();
                     gt.TypeName = txtTypeName.Text.Trim();
                     gt.SortID = Convert.ToInt32(txtSortID.Text.Trim());
                     gt.ImageID = Convert.ToInt32(dropImgID.SelectedValue);
                     gt.Enable = true;

                     int result = 0;
                     if (!string.IsNullOrEmpty(Request.QueryString["tid"]))
                     {
                         int tid = Convert.ToInt32(Request.QueryString["tid"]);
                         gt.TypeID = tid;
                         result = new BLL.GameTypeInfo().Update(gt);
                     }
                     else
                     {
                         result = new BLL.GameTypeInfo().Add(gt);
                     }
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