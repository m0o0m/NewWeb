using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class ADBLL
    {
        public static int Add(ADInfo adInfo)
        {
            return ADDAL.Add(adInfo);
        }

        public static int AddClickInfo(ADInfo adInfo)
        {
            return ADDAL.AddClickInfo(adInfo);
        }

        public static int Add(DMModel model) {
            return ADDAL.Add(model);
        }

        public static int Update(DMModel model)
        {
            return ADDAL.Update(model);
        }


        public static DMModel GetDMModel(string appkey)
        {
            return ADDAL.GetDMModel(appkey);
        }

        public static DMModel GetDMModel(DMModel model)
        {
            return ADDAL.GetDMModel(model);
        }
        public static DMModel GetDMReapeatModel(DMModel model)
        {
            return ADDAL.GetDMReapeatModel(model);
        }

        public static int Delete(DMModel model)
        {
            return ADDAL.Delete(model);
        }

    }
}
