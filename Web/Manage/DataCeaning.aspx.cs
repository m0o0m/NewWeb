using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class DataCeaning : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

        }

        
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="falg"></param>
        /// <param name="qsdate"></param>
        /// <param name="jsdate"></param>
        /// <returns></returns>
        public string getcount(string falg, string qsdate, string jsdate)
        {
            string value = string.Empty;
            switch (falg)
            {
                case"1":
                    value = "在日期 " + qsdate + " 至 " + jsdate + " 的记录总数为：" + new BLL.OperationLogInfo().GetRecordCount("   OperateType=1 and OperateTime between convert(varchar(10),'" + qsdate + "',120) and dateadd(day,1,'" + jsdate + "')") + "条";
                    break;
                case"2":
                    value = "在日期 " + qsdate + " 至 " + jsdate + " 的记录总数为：" + new BLL.OperationLogInfo().GetRecordCount("   OperateType=2 and OperateTime between convert(varchar(10),'" + qsdate + "',120) and dateadd(day,1,'" + jsdate + "')") + "条";
                    break;
                case"3":
                    value = "在日期 " + qsdate + " 至 " + jsdate + " 的记录总数为：" + new BLL.OperationLogInfo().GetRecordCount("   OperateType=3 and OperateTime between convert(varchar(10),'" + qsdate + "',120) and dateadd(day,1,'" + jsdate + "')") + "条";
                    break;

                case"4":
                    long qs = GetConversion(qsdate);
                    long js = GetConversion(jsdate);
                    value = "在日期 " + qsdate + " 至 " + jsdate + " 的记录总数为：" + new BLL.GameRoundInfo().GetRecordCount("    GRStartTime >= '" + qs + "' and  GRStartTime <= '" + js + "' ") + "条";


                    break;

            }

            return value;
        }

        protected void btnDataClear_Click(object sender, EventArgs e)
        {
            
            if (Convert.ToDateTime(txtjs.Text) < Convert.ToDateTime(txtqs.Text))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('结束日期不能小于开始日期')</script>");
            }
            else
            {
                string tmp = getcount(dropdatatype.SelectedValue, txtqs.Text, txtjs.Text);



                string js = string.Format("document.getElementById('{0}').value=confirm('" + tmp + ",是否确认清理数据?');document.getElementById('{1}').click();", hid.ClientID, btnHid.ClientID);
                ClientScript.RegisterStartupScript(GetType(), "confirm", js, true);

            }
        }



        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        public bool delRecord(string flag)
        {
            bool result = false;
            switch (flag)
            {
              
                case"1":
                    result = new BLL.OperationLogInfo().DeleteList(" select ID from OperationLogInfo where OperateType=1 and  OperateTime >= '" + txtqs.Text.Trim() + "' and OperateTime <= '" + txtjs.Text.Trim() + "' ");

                    break;
                case"2":
                    result = new BLL.OperationLogInfo().DeleteList(" select ID from OperationLogInfo where OperateType=2 and  OperateTime >= '" + txtqs.Text.Trim() + "' and OperateTime <= '" + txtjs.Text.Trim() + "'");
                    break;
                case"3":
                    result = new BLL.OperationLogInfo().DeleteList(" select ID from OperationLogInfo where OperateType=3 and  OperateTime >= '" + txtqs.Text.Trim() + "' and OperateTime <= '" + txtjs.Text.Trim() + "'");
 
                    break;

                case"4":
                    result = new BLL.UserChartInfo().DeleteList("  select GRID from GameRoundInfo where  GRStartTime>=" + GetConversion(txtqs.Text.Trim()) + "   and  GRStartTime<=" + GetConversion(txtjs.Text.Trim()) + " ");
                    result = new BLL.GameRoundInfo().DeleteList("  select GRID from GameRoundInfo where  GRStartTime>=" + GetConversion(txtqs.Text.Trim()) + "   and  GRStartTime<=" + GetConversion(txtjs.Text.Trim()) + "   ");
                    break;
                case"5":

                    break;
                case"6":

                    break;
            }

            return result;
        }

        protected void btnHid_Click(object sender, EventArgs e)
        {
            string result = hid.Value.ToLower() == "true" ? "是" : "否";
         
            if (result == "是")
            {
                // 进行数据的更新
                if (delRecord(dropdatatype.SelectedValue))
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('清理成功')</script>");
                }
               
              
            }
            else
            {
                //直接返回
               
                return;
            }
        }

    }
}