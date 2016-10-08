using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace GS.Web.Manage
{
    /// <summary>
    /// Service 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)]
        public int ClearSession()
        {
            if (Session["Agent"] != null)
            {
                Model.AgentInfo ags = new Model.AgentInfo();

                ags.AgentID = ((Model.AgentInfo)Session["Agent"]).AgentID;
                ags.OnlineState = 0;
                new BLL.AgentInfo().UpdateONState(ags);
            }
            Session.Clear();


            return 0;
        }

        
        

        //定义数组保存获取的内容 
        private string[] autoCompleteWordList = null;
        //两个参数“prefixText”表示用户输入的前缀，count表示返回的个数 
        [WebMethod]
        public String[] GetCompleteDepart(string prefixText, int count)
        {
            ///检测参数是否为空 
            if (string.IsNullOrEmpty(prefixText) == true || count <= 0) return null;
            // 如果数组为空 
            if (autoCompleteWordList == null)
            {
                //读取数据库的内容 
                DataSet ds = new BLL.AgentInfo().GetList("   AgentAccount like'" + prefixText + "%' order by RegisterTime desc");

                //读取内容文件的数据到临时数组 
                string[] temp = new string[ds.Tables[0].Rows.Count];
                int i = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    temp[i] = dr["AgentAccount"].ToString();
                    i++;
                }
                Array.Sort(temp, new CaseInsensitiveComparer());
                //将临时数组的内容赋给返回数组 
                autoCompleteWordList = temp;
            }
            //定位二叉树搜索的起点 
            int index = Array.BinarySearch(autoCompleteWordList, prefixText, new CaseInsensitiveComparer());
            if (index < 0)
            { //修正起点 
                index = ~index; 
            }
            //搜索符合条件的数据 
            int matchCount = 0;
            for (matchCount = 0; matchCount < count && matchCount + index < autoCompleteWordList.Length; matchCount++)
            { ///查看开头字符串相同的项 
                if (autoCompleteWordList[index + matchCount].StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    break;
                }
            }
            //处理搜索结果 
            string[] matchResultList = new string[matchCount];
            if (matchCount > 0)
            { //复制搜索结果 
                Array.Copy(autoCompleteWordList, index, matchResultList, 0, matchCount);
            }
            return matchResultList;
        }




    }
}




//        private string[] autoCompleteWordList = null; 
////两个参数“prefixText”表示用户输入的前缀，count表示返回的个数 
//[WebMethod] 
//public String[] GetCompleteDepart(string prefixText, int count) 
//{ 
/////检测参数是否为空 
//if (string.IsNullOrEmpty(prefixText) == true || count <= 0) return null; 
//// 如果数组为空 
//if (autoCompleteWordList == null) 
//{ 
////读取数据库的内容 
//OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("Ex18_02.mdb")); 
//conn.Open(); 
//OleDbDataAdapter da = new OleDbDataAdapter("select keyName from keyInfo where keyName like'" + prefixText + "%' order by keyName", conn); 
//DataSet ds = new DataSet(); 
//da.Fill(ds); 
////读取内容文件的数据到临时数组 
//string[] temp = new string[ds.Tables[0].Rows.Count]; 
//int i = 0; 
//foreach (DataRow dr in ds.Tables[0].Rows) 
//{ 
//temp[i] = dr["keyName"].ToString(); 
//i++; 
//} 
//Array.Sort(temp, new CaseInsensitiveComparer()); 
////将临时数组的内容赋给返回数组 
//autoCompleteWordList = temp; 
//if (conn.State == ConnectionState.Open) 
//conn.Close(); 
//} 
////定位二叉树搜索的起点 
//int index = Array.BinarySearch(autoCompleteWordList, prefixText, new CaseInsensitiveComparer()); 
//if (index < 0) 
//{ //修正起点 
//index = ~index; 
//} 
////搜索符合条件的数据 
//int matchCount = 0; 
//for (matchCount = 0; matchCount < count && matchCount + index < autoCompleteWordList.Length; matchCount++) 
//{ ///查看开头字符串相同的项 
//if (autoCompleteWordList[index + matchCount].StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) == false) 
//{ 
//break; 
//} 
//} 
////处理搜索结果 
//string[] matchResultList = new string[matchCount]; 
//if (matchCount > 0) 
//{ //复制搜索结果 
//Array.Copy(autoCompleteWordList, index, matchResultList, 0, matchCount); 
//} 
//return matchResultList; 
//} 
//} 
   

