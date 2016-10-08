using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class CarGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public decimal RoundID { get; set; }
        public CarType CarID { get; set; }

      
    
        public string UserData { get; set; }

        public Int64 BoardTime { get; set; }

    }

  

    public enum CarType {
        法拉利 = 0,
        奔驰 = 1,
        兰博基尼 = 2,
        宝马 = 3,
        玛莎拉蒂 = 4,
        雷克萨斯 = 5,
        保时捷 =6,
        大众 = 7
    }


    public class CarGameRecordMethod
    {
        public static string TransPositionTo(string n)
        {
            switch (n)
            {
                case "0":
                    return "法拉利";

                case "1":
                    return "奔驰";
                case "2":
                    return "兰博基尼";
                case "3":
                    return "宝马";
                case "4":
                    return "玛莎拉蒂";
                case "5":
                    return "雷克萨斯";
                case "6":
                    return "保时捷";
                case "7":
                    return "大众";
              
                default:
                    return "";
            }
        }


        public static string TransToBL(string n) {
            switch (n)
            {
                case "0":
                    return "(40倍)";

                case "1":
                    return "(5倍)";
                case "2":
                    return "(30倍)";
                case "3":
                    return "(5倍)";
                case "4":
                    return "(20倍)";
                case "5":
                    return "(5倍)";
                case "6":
                    return "(10倍)";
                case "7":
                    return "(5倍)";

                default:
                    return "";
            }
        }

    }



}
