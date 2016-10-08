using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class DialogueInfo : System.Web.UI.Page
    {
        public string list;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!string.IsNullOrEmpty(Request.QueryString["vid"]) && !string.IsNullOrEmpty(Request.QueryString["mid"]))
            {
                int msid = Convert.ToInt32(Request.QueryString["mid"]);
                int vid = Convert.ToInt32(Request.QueryString["vid"]);
                list = new BLL.CustomerServCenter().GetDialogueInfo(msid, msid, vid);
            }
        }


        /// <summary>
        /// 将日期转为秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetConversion()
        {
            DateTime dt1 = new DateTime(1970, 1, 1);
            DateTime dt = DateTime.Now;
            TimeSpan tp = dt - dt1;
            long ver = tp.Ticks / 10000000;
            return ver;
        }

        protected void btnSub_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["vid"]) && !string.IsNullOrEmpty(Request.QueryString["mid"]))
                {
                    int msid = Convert.ToInt32(Request.QueryString["mid"]);
                    Model.CustomerServCenter cust = new Model.CustomerServCenter();
                    if (Session["manager"] != null)
                    {
                        Model.ManagerInfo ma = ((Model.ManagerInfo)Session["manager"]);

                      
                        cust.GUUserID = ma.AdminID;
                        cust.GUName = ma.AdminName;
                        cust.GUType = 1;
                    }
                    else if (Session["Cust"] != null)
                    {
                        Model.CustomerInfo custinfo = ((Model.CustomerInfo)Session["Cust"]);
                    
                        cust.GUUserID = custinfo.CustomerID;
                        cust.GUName = custinfo.CustomerName;
                        cust.GUType =2;
                    }
                    cust.CSCSubId = msid;
                    cust.CSCContent = txthdcontent.Text.Trim();
                    cust.CSCTime = GetConversion();
             
                    cust.CSCState = 1;
      
                    cust.CSCTitle = null;
                    int result = new BLL.CustomerServCenter().Add(cust);
                    if (result>0)
                    {
                       
                        int vid = Convert.ToInt32(Request.QueryString["vid"]);
                        list = new BLL.CustomerServCenter().GetDialogueInfo(msid, msid, vid);
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>window.scroll(0,document.body.scrollHeight) </script>");
                        txthdcontent.Text = string.Empty;
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('回复失败')</script>");
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}