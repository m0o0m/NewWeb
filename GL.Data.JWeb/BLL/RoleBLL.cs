using GL.Data.DAL;
using GL.Data.JWeb.DAL;
using GL.Data.JWeb.Model;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GL.Data.JWeb.BLL
{
    public class RoleBLL
    {
        public static string GetNickNameByID(int userid)
        {
            SingleData res = RoleDAL.GetNickNameByID(userid);
            if (res == null)
            {
                return null;
            }
            else {
                return res.ObjData;
            }
        }
    }
}
