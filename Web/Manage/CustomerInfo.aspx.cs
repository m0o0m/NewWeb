using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class CustomerInfo :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            AspNetPager1.RecordCount = new BLL.CustomerInfo().GetRecordCount("");
            CustometBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            Repeater1.DataSource = new BLL.CustomerInfo().GetListByPage(" CustomerAccount like '"+txtAgentAccount.Text.Trim()+"%' ", "CreateDate desc ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
            Repeater1.DataBind();

        }


        /// <summary>
        /// 客服绑定
        /// </summary>
        public void CustometBind()
        {
            Repeater1.DataSource = new BLL.CustomerInfo().GetListByPage("", "CreateDate desc ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
            Repeater1.DataBind();
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetState(string state)
        {
            string S = string.Empty;
            switch (state)
            {
                case"0":
                    S = "正常使用";
                    break;
                case"1":
                    S = "禁用";
                    break;
            }
            return S;


        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Del":
                    int did=Convert.ToInt32(e.CommandArgument);
                    new BLL.CustomerInfo().Delete(did);
                    CustometBind();
                    break;
                case"Update":
                    int uid = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("UpdateCustomer.aspx?id=" + uid + "");

                    break;
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            CustometBind();
        }
    }
}