using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected string strSession = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                //strSession = Session["test"].ToString();
            }
        }

        protected void form1_Init(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('dasdas')</script>");
        }
    }
}