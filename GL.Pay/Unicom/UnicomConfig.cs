using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.Unicom
{
    public class UnicomConfig
    {
        #region 字段
        private static string _CPID = "test";
        private static string _KEY = "test";
        private static string _APP_ID = "test";
        private static string _APP_NAME = "test";
        private static string _APP_DEVELOPER = "test";
        private static string _CHANNEL_ID = "test";

        private static Dictionary<string, string> serviceidList; 

        static UnicomConfig()
        {
            _CPID = "91007740";
            _KEY = "2b57f1d74fb9f1b61d12";
            _APP_ID = "90810001529720151105095126335700";
            _APP_NAME = "515德州扑克";
            _APP_DEVELOPER = "深圳市五一五游戏网络有限公司";
            _CHANNEL_ID = "";

            serviceidList = new Dictionary<string, string>();
            serviceidList.Add("5b_1", "151123154828");
            serviceidList.Add("5b_2", "151123154829");
            serviceidList.Add("5b_3", "151123154830");
            serviceidList.Add("5b_4", "151123154831");
            serviceidList.Add("5b_5", "151123154832");
            serviceidList.Add("5b_6", "151123154833");
            serviceidList.Add("Chip_1", "151123154834");
            serviceidList.Add("Chip_2", "151123154835");
            serviceidList.Add("Chip_3", "151123154836");
            serviceidList.Add("Chip_4", "151123154837");
            serviceidList.Add("Chip_5", "151123154838");
            serviceidList.Add("Chip_6", "151123154839");


            serviceidList.Add("5b_7", "151123154840");
            serviceidList.Add("5b_8", "151123154841");
            serviceidList.Add("5b_9", "151123154842");
            serviceidList.Add("5b_10", "151123154843");
            serviceidList.Add("5b_11", "151123154844");
            serviceidList.Add("5b_12", "151123154845");
            serviceidList.Add("5b_13", "151123154846");
            serviceidList.Add("5b_14", "151123154847");
            serviceidList.Add("5b_15", "151123154848");
            serviceidList.Add("5b_16", "151123154849");
            serviceidList.Add("5b_17", "151123154850");
            serviceidList.Add("5b_18", "151123154851");
            serviceidList.Add("5b_19", "151123154852");


            serviceidList.Add("Chip_7", "151123154853");
            serviceidList.Add("Chip_8", "151123154854");
            serviceidList.Add("Chip_9", "151123154855");
            serviceidList.Add("Chip_10", "151123154856");
            serviceidList.Add("Chip_11", "151123154857");
            serviceidList.Add("Chip_12", "151123154858");
            serviceidList.Add("Chip_13", "151123154859");
            serviceidList.Add("Chip_14", "151123154860");
            serviceidList.Add("Chip_15", "151123154861");
            serviceidList.Add("Chip_16", "151123154862");
            serviceidList.Add("Chip_17", "151123154863");
            serviceidList.Add("Chip_18", "151123154864");
            serviceidList.Add("Chip_19", "151123154865");
            serviceidList.Add("Chip_20", "151123154866");
         




            serviceidList.Add("firstCharge_1", "160111505947");
            serviceidList.Add("firstCharge_2", "160111505948");
            serviceidList.Add("firstCharge_3", "160111505949");
            serviceidList.Add("firstCharge_4", "160111505950");

        }

        #endregion


        public static string GetServiceid(string key)
        {
            return serviceidList[key];
        }

        public static string CPID
        {
            get { return _CPID; }
            set { _CPID = value; }
        }

    
        //计费密钥
        public static string KEY
        {
            get { return _KEY; }
            set { _KEY = value; }
        }



        //沃商店应用id
        public static string APP_ID
        {
            get { return _APP_ID; }
            set { _APP_ID = value; }
        }

        //应用名称
        public static string APP_NAME
        {
            get { return _APP_NAME; }
            set { _APP_NAME = value; }
        }


        //开发者名称
        public static string APP_DEVELOPER
        {
            get { return _APP_DEVELOPER; }
            set { _APP_DEVELOPER = value; }
        }


        //渠道号
        public static string CHANNEL_ID
        {
            get { return _CHANNEL_ID; }
            set { _CHANNEL_ID = value; }
        }

    }
}
