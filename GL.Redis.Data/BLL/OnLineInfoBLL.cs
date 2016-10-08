using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.BLL
{
    public class OnLineInfoBLL
    {
        public static string GetNewJson()
        {
            return OnLineInfoDAL.GetNewJson();
        }

        public static int InsertNewJson(OnLineInfo info)
        {
            return OnLineInfoDAL.InsertNewJson(info);

        }
    }
}
