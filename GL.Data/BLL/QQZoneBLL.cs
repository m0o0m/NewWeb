using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.BLL
{
    public class QQZoneBLL
    {
        public static int Add(QQZone model)
        {
            return DAL.QQZoneDAL.Add(model);
        }

    }
}
