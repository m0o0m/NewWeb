using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class AddVIP : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (Session["Agent"] != null)
            {
                txtAgentAccount.ReadOnly = true;
            }
        }

        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {

                Model.GameUserInfo guser = new Model.GameUserInfo();
                guser.GUAccount = txtGUAccount.Text.Trim();
                guser.GUPasswd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtGUPasswd.Text.Trim(), "MD5"); ;
                guser.GUEmail = txtGUEmail.Text.Trim();
                guser.GUName = txtGUName.Text.Trim();
                guser.GUTel = txtGUTel.Text.Trim();
                guser.GUExtend_Sex =Convert.ToInt32(dropsex.SelectedValue);
                guser.GUExtend_Signature = txtGUExtend_Signature.Text.Trim();
                guser.GUExtend_Birthday = GetConversion(txtGUExtend_Birthday.Text.Trim());
                guser.GUExtend_RealName = txtGUExtend_RealName.Text;
                guser.GUExtend_Address = txtGUExtend_Address.Text;
                guser.GUExtend_IDCardNo = txtGUExtend_IDCardNo.Text;
                guser.GURegisterTime =GetConversion(DateTime.Now.ToString());
                if (Session["manager"] != null)
                {
                    Model.AgentInfo agent = new BLL.AgentInfo().GetModel(txtAgentAccount.Text.Trim());
                    if (agent == null)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('代理不存在，请重新输入')</script>");
                        return;
                    }
                    else
                    {
                        guser.GUParentUserID = agent.AgentID;
                    }

                }
                else if (Session["Agent"] != null)
                {
                    guser.GUParentUserID = ((Model.AgentInfo)Session["Agent"]).AgentID;
                }


                int result = new BLL.GameUserInfo().Add(guser);
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

        /// <summary>
        /// 将日期转为秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
   
        public long GetConversion(string time)
        {
            DateTime dt1 = new DateTime(1970, 1, 1);
            DateTime dt = Convert.ToDateTime(time);
            TimeSpan tp = dt - dt1;
            long ver = tp.Ticks / 10000000;
            return ver;
        }
    }
}