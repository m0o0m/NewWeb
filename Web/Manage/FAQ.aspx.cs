using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class FAQ : Public
    {

        protected string strSession = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            AspNetPager1.RecordCount = new BLL.FAQ().GetRecordCount("");
            FAQBind();
         
        }


        public string GetString(string str, int length)
        {
            if (str.Length > length)
            {
                if (Session["Cust"] != null)
                {
                    return str;
                }
                else
                {
                    return str = str.Substring(0, length) + "...";
                }
            }
            else
            {
                return str;
            }
        }


        public string gettype(string tmp)
        {
            string value = string.Empty;
            switch (tmp)
            {


                case "1":
                    value = "游戏问题";
                    break;
                case "2":
                    value = "充值问题";
                    break;
                case "3":
                    value = "兑换问题";
                    break;
                case "4":
                    value = "其他问题";
                    break;



            }
            return value;

        }

        public void FAQBind()
        {
            Repeater1.DataSource = new BLL.FAQ().GetListByPage("", " operdate desc", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
            Repeater1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string SqlWhere =" 1=1 ";
            if (txtfaqtitle.Text.Trim() != "")
            {
                SqlWhere += " and faqtitle like '" + txtfaqtitle.Text.Trim() + "%'";
            }
            else if (droptype.SelectedValue != "0")
            {
                SqlWhere += " and faqtype=" + droptype.SelectedValue + " ";
            }


            Repeater1.DataSource = new BLL.FAQ().GetListByPage(SqlWhere, " operdate desc", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
            Repeater1.DataBind();
           
         
         
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case"Del":
                    int id = Convert.ToInt32(e.CommandArgument);
                    new BLL.FAQ().Delete(id);
                    FAQBind();
                    break;

                case"Update":
                    int uid = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("UpdateFAQ.aspx?id="+uid+"");
                    break;
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {

        }

        protected void form1_Init(object sender, EventArgs e)
        {
            if( (Session["Cust"] != null))
            {
                
                strSession = Session["Cust"].ToString();

                
                //Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('大大大');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>GetSession();</script>");
            }
        }
    }
}