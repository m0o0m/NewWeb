using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace GS.Web
{
    public class Public:System.Web.UI.Page 
    {

        public Public()
        {


        }

        protected override void OnInit(EventArgs O)
        {
            //if (base.Session["manager"] == null || base.Session["manager"].ToString().Equals(""))
            //{
            //    Response.Write("<script>alert('超时,请重新登录');window.location.href='';</script>");
            //}

            GetQx();
        }



   

        /// <summary>
        /// 根据权限编号赋予相应的权限
        /// </summary>
        public void fqx(string str){
            //string str = "1,2,3,4,5,6,7";
            if (!string.IsNullOrEmpty(str))
            {
                string[] s = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                List<string> list = new List<string>(s);

                if (!list.Contains("1"))
                {
                    if (System.IO.Path.GetFileName(Request.PhysicalPath) == "AddAgent.aspx")
                    {
                        Response.Redirect("Error.aspx");
                    }
                }
            }

            //for (int i = 0; i < s.Length; i++)
            //{

            //    if (!s[i].Contains("1"))
            //    {
            //        //是否等于1

            //        if (System.IO.Path.GetFileName(Request.PhysicalPath) == "AddAgent.aspx")
            //        {
            //            Response.Redirect("Error.aspx");
            //        }
            //    }
            //    //else if (s[i] != "2")
            //    //{

            //    //}
            //    //else if (s[i] != "3")
            //    //{

            //    //}

            //}
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



        /// <summary>
        /// 获取权限
        /// </summary>
        public void GetQx()
        {
            string[] array = { "ListOfConfiguration.aspx", "FAQ.aspx", "AddFAQ.aspx", "Update.aspx", "Management.aspx", "AddManage.aspx", "UpdateManage.aspx", "BlackListInfo.aspx", "AddCustomer.aspx", "CustomerInfo.aspx", "OnlineProblem.aspx", "AnnouncementInfo.aspx", "AddAnnouncement.aspx", "DataCeaning.aspx" };

            List<string> list = new List<string>(array);

            if (base.Session["manager"] == null && base.Session["Agent"]!=null)
            {
                if (list.Contains(System.IO.Path.GetFileName(Request.PhysicalPath)))
                {
                    
                    Response.Redirect("Error.aspx");
                }
                fqx(((Model.AgentInfo)Session["Agent"]).JurisdictionID);
            }
            else if (base.Session["manager"] != null && ((Model.ManagerInfo)Session["manager"]).AdminMasterRight != 1)
            {
                if (System.IO.Path.GetFileName(Request.PhysicalPath) == "AddManage.aspx" || System.IO.Path.GetFileName(Request.PhysicalPath) == "AgentPI.aspx" )
                {

                    Response.Redirect("Error.aspx");
                }
            }
            else if (base.Session["manager"] != null && base.Session["Agent"] == null)
            {
                if (System.IO.Path.GetFileName(Request.PhysicalPath) == "AgentPI.aspx")
                {
                    Response.Redirect("Error.aspx");
                }
            }
            else if (base.Session["Cust"] != null)
            {
                switch (System.IO.Path.GetFileName(Request.PhysicalPath))
                {
                    default :
                    Response.Redirect("Error.aspx");
                    break;
                    case"OnlineProblem.aspx":
                  
                    break;
                    case"FAQ.aspx":
                    
                    break;
                }
               
                
            }
            else if (base.Session["manager"] == null && base.Session["Agent"] == null && base.Session["Cust"]==null)
            {

            }
        }


        /// <summary>
        /// 获取权限 1增加  2删除  3修改  4查询
        /// </summary>
        /// <returns></returns>
        //public bool GetQx()
        //{

        //}

        /// <summary>
        /// 添加日志
        /// </summary>
        public void AddLog(int OperateID, int OperateType, DateTime OperateTime,string Remark)
        {
            try
            {
                Model.OperationLogInfo operlog = new Model.OperationLogInfo();
                operlog.OperateID = OperateID;
                operlog.OperateType = OperateType;
                operlog.OperateTime = OperateTime;
                operlog.Remark = Remark;
                int result = new BLL.OperationLogInfo().Add(operlog);
               
            }
            catch (Exception ec)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "<script>alert('"+ec.Message+"')</script>", "");
                throw;
            }
           

        }
    }
}