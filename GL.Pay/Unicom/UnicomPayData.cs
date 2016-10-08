using GL.Pay.AliPay;
using LitJson;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GL.Pay.Unicom
{
    public class UnicomPayData
    {
        private ILog log = LogManager.GetLogger("UnicomPayData");
        public UnicomPayData()
        {

        }

        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

        /**
        * 设置某个字段的值
        * @param key 字段名
         * @param value 字段值
        */
        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        /**
        * 根据字段名获取某个字段的值
        * @param key 字段名
         * @return key对应的字段值
        */
        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        /**
         * 判断某个字段是否已设置
         * @param key 字段名
         * @return 若字段key已被设置，则返回true，否则返回false
         */
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        /**
        * @将Dictionary转成xml
        * @return 经转换得到的xml串
        * @throws WxPayException
        **/
        public string ToXml()
        {
            //数据为空时不能转化为xml格式
            if (0 == m_values.Count)
            {
                throw new Exception("UnicomPayData数据为空!");
            }

            string xml = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    throw new Exception("UnicomPayData内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    throw new Exception("UnicomPayData字段数据类型错误!");
                }
            }
            //xml += "</xml>";
            return xml;
        }

        public string ToXmlForCheck()
        {
            //数据为空时不能转化为xml格式
            if (0 == m_values.Count)
            {
                throw new Exception("UnicomPayData数据为空!");
            }

            string xml = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?><paymessages>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    throw new Exception("UnicomPayData内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    throw new Exception("UnicomPayData字段数据类型错误!");
                }
            }
            xml += "</paymessages>";
            return xml;
        }


        /**
        * @将xml转为UnicomPayData对象并返回对象内部的数据
        * @param string 待转换的xml串
        * @return 经转换得到的Dictionary
        * @throws WxPayException
        */
        public SortedDictionary<string, object> FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new Exception("将空的xml串转换为UnicomPayData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            //XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            //<?xml version="1.0" encoding="UTF-8"?><callbackReq><orderid>UniPay020160119150726721</orderid><ordertime>20160119150729</ordertime><cpid>91007740</cpid><appid>90810001529720151105095126335700</appid><fid>00012243</fid><consumeCode>90810001529720151105095126335700016</consumeCode><payfee>10</payfee><payType>1</payType><hRet>0</hRet><status>00000</status><signMsg>2915a659aafa66b2519f401f0915797b</signMsg></callbackReq>

            //<?xml version="1.0" encoding="UTF-8"?><checkOrderIdReq><orderid>UniPay020160120142812799</orderid><signMsg>4d8d5ee65e83e77bf085161d6cf8c0cf</signMsg><usercode></usercode><provinceid></provinceid><cityid></cityid></checkOrderIdReq>

            XmlNode xmlNode = xmlDoc.DocumentElement;
            string resName = xmlNode.Name;

            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//获取xml的键值对到UnicomPayData内部的数据中
            }
            try
            {
                //2015-06-29 错误是没有签名
                switch (resName)
                {
                    case "callbackReq":
                        CheckSign(resName);//验证签名,不通过会抛异常
                        break;
                    case "checkOrderIdReq":
                        CheckSign(resName);//验证签名,不通过会抛异常
                        break;
                    default:
                        return m_values;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return m_values;
        }

        /**
        * @Dictionary格式转化成url参数格式
        * @ return url格式串, 该串不包含sign字段值
        */
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "UnicomPayData内部含有值为null的字段!");
                    throw new Exception("UnicomPayData内部含有值为null的字段!");
                }

                if (pair.Key != "signMsg" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }


        /**
        * @Dictionary格式化成Json
         * @return json串数据
        */
        public string ToJson()
        {
            string jsonStr = JsonMapper.ToJson(m_values);
            return jsonStr;
        }

        /**
        * @values格式化成能在Web页面上显示的结果（因为web页面上不能直接输出xml格式的字符串）
        */
        public string ToPrintStr()
        {
            string str = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "UnicomPayData内部含有值为null的字段!");
                    throw new Exception("UnicomPayData内部含有值为null的字段!");
                }

                str += string.Format("{0}={1}<br>", pair.Key, pair.Value.ToString());
            }
            Log.Debug(this.GetType().ToString(), "Print in Web Page : " + str);
            return str;
        }

        /**
        * @生成签名，详见签名生成算法
        * @return 签名, sign字段不参加签名
        */
        public string MakeSign(string resName)
        {
            //转url格式
            //string str = ToUrl();
            string str = "";
            switch (resName)
            {
                case "callbackReq":
                    str = string.Concat("orderid=", m_values["orderid"], "&ordertime=", m_values["ordertime"], "&cpid=", m_values["cpid"], "&appid=", m_values["appid"], "&fid=", m_values["fid"], "&consumeCode=", m_values["consumeCode"], "&payfee=", m_values["payfee"], "&payType=", m_values["payType"], "&hRet=", m_values["hRet"], "&status=", m_values["status"]);
                    break;
                case "checkOrderIdReq":
                    str = string.Concat("orderid=", m_values["orderid"]);
                    break;
                default:
                    break;
            }

            //在string后加入API KEY
            str += "&Key=" + UnicomConfig.KEY;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToLower();
        }

        /**
        * 
        * 检测签名是否正确
        * 正确返回true，错误抛异常
        */
        public bool CheckSign(string resName)
        {
            //如果没有设置签名，则跳过检测
            if (!IsSet("signMsg"))
            {
                throw new Exception("UnicomPayData签名存在但不合法!");
            }
            //如果设置了签名但是签名为空，则抛异常
            else if (GetValue("signMsg") == null || GetValue("signMsg").ToString() == "")
            {
                throw new Exception("UnicomPayData签名存在但不合法!");
            }
            //获取接收到的签名
            string return_sign = GetValue("signMsg").ToString();
            //在本地计算新的签名
            string cal_sign = MakeSign(resName);
            if (cal_sign == return_sign)
            {
                return true;
            }

            throw new Exception("UnicomPayData签名验证错误!");
        }

        /**
        * @获取Dictionary
        */
        public SortedDictionary<string, object> GetValues()
        {
            return m_values;

        }
    }
}
