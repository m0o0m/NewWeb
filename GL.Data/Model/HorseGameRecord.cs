using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class HorseGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public decimal RoundID { get; set; }

        public string RankID { get; set; }

        public string Obbs { get; set; }
     
        public string UserData { get; set; }

    }

    public class HorseGameRecordMethod {
        public static string TransPositionTo(string n) {
            switch (n) {
                case "0":
                    return "1-2";
                  
                case "1":
                    return "1-3";
                case "2":
                    return "1-4";
                case "3":
                    return "1-5";
                case "4":
                    return "1-6";
                case "5":
                    return "2-3";
                case "6":
                    return "2-4";
                case "7":
                    return "2-5";
                case "8":
                    return "2-6";
                case "9":
                    return "3-4";
                case "10":
                    return "3-5";
                case "11":
                    return "3-6";
                case "12":
                    return "4-5";
                case "13":
                    return "4-5";
                case "14":
                    return "5-6";
                case "15":
                    return "单";
                case "16":
                    return "双";
                case "17":
                    return "1";
                case "18":
                    return "2";
                case "19":
                    return "3";
                case "20":
                    return "4";
                case "21":
                    return "5";
                case "22":
                    return "6";
                default:
                    return "";
            }
        }

    }


}
