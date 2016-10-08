using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.XY
{
    public class XYKeyPackage
    {
        public string appkey { get; set; }
        public string paykey { get; set; }
    }

    public class XYConfig
    {
        #region 字段


        private static Dictionary<string, XYKeyPackage> keyList;

        static XYConfig()
        {
            keyList = new Dictionary<string, XYKeyPackage>();
            keyList.Add("100026254", new XYKeyPackage { appkey = "LLpoXLyikrfQuQOoV0rCxRD6JGB79wv1", paykey = "QOLRfWEwi1cnbuFTxvbs2cg3nS95vAHF" });

        }

        #endregion


        public static XYKeyPackage GetKey(string key)
        {
            return keyList[key];
        }

    }
}
