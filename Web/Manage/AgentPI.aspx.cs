using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AgentPI :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            AgentPIBind();

        }
        public void AgentPIBind()
        {
            int agenetId = ((Model.AgentInfo)Session["Agent"]).AgentID;
            Repeater1.DataSource = new BLL.AgentInfo().GetList(" AgentID='" + agenetId + "'");
            Repeater1.DataBind();
        }

    }
}