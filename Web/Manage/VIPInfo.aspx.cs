using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class VIPInfo : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string strWhere = string.Empty;
            if (Session["manager"] != null)
            {
                strWhere = " GUUserID>'2455'";
            }
            else if (Session["Agent"] != null)
            {
                strWhere = " GUParentUserID='" + ((Model.AgentInfo)Session["Agent"]).AgentID + "'";
            }

            AspNetPager1.RecordCount = new BLL.GameUserInfo().GetRecordCount(strWhere);
            VIPBind();
        }


        /// <summary>
        /// 获取各类信息
        /// </summary>
        /// <returns></returns>
        public string getInfo(string tmp,string flag)
        {
            string info = string.Empty;
            switch (flag)
            {
                case"1":
                    DateTime dt1 = new DateTime(1970, 1, 1);
                    DateTime dt = new DateTime(dt1.Ticks+(long.Parse(tmp) * 10000000));
                    info = Convert.ToString(dt);
                    break;
                case"2":
                    if (tmp == "0")
                    {
                        info = "男";
                    }
                    else
                    {
                        info = "女";
                    }
                    break;
                case"3":
                    Model.AgentInfo ag = new BLL.AgentInfo().GetModel(Convert.ToInt32(tmp));
                    info = ag.AgentAccount;
                    break;

            }
            return info;
        }

        /// <summary>
        /// 会员绑定
        /// </summary>
        public void VIPBind()
        {

            if (Session["manager"] != null)
            {
                Repeater1.DataSource = new BLL.GameUserInfo().GetListByPage("GUUserID>'2455'", " ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
                Repeater1.DataBind();
                for (int i = 0; i < Repeater1.Items.Count; i++)
                {
                    Repeater1.Items[i].FindControl("linkPullblack").Visible = true;
                }

            }
            else if (Session["Agent"] != null)
            {
                Repeater1.DataSource = new BLL.GameUserInfo().GetListByPage(" GUParentUserID='"+((Model.AgentInfo)Session["Agent"]).AgentID+"'", " ", AspNetPager1.StartRecordIndex, AspNetPager1.EndRecordIndex);
                Repeater1.DataBind();
                for (int i = 0; i < Repeater1.Items.Count; i++)
                {
                    Repeater1.Items[i].FindControl("linkPullblack").Visible = false;
                }
            }

           
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case"Del":
                    int dvipId=Convert.ToInt32(e.CommandArgument);
                    new BLL.GameUserInfo().Delete(dvipId);
                    VIPBind();
                    break;

                case"VIPShift":
                    int svipId = Convert.ToInt32(e.CommandArgument);
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('会员转移','VIPShift.aspx?id=" + svipId + "',380,200)</script>");
                    break;

                case"Update":
                    int uvipId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("UpdateVIP.aspx?id=" + uvipId + "");
                    break;
                case"VIPPB":
                    int pvipId = Convert.ToInt32(e.CommandArgument);
                    bool iscz = new BLL.BlackList().Exists(pvipId);
                    if (iscz)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('黑名单已存在，不能重复拉黑')</script>");
                    }
                    else
                    {
                        Model.GameUserInfo guser = new BLL.GameUserInfo().GetModel(pvipId);
                        Model.BlackList bl = new Model.BlackList();
                        bl.ViPId = pvipId;
                        bl.Account = guser.GUAccount;
                        bl.Email = guser.GUEmail;
                        bl.LoginIP = "";
                        bl.QQ = "";
                        bl.RealName = guser.GUExtend_RealName;
                        bl.MobilePhone = guser.GUTel;
                        bl.BankInformation = " 银行游戏豆：'" + guser.GUExtend_Score2 + "', 银行金币：'" + guser.GUExtend_Money2 + "' ， 银行奖券： '" + guser.GUExtend_Lottery2 + "' , 银行密码：'" + guser.GUExtend_BandPasswd + "'";
                        int result = new BLL.BlackList().Add(bl);
                        if (result > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('拉黑成功')</script>");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('拉黑失败')</script>");
                        }
                    }

                    break;

            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            VIPBind();
        }


    }
}