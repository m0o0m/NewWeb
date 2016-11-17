using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MWeb
{
    public class LogAttribute: ActionFilterAttribute
    {

        // <summary>
        /// 参数名称列表,可以用, | 分隔
        /// </summary>
        private readonly string _parameterNameList;


        //类型名称
        private string _activityLogTypeName = "";

        /// <summary>
        /// 活动日志
        /// </summary>
        /// <param name="activityLogTypeName">类别名称</param>
        /// <param name="parm">参数名称列表,可以用, | 分隔</param>
        public LogAttribute(string activityLogTypeName, string parm)
        {
            _activityLogTypeName = activityLogTypeName;
            _parameterNameList = parm;
        }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            if (workContext != null && workContext.CurrentCustomer != null)
            {
                Dictionary<string, string> parmsObj = new Dictionary<string, string>();

                foreach (var item in _parameterNameList.Split(',', '|'))
                {
                    var valueProviderResult = filterContext.Controller.ValueProvider.GetValue(item);

                    if (valueProviderResult != null && !parmsObj.ContainsKey(item))
                    {
                        parmsObj.Add(item, valueProviderResult.AttemptedValue);
                    }
                }
                //日志内容
                StringBuilder logContent = new StringBuilder();

                foreach (KeyValuePair<string, string> kvp in parmsObj)
                {
                    logContent.AppendFormat("{0}:{1} ", kvp.Key, kvp.Value);
                }
                //******************************************************************************
                //这里是插入数据表操作
                //步骤：
                //1、根据日志类型表的SystemKeyword得到日志类型Id
                //2、往日志表里插入数据，logContent.ToString()是内容，内容可以自己拼接字符串，比如：string.Format("删除记录，删除操作者{0}","xxxx");


                //var _customerActivityService = EngineContext.Current.Resolve<ICustomerActivityService>();
                //_customerActivityService.InsertActivity(_activityLogTypeName, logContent.ToString(), workContext.CurrentCustomer, workContext.CurrentCustomer.Id);


                //******************************************************************************
            }
        }
    }
}