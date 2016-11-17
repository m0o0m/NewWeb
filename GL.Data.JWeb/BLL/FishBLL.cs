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
    public class FishBLL
    {
        public static IEnumerable<FishInfoRecord> GetFishInfoRecord(int userid,int pageIndex)
        {
            IEnumerable<FishInfoRecord> res = FishDAL.GetFishInfoRecord(userid, pageIndex);
            return res;
        }

    }
}
