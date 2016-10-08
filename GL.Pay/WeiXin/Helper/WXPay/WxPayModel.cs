using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using GL.Pay.WxPay.Consts;

namespace GL.Pay.WxPay.Helper.WXPay
{
    public class WxPayModel
    {
        #region ���캯����˽�б���
        private WxPayModel()
        {
            this.parameters = new Dictionary<string, string>();
        }

        private Dictionary<string, string> parameters;
        public string AppId;
        private string AppKey;
        public string SignType;
        private string PartnerKey;
        #endregion

        #region Model������̬��
        /// <summary>
        /// ���� ֧��Model
        /// </summary>
        /// <param name="description">��Ʒ����</param>
        /// <param name="tradeNo">������</param>
        /// <param name="totalFee">��������Ϊ��λ��</param>
        /// <param name="notifyUrl">֧����ɺ󣬽���΢��֪֧ͨ�������Url</param>
        /// <param name="createIp">�û��������Ip</param>
        /// <returns></returns>
        public static WxPayModel Create(string description, string tradeNo, string totalFee, string notifyUrl, string createIp)
        {
            WxPayModel wxPayModel = new WxPayModel();
            //�����û�����Ϣ
            wxPayModel.SetAppId(WeiXinConst.AppId);
            wxPayModel.SetAppKey(WeiXinConst.PaySignKey);
            wxPayModel.SetPartnerKey(WeiXinConst.PartnerKey);
            wxPayModel.SetSignType("sha1");
            //��������package��Ϣ
            wxPayModel.SetParameter("bank_type", "WX");//�̶�
            wxPayModel.SetParameter("fee_type", "1");//ȡֵ��1������ң� ����ֻ֧�� 1��
            wxPayModel.SetParameter("input_charset", "UTF-8"); //
            wxPayModel.SetParameter("partner", WeiXinConst.PartnerId);//ע��ʱ����ĲƸ�ͨ�̻��� partnerId

            wxPayModel.SetParameter("body", description);//��Ʒ����
            wxPayModel.SetParameter("out_trade_no", tradeNo); //�̻�ϵͳ�ڲ��Ķ����ţ�32 ���ַ��ڡ��ɰ�����ĸ��ȷ�����̻�ϵͳΨһ��
            //todo:��ʱд��Ϊ 1��
            wxPayModel.SetParameter("total_fee", totalFee); //�����ܽ���λΪ�֣�
            wxPayModel.SetParameter("notify_url", notifyUrl); //��֧����ɺ󣬽���΢��֪֧ͨ�������URL���������·��,255�ַ���,��ʽ��:http://wap.tenpay.com/tenpay.asp��
            wxPayModel.SetParameter("spbill_create_ip", createIp);//ָ�û�������� IP�������̻������� IP����ʽΪIPV4��

            return wxPayModel;
        }

        /// <summary>
        /// ���� ������ѯModel
        /// </summary>
        /// <param name="tradeNo">������</param>
        /// <param name="isApp">�Ƿ�ΪApp֧�������ģ�Ĭ��Ϊ��</param>
        /// <returns></returns>
        public static WxPayModel Create(string tradeNo)
        {
            WxPayModel wxPayModel = new WxPayModel();
            //�����û�����Ϣ
            wxPayModel.SetAppId(WeiXinConst.AppId);
            wxPayModel.SetAppKey(WeiXinConst.PaySignKey);
            wxPayModel.SetPartnerKey(WeiXinConst.PartnerKey);
            wxPayModel.SetSignType("sha1");
            //��������package��Ϣ
            wxPayModel.SetParameter("partner", WeiXinConst.PartnerId);//ע��ʱ����ĲƸ�ͨ�̻��� partnerId

            wxPayModel.SetParameter("out_trade_no", tradeNo); //�̻�ϵͳ�ڲ��Ķ����ţ�32 ���ַ��ڡ��ɰ�����ĸ��ȷ�����̻�ϵͳΨһ��

            return wxPayModel;
        }

        /// <summary>
        /// ���� ��֤֧��֪ͨ�� Model
        /// </summary>
        /// <param name="isApp">�Ƿ�AppӦ��</param>
        /// <returns></returns>
        public static WxPayModel Create()
        {
            WxPayModel wxPayModel = new WxPayModel();
            //�����û�����Ϣ
            wxPayModel.SetAppId(WeiXinConst.AppId);
            wxPayModel.SetAppKey(WeiXinConst.PaySignKey);
            wxPayModel.SetPartnerKey(WeiXinConst.PartnerKey);
            wxPayModel.SetSignType("sha1");

            return wxPayModel;
        }

        /// <summary>
        /// ���ݴ����dic ���ò���
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="isApp"></param>
        /// <returns></returns>
        public static WxPayModel Create(Dictionary<string, string> dic)
        {
            WxPayModel wxPayModel = new WxPayModel();
            //�����û�����Ϣ
            wxPayModel.SetAppId(WeiXinConst.AppId);
            wxPayModel.SetAppKey(WeiXinConst.PaySignKey);
            wxPayModel.SetPartnerKey(WeiXinConst.PartnerKey);
            wxPayModel.SetSignType("sha1");

            foreach (var key in dic.Keys)
            {
                wxPayModel.SetParameter(key, dic[key]);
            }

            return wxPayModel;
        }

        #endregion

        public sealed class Anonymous_C0 :
                IComparer<KeyValuePair<string, string>>
        {
            public int Compare(KeyValuePair<string, string> o1,
                    KeyValuePair<string, string> o2)
            {
                return String.CompareOrdinal((o1.Key).ToString(), o2.Key);
            }
        }

        #region �����������
        public void SetAppId(string str)
        {
            AppId = str;
        }

        public void SetAppKey(string str)
        {
            AppKey = str;
        }

        public void SetSignType(string str)
        {
            SignType = str;
        }

        public void SetPartnerKey(string str)
        {
            PartnerKey = str;
        }

        public void SetParameter(string key, string value_ren)
        {
            if (parameters.ContainsKey(key))
            {
                parameters.Add(key, value_ren);
            }
            else
            {
                parameters[key] = value_ren;
            }
        }

        public string GetParameter(string key)
        {
            return parameters[key];
        }

        public string GetPartnerId()
        {
            return parameters["partner"];
        }

        private bool CheckCftParameters()
        {
            if (parameters["bank_type"] == "" || parameters["body"] == "" || parameters["partner"] == "" || parameters["out_trade_no"] == ""
                    || parameters["total_fee"] == "" || parameters["fee_type"] == "" || parameters["notify_url"] == null || parameters["spbill_create_ip"] == ""
                    || parameters["input_charset"] == "")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��� ֧���������� �Ƿ�����
        /// </summary>
        /// <returns></returns>
        private bool CheckOrderQueryParameters()
        {
            if (parameters["partner"] == "" || parameters["out_trade_no"] == "")
            {
                return false;
            }
            return true;
        }

        #endregion

        #region ���ɶ�������package��֧��ǩ��

        /// <summary>
        /// ���� �������� ��package��
        /// </summary>
        /// <returns></returns>
        public string GetCftPackage()
        {
            if ("" == PartnerKey)
            {
                throw new SDKRuntimeException("��Կ����Ϊ�գ�");
            }
            string unSignParaString = CommonUtil.FormatBizQueryParaMap(parameters,
                    false);
            string paraString = CommonUtil.FormatBizQueryParaMap(parameters, true);
            return paraString + "&sign="
                    + MD5SignUtil.Sign(unSignParaString, PartnerKey);

        }

        /// <summary>
        /// ֧��ǩ��
        /// </summary>
        /// <param name="bizObj"></param>
        /// <returns></returns>
        public string GetBizSign(Dictionary<string, string> bizObj)
        {
            Dictionary<string, string> bizParameters = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> item in bizObj)
            {
                if (item.Key != "")
                {
                    bizParameters.Add(item.Key.ToLower(), item.Value);
                }
            }

            if (this.AppKey == "")
            {
                throw new SDKRuntimeException("APPKEYΪ�գ�");
            }
            bizParameters.Add("appkey", AppKey);
            string bizString = CommonUtil.FormatBizQueryParaMap(bizParameters, false);

            return SHA1Util.Sha1(bizString);

        }

        #endregion

        #region ��������ʹ�õ�Package��

        /// <summary>
        /// ����appԤ֧������json���Ѳ���ͨ����
        /// </summary>
        /// <param name="traceid"></param>
        /// <returns></returns>
        /// <remarks>
        ///* { "appid":"wwwwb4f85f3a797777", "traceid":"crestxu",
        ///* "noncestr":"111112222233333", "package":
        ///* "bank_type=WX&body=XXX&fee_type=1&input_charset=GBK&notify_url=http%3a%2f%2f
        ///* www
        ///* .qq.com&out_trade_no=16642817866003386000&partner=1900000109&spbill_create_ip
        ///* =127.0.0.1&total_fee=1&sign=BEEF37AD19575D92E191C1E4B1474CA9",
        ///* "timestamp":1381405298,
        ///* "app_signature":"53cca9d47b883bd4a5c85a9300df3da0cb48565c",
        ///* "sign_method":"sha1" }
        /// </remarks>
        public string CreateAppPrePayPackage(string traceid)
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            if (CheckCftParameters() == false)
            {
                throw new SDKRuntimeException("����package����ȱʧ��");
            }
            nativeObj.Add("appid", AppId);
            nativeObj.Add("package", GetCftPackage());
            nativeObj.Add("timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            nativeObj.Add("traceid", traceid);
            nativeObj.Add("noncestr", CommonUtil.CreateNoncestr());
            nativeObj.Add("app_signature", GetBizSign(nativeObj));
            nativeObj.Add("sign_method", SignType);

            var entries = nativeObj.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));

            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        /// <summary>
        /// ����app֧������json���Ѳ���ͨ����
        /// </summary>
        /// <param name="traceid"></param>
        /// <returns></returns>
        /// <remarks>
        /// { 
        /// "appid":"wwwwb4f85f3a797777",
        /// "noncestr":"111112222233333", 
        /// "package":"Sign=WXpay",
        /// "partnerid":"1900000109",
        /// "prepayid":"1101000000140429eb40476f4c9",
        /// "sign":"53cca9d47b883bd4a5c85a9300df3da0cb48565c",
        /// "timestamp":1381405298,
        ///  }
        /// </remarks>
        public string CreateAppPayPackage(string prepayid)
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            if (CheckCftParameters() == false)
            {
                throw new SDKRuntimeException("����package����ȱʧ��");
            }
            nativeObj.Add("appid", AppId);
            nativeObj.Add("noncestr", CommonUtil.CreateNoncestr());
            nativeObj.Add("package", "Sign=WXPay");
            nativeObj.Add("partnerid", GetPartnerId());
            nativeObj.Add("prepayid", prepayid);
            nativeObj.Add("timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            nativeObj.Add("sign", GetBizSign(nativeObj));

            var entries = nativeObj.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));

            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        /// <summary>
        /// ���ɶ�����ѯJson  ���Ѳ���ͨ����
        /// </summary>
        /// <returns></returns>
        public string CreateOrderQueryPackage()
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            if (CheckOrderQueryParameters() == false)
            {
                throw new SDKRuntimeException("����package����ȱʧ��");
            }
            nativeObj.Add("appid", AppId);
            nativeObj.Add("package", GetCftPackage());
            nativeObj.Add("timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            nativeObj.Add("app_signature", GetBizSign(nativeObj));
            nativeObj.Add("sign_method", SignType);

            var entries = nativeObj.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));

            return "{" + string.Join(",", entries.ToArray()) + "}";
        }



        // ����jsapi֧������json
        /*
         * "appId" : "wxf8b4f85f3a794e77", //���ں����ƣ����̻����� "timeStamp" : "189026618",
         * //ʱ�����������ʹ����һ��ֵ "nonceStr" : "adssdasssd13d", //����� "package" :
         * "bank_type=WX&body=XXX&fee_type=1&input_charset=GBK&notify_url=http%3a%2f
         * %2fwww.qq.com&out_trade_no=16642817866003386000&partner=1900000109&
         * spbill_create_i
         * p=127.0.0.1&total_fee=1&sign=BEEF37AD19575D92E191C1E4B1474CA9",
         * //��չ�ֶΣ����̻����� "signType" : "SHA1", //΢��ǩ����ʽ:sha1 "paySign" :
         * "7717231c335a05165b1874658306fa431fe9a0de" //΢��ǩ��
         */
        public string CreateBizPackage()
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            if (CheckCftParameters() == false)
            {
                throw new SDKRuntimeException("����package����ȱʧ��");
            }
            nativeObj.Add("appId", AppId);
            nativeObj.Add("package", GetCftPackage());
            nativeObj.Add("timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            nativeObj.Add("noncestr", CommonUtil.CreateNoncestr());
            nativeObj.Add("paySign", GetBizSign(nativeObj));
            nativeObj.Add("signType", SignType);

            var entries = nativeObj.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));

            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        // ����ԭ��֧��url
        /*
         * weixin://wxpay/bizpayurl?sign=XXXXX&appid=XXXXXX&productid=XXXXXX&timestamp
         * =XXXXXX&noncestr=XXXXXX
         */
        public string CreateNativeUrl(string productid)
        {
            string bizString = "";
            try
            {
                Dictionary<string, string> nativeObj = new Dictionary<string, string>();
                nativeObj.Add("appid", AppId);
                nativeObj.Add("productid", System.Web.HttpUtility.UrlEncode(productid));
                nativeObj.Add("timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
                nativeObj.Add("noncestr", CommonUtil.CreateNoncestr());
                nativeObj.Add("sign", GetBizSign(nativeObj));
                bizString = CommonUtil.FormatBizQueryParaMap(nativeObj, false);

            }
            catch (Exception e)
            {
                throw new SDKRuntimeException(e.Message);

            }
            return "weixin://wxpay/bizpayurl?" + bizString;
        }

        // ����ԭ��֧������xml
        /*
         * <xml> <AppId><![CDATA[wwwwb4f85f3a797777]]></AppId>
         * <Package><![CDATA[a=1&url=http%3A%2F%2Fwww.qq.com]]></Package>
         * <TimeStamp> 1369745073</TimeStamp>
         * <NonceStr><![CDATA[iuytxA0cH6PyTAVISB28]]></NonceStr>
         * <RetCode>0</RetCode> <RetErrMsg><![CDATA[ok]]></ RetErrMsg>
         * <AppSignature><![CDATA[53cca9d47b883bd4a5c85a9300df3da0cb48565c]]>
         * </AppSignature> <SignMethod><![CDATA[sha1]]></ SignMethod > </xml>
         */
        public string CreateNativePackage(string retcode, string reterrmsg)
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            if (CheckCftParameters() == false && retcode == "0")
            {
                throw new SDKRuntimeException("����package����ȱʧ��");
            }
            nativeObj.Add("AppId", AppId);
            nativeObj.Add("Package", GetCftPackage());
            nativeObj.Add("TimeStamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            nativeObj.Add("RetCode", retcode);
            nativeObj.Add("RetErrMsg", reterrmsg);
            nativeObj.Add("NonceStr", CommonUtil.CreateNoncestr());
            nativeObj.Add("AppSignature", GetBizSign(nativeObj));
            nativeObj.Add("SignMethod", SignType);
            return CommonUtil.ArrayToXml(nativeObj);
        }

        /// <summary>
        /// ���������ɹ� Package (�Ѳ���ͨ��)
        /// </summary>
        /// <returns></returns>
        public string CreateDeliverNotifyPackage(string openId, string transId, string out_trade_no)
        {
            Dictionary<string, string> nativeObj = new Dictionary<string, string>();

            nativeObj.Add("appid", AppId);
            nativeObj.Add("openid", openId);
            nativeObj.Add("transid", transId);
            nativeObj.Add("out_trade_no", out_trade_no);
            nativeObj.Add("deliver_timestamp", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            nativeObj.Add("deliver_status", "1");
            nativeObj.Add("deliver_msg", "ok");
            nativeObj.Add("app_signature", GetBizSign(nativeObj));
            nativeObj.Add("sign_method", "sha1");

            var entries = nativeObj.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));

            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        #endregion

        /// <summary>
        /// ��֤ package��ǩ��(MD5) ���Ѳ���ͨ����
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public bool ValidateMD5Signature(Dictionary<string, string> dic, string sign)
        {
            //��֤ ��������Ϊ��
            foreach (var item in dic)
            {
                if (string.IsNullOrEmpty(item.Value))
                    return false;
            }

            if ("" == PartnerKey)
            {
                throw new SDKRuntimeException("��Կ����Ϊ�գ�");
            }
            string unSignParaString = CommonUtil.FormatBizQueryParaMap(dic,
                    false);

            return MD5SignUtil.VerifySignature(unSignParaString, sign, PartnerKey);
        }

        /// <summary>
        /// ��֤֧��ǩ��(Sha1) �Ѳ���ͨ��
        /// </summary>
        /// <param name="dic">�μ�ǩ�����ֵ�����</param>
        /// <param name="sign">ǩ��ֵ</param>
        /// <returns></returns>
        public bool ValidateSha1Signature(Dictionary<string, string> dic, string sign)
        {
            string appSignature = GetBizSign(dic);
            return appSignature == sign;
        }
    }
}
