using GL.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GL.Data.Model;

namespace GL.Data.BLL
{
    public class GateInfoBLL
    {
        public static IEnumerable<GateInfo> GetModel()
        {

            return GateInfoDAL.GetModel();

        }

        public static GateInfo GetModelByID(int ID)
        {

            return GateInfoDAL.GetModelByID(ID);

        }

        public static IEnumerable<GateInfo> GetModelByIDArray(IEnumerable<int> ID)
        {
            return GateInfoDAL.GetModelByIDArray(ID);
        }
    }
}
