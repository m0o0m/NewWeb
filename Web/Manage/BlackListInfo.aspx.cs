using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class BlackListInfo :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            AspNetPager1.RecordCount = new BLL.BlackList().GetRecordCount("");
            BlackListBind();    
        }

        public void BlackListBind()
        {
            Repeater1.DataSource = new BLL.BlackList().GetListByPage("", " ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
            Repeater1.DataBind();
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case"Del":
                    int bid = Convert.ToInt32(e.CommandArgument);
                    new BLL.BlackList().Delete(bid);
                    BlackListBind();
                    break; 
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BlackListBind();
        }
    }
}