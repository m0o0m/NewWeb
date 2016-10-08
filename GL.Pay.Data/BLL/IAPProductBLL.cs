using GL.Data.Model;
using GL.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GL.Data.BLL
{
    public class IAPProductBLL
    {
        public static IAPProduct GetModelByID(string ID)
        {

            return IAPProductDAL.GetModelByID(ID);

        }
        public static IAPProduct GetModelByIDForIOS(string ID)
        {

            return IAPProductDAL.GetModelByIDForIOS(ID);

        }
    }
}
