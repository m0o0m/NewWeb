using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AgentInfo : Public
    {
        public string navlist;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            AspNetPager1.RecordCount = new BLL.AgentInfo().GetRecordCount("");
            AgentBind("");
           

      
            
        }


        public string GetOnlineState(string state)
        {
            string sn = string.Empty;
            switch (state)
            {
                case"0":
                    sn = "未在线";
                    break;
                case"1":
                    sn = "在线";
                    break;
            }
            return sn;
        }

        /// <summary>
        /// 代理绑定
        /// </summary>
        public void AgentBind(string tmp)
        {
            int higherlevel = 0;
            if (base.Session["Agent"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    higherlevel = Convert.ToInt32(Request.QueryString["id"]);
                }
                else
                {
                    higherlevel = ((Model.AgentInfo)Session["Agent"]).AgentID;
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    higherlevel = Convert.ToInt32(Request.QueryString["id"]);
                }
            }

            navlist = GetNavigation(higherlevel);
            Repeater1.DataSource = new BLL.AgentInfo().GetListByPage(" HigherLevel='" + higherlevel + "' "+tmp+"", " RegisterTime desc", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
            Repeater1.DataBind();
        }


        /// <summary>
        /// 获取导航
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetNavigation(int Id)
        {
          string nav =string.Empty;
          if (Id != 0)
          {
              Model.AgentInfo agents = new BLL.AgentInfo().GetModel(Id);

              if (agents.HigherLevel != 0)
              {
                  Model.AgentInfo aenet = new BLL.AgentInfo().GetModel(Convert.ToInt32(agents.HigherLevel));


                  nav += " <a href='AgentInfo.aspx?id=" + aenet.AgentID + "' class='current'>" + aenet.AgentName + "</a>";
              }
              nav += " <a href='AgentInfo.aspx?id=" + agents.AgentID + "' class='current'>" + agents.AgentName + "</a>";
              //nav += " <a href='AgentInfo.aspx?id="+agents.AgentID+"' class='current'>" + agents.AgentName + "</a>";
          } 
            return nav;
        }



   

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case"Del":
                    int agentId = Convert.ToInt32(e.CommandArgument);
                    bool extits = new BLL.AgentInfo().Exists(" HigherLevel='" + agentId + "'");
                    bool extitsvip = new BLL.GameUserInfo().extitsvip(" GUParentUserID='" + agentId + "'");
                    if (extits || extitsvip)
                    {

                     Page.ClientScript.RegisterStartupScript(GetType(),"","<script>alert('该代理存在下级代理或会员,请亲转移或者删除下级代理或者会员在做操作')</script>");
                    }
                    else
                    {
                        new BLL.AgentInfo().Delete(agentId);

                        AgentBind("");
                    }
                    break;
                case"AgShift":
                    int agId = Convert.ToInt32(e.CommandArgument);
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('代理转移','AgShift.aspx?id=" + agId + "',380,200)</script>");
                    break;

                case"Update":
                    int uageId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("UpdateAgent.aspx?id="+uageId+"");
                    break;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string tmp = " and AgentAccount like '" + txtAgentAccount.Text.Trim()+"%' " ;
            AgentBind(tmp);
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            AgentBind("");
        }

        

    }
}