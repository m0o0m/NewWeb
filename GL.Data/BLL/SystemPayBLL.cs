using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GL.Data.BLL
{
    public class SystemPayBLL
    {
        public static List<GL.Data.Model.SystemPay> GetSystemPay(string sTime, string eTime, int channels = 0)
        {
            return DAL.SystemPayDAL.GetSystemPay(sTime, eTime, channels);
        }

        public static List<GL.Data.Model.SystemExpend> GetSystemSystemExpend(string sTime, string eTime, int channels = 0)
        {
            return DAL.SystemPayDAL.GetSystemSystemExpend(sTime, eTime, channels);
        }

        public static void PaiJuAfter()
        {
             DAL.SystemPayDAL.PaiJuAfter();
        }

        public static void ShuihuAfter(string startTime,string endTime) {
            DAL.SystemPayDAL.ShuihuAfter(startTime,endTime);
        }
    }
}
