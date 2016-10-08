using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.BLL
{
    public class ActiveBLL
    {
        public static IEnumerable<Roulette> GetRouletteDataDetail(BaseDataView bdv)
        {
            return DAL.ActiveDAL.GetRouletteDataDetail(bdv);
        }
    }
}
