using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class UpdateVIP :Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int vipId = Convert.ToInt32(Request.QueryString["id"]);
                Model.GameUserInfo guser = new BLL.GameUserInfo().GetModel(vipId);
                txtGUAccount.Text = guser.GUAccount;
                txtGUEmail.Text = guser.GUEmail;
                txtGUExtend_Address.Text = guser.GUExtend_Address;
                txtGUExtend_IDCardNo.Text = guser.GUExtend_IDCardNo;
                txtGUExtend_RealName.Text = guser.GUExtend_RealName;
                txtGUExtend_Signature.Text = guser.GUExtend_Signature;
                txtGUExtend_Birthday.Text = Convert.ToString(zhtime(guser.GUExtend_Birthday));
                dropsex.SelectedValue = Convert.ToString(guser.GUExtend_Sex);
                txtGUName.Text = guser.GUName;
                txtGUTel.Text = guser.GUTel;
                txtAgentAccount.Text = (new BLL.AgentInfo().GetModel(guser.GUParentUserID).AgentAccount);
            }
        }


        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DateTime zhtime(long time)
        {
            DateTime dt = new DateTime(1970, 1, 1);

            DateTime date = new DateTime(dt.Ticks+(time * 10000000));

            return date;

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


        protected void btnSava_Click(object sender, EventArgs e)
        {
            try
            {
                int vipId = Convert.ToInt32(Request.QueryString["id"]);
                Model.GameUserInfo gu = new Model.GameUserInfo();
                gu.GUUserID = vipId;
                gu.GUAccount = txtGUAccount.Text.Trim();
                gu.GUName = txtGUName.Text.Trim();
                gu.GUEmail = txtGUEmail.Text.Trim();
                gu.GUTel = txtGUTel.Text.Trim();
                gu.GUExtend_RealName = txtGUExtend_RealName.Text.Trim();
                gu.GUExtend_IDCardNo = txtGUExtend_IDCardNo.Text.Trim();
                gu.GUExtend_Address = txtGUExtend_Address.Text.Trim();
                gu.GUExtend_Signature = txtGUExtend_Signature.Text;
                gu.GUExtend_Sex =Convert.ToInt32(dropsex.SelectedValue);
                gu.GUExtend_Address = txtGUExtend_Address.Text.Trim();
                gu.GUExtend_Birthday = GetConversion(txtGUExtend_Birthday.Text);
                gu.GUParentUserID=(new BLL.AgentInfo().GetModel(txtAgentAccount.Text.Trim()).AgentID);
                if (txtGUPasswd.Text.Trim() != "")
                {
                    gu.GUPasswd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txtGUPasswd.Text.Trim(), "MD5");
                }
                else
                {
                 
                    Model.GameUserInfo guser = new BLL.GameUserInfo().GetModel(vipId);
                    gu.GUPasswd = guser.GUPasswd;
                }

                bool result = new BLL.GameUserInfo().UpdateVIP(gu);
                if (result)
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