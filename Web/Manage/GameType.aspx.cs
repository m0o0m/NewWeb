using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class GameType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            dropBing();
            if (!string.IsNullOrEmpty(Request.QueryString["kid"]))
            {
                int kid = Convert.ToInt32(Request.QueryString["kid"]);
                Model.GameKindInfo gk = new BLL.GameKindInfo().GetModel(kid);
                txtKindName.Text = gk.KindName;
                txtSortID.Text = Convert.ToString(gk.SortID);
                dropType.SelectedValue = Convert.ToString(gk.TypeID);
                txtTableCount.Text = Convert.ToString(gk.TableCount);
                txtCellScore.Text = Convert.ToString(gk.CellScore);
                txtHighScore.Text = Convert.ToString(gk.HighScore);
                txtLessScore.Text = Convert.ToString(gk.LessScore);
                txtProcessType.Text = Convert.ToString(gk.ProcessType);
                txtTaxRate.Text = Convert.ToString(gk.TaxRate);
                txtBetPool.Text = Convert.ToString(gk.BetPool);
                TextBox1.Text = Convert.ToString(gk.AILevel);
                txtAIUserCount.Text = Convert.ToString(gk.AIUserCount);
                txtProcessType.ReadOnly = true;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["tid"])) 
            {
                int tid = Convert.ToInt32(Request.QueryString["tid"]);
                dropType.SelectedValue = Convert.ToString(tid);
                dropType.Enabled = false;
                txtProcessType.ReadOnly = false;
                    
            }   
        }


        public void dropBing()
        {
            dropType.DataTextField = "TypeName";
            dropType.DataValueField = "TypeID";
            dropType.DataSource = new BLL.GameTypeInfo().GetList(" Enable=1");
            dropType.DataBind();

        }

        protected void btnSubSpecies_Click(object sender, EventArgs e)
        {
            try
            {
                Model.GameKindInfo gk = new Model.GameKindInfo();
                gk.KindName = txtKindName.Text.Trim();
                gk.SortID = Convert.ToInt32(txtSortID.Text);
                gk.TypeID =Convert.ToInt32(dropType.SelectedValue);
                gk.TableCount = Convert.ToInt32(txtTableCount.Text);
                gk.HighScore = Convert.ToInt32(txtHighScore.Text);
                gk.CellScore = Convert.ToInt32(txtCellScore.Text);
                gk.LessScore = Convert.ToInt32(txtLessScore.Text);
                gk.TaxRate = Convert.ToDecimal(txtTaxRate.Text)/1000;
                gk.AIUserCount = Convert.ToInt32(txtAIUserCount.Text);
                gk.AILevel = Convert.ToInt32(TextBox1.Text);
                gk.BetPool = Convert.ToInt32(gk.BetPool);
                gk.ProcessType =Convert.ToInt32(txtProcessType.Text);
                gk.Enable = true;
                bool result = false;
                if (!string.IsNullOrEmpty(Request.QueryString["kid"]))
                {
                    int kid = Convert.ToInt32(Request.QueryString["kid"]);
                    gk.KindID = kid;
                    result = new BLL.GameKindInfo().Update(gk);
                }
                else
                {
                    result = new BLL.GameKindInfo().Add(gk);
                }

                if (result)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存成功')</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<scirpt>alert('保存失败')</script>");
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}