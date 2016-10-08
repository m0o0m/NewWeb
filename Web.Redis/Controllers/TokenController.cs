using GL.Common.Helper;
using GL.Data.BLL;
using GL.Redis.Data.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Redis.Controllers
{
    [RoutePrefix("api/Token")]
    public class TokenController : ApiController
    {
        private ILog log = LogManager.GetLogger("Token");

        [Route("State")]
        public Ret GetToken(string uid = "", string appid = "", string token = "")
        {
            //0 成功，是登录状态  2 uid不能为空    20 缺少APPID    997 Token过期   999 验证码校验失败

            var ret = HttpClientHelper.PostResponse<Ret>("http://passport.xyzs.com/checkLogin.php", string.Format("uid={0}&appid={1}&token={2}", uid, appid, token), "application/x-www-form-urlencoded");


            if (ret.ret == "0")
            {
                var res = QQZoneBLL.Add(new GL.Data.Model.QQZone { OpenID = uid, Json = token, UpdateTime = DateTime.Now });
            }

            return ret;
        }

    }
}

