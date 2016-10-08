using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class OnModel
    {
        public long ID { get; set; }

        public string Account { get; set; }

        public string NickName { get; set; }

        public string Type { get; set; }

        public static string GetGameType(string value ) {
            string[] strs = value.Trim('|').Split('|');
            string res = "";
            ////    斗地主 = 12,
            ////    中发白 = 13,
            ////    十二生肖 = 14,
            ////    德州扑克 = 15
            ////}

            for (int i = 0; i < strs.Length; i++) {
                string str = strs[i];
                switch (str) {
                    case "12":
                        res += "斗地主,";
                        break;
                    case "13":
                        res += "中发白,";
                        break;
                    case "14":
                        res += "十二生肖,";
                        break;
                    case "15":
                        res += "德州扑克,";
                        break;
                    case "16":
                        res += "小马快跑,";
                        break;
                    case "17":
                        res += "奔驰宝马,";
                        break;
                    default:
                        res += (res+",");
                        break;
                }
            }

            return res.Trim(',');
        }
    }
}
