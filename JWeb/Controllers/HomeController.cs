using NS_OpenApiV3;
using NS_SnsNetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [QueryValues]
        public ActionResult Index(Dictionary<string, string> queryvalues)
        {

            bool firstLogin = false;
            string invkey = queryvalues.ContainsKey("invkey") ? queryvalues["invkey"].ToString() : "0";
            string iopenid = queryvalues.ContainsKey("iopenid") ? queryvalues["iopenid"].ToString() : "0";
            string itime = queryvalues.ContainsKey("itime") ? queryvalues["itime"].ToString() : "0";
            string pfkey = queryvalues.ContainsKey("pfkey") ? queryvalues["pfkey"].ToString() : "";
            string vertime = queryvalues.ContainsKey("VERTIME") ? queryvalues["VERTIME"].ToString() : "";
            string via = queryvalues.ContainsKey("via") ? queryvalues["via"].ToString() : "0";
            if (!string.IsNullOrEmpty(invkey))
            {
                firstLogin = true;
            }
            string openid = queryvalues.ContainsKey("openid") ? queryvalues["openid"].ToString() : "";
            string openkey = queryvalues.ContainsKey("openkey") ? queryvalues["openkey"].ToString() : "";
            string pf = queryvalues.ContainsKey("pf") ? queryvalues["pf"].ToString() : "";

            //
            string sessionStr = "";

            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            RstArray result = new RstArray();

            return View();
        }



        public RstArray verify_invkey(OpenApiV3 sdk, string openid, string openkey, string pf) {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", openid);
            param.Add("openkey", openkey);
            param.Add("pf", pf);
            param.Add("invkey", Request["invkey"]);
            param.Add("itime", Request["itime"]);
            param.Add("iopenid", Request["iopenid"]);

            string script_name = "/v3/spread/verify_invkey";

            RstArray arr = sdk.Api(script_name, param);

            return arr;
        }


        //用户信息
        public ActionResult GetUserInfo()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
            string script_name = "/v3/user/get_info";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);

        }

        //获取已安装了应用的好友列表
        public ActionResult GetAppFriends()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
            string script_name = "/v3/relation/get_app_friends";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }
        //获取多个好友信息
        public ActionResult Getmultiinfo()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
            param.Add("fopenids", Request["fopenids"]);

            string script_name = "/v3/user/get_multi_info";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }
        //验证登录用户是否安装了应用
        public ActionResult IsSetup()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
         

            string script_name = "/v3/user/is_setup";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }



        //验证用户的登录态，判断openkey是否过期，没有过期则对openkey有效期进行续期（一次调用续期2小时）。
        public ActionResult IsAreaLogin()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);


            string script_name = "/v3/user/is_login";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }



        /**
	 * 验证好友邀请的invkey，用于“邀请好友即赠送礼品”等场景。
	 *
	 * @param object $sdk OpenApiV3 Object
	 * @param string $openid openid
	 * @param string $openkey openkey
	 * @param string $pf 平台
	 * @return array 好友资料数组
	 */
        public ActionResult VerifyInvkey()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
            param.Add("invkey", Request["invkey"]);
            param.Add("itime", Request["itime"]);
            param.Add("iopenid", Request["iopenid"]);
            string script_name = "/v3/spread/verify_invkey";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }
        //获取好友资料
        public ActionResult TotalVipInfo()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
       
            string script_name = "/v3/user/total_vip_info";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }


        public ActionResult buyGoods()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
            param.Add("pfkey", Request["pfkey"]);
            param.Add("ts", DateTime.Now.ToString());
            param.Add("payitem", Request["payitem"]);
            param.Add("goodsmeta", Request["goodsmeta"]);
            param.Add("goodsurl", Request["goodsurl"]);
            param.Add("zoneid", "0");

            param.Add("appmode", Request["appmode"]);//暂时有问题，对比以前查看

            string script_name = "/v3/pay/buy_goods";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }



        public ActionResult buyGoods1()
        {

            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
            param.Add("pfkey", Request["pfkey"]);
            param.Add("ts", DateTime.Now.ToString());
            param.Add("payitem", Request["payitem"]);
            param.Add("goodsmeta", Request["goodsmeta"]);
            param.Add("goodsurl", Request["goodsurl"]);
            param.Add("zoneid", "0");

            param.Add("appmode","1");//暂时有问题，对比以前查看

            string script_name = "/v3/pay/buy_goods";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }

        //活动中心
        public ActionResult AppstoreActivity()
        {
            OpenApiV3 sdk = new OpenApiV3(1, "appkey");
            sdk.SetServerName("server_name");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", Request["openid"]);
            param.Add("openkey", Request["openkey"]);
            param.Add("pf", Request["pf"]);
            param.Add("pfkey", Request["pfkey"]);
            param.Add("ts", DateTime.Now.ToString());
            param.Add("payitem", Request["payitem"]);
            param.Add("goodsmeta", Request["goodsmeta"]);
            param.Add("goodsurl", Request["goodsurl"]);
            param.Add("zoneid", "0");

            param.Add("appmode", "1");//暂时有问题，对比以前查看

            string script_name = "/v3/pay/buy_goods";

            RstArray arr = sdk.Api(script_name, param);

            return Json(arr);
        }
    }
}