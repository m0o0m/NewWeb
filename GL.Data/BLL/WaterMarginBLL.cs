using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Common;
using GL.Data.DAL;
using Webdiyer.WebControls.Mvc;
using GL.Command.DBUtility;

namespace GL.Data.BLL
{
    public class WaterMarginBLL
    {
        public static IEnumerable<WaterMargin> GetWaterMarginList()
        {
            return DAL.WaterMarginDAL.GetWaterMarginList();
        }


        public static IEnumerable<SSwitch> GetSSwitchList(string id)
        {
            return DAL.WaterMarginDAL.GetSSwitchList(id);
        }

        

        public static IEnumerable<ArcadeGameStock> GetArcadeGameStockList(string id)
        {
            return DAL.WaterMarginDAL.GetArcadeGameStockList(id);
        }

        public static int UpdateWatermargin(List<WaterMargin> models) {
            return DAL.WaterMarginDAL.UpdateWatermargin(models);
        }


        public static int UpdateSSwitch(List<SSwitch> models)
        {
            return DAL.WaterMarginDAL.UpdateSSwitch(models);
        }

        public static int UpdateArcadeGameStock(List<ArcadeGameStock> models)
        {
            return DAL.WaterMarginDAL.UpdateArcadeGameStock(models);
        }


        //MarryConfig
        public static MarryConfig GetMarryConfig(int id)
        {
            return DAL.WaterMarginDAL.GetMarryConfig(id);
        }

        public static IEnumerable<BiBeiConfig>  GetBiBeiConfig() {
            return DAL.WaterMarginDAL.GetBiBeiConfig();
        }


        public static Int64 GetHuiShou(string createtime)
        {
            return DAL.WaterMarginDAL.GetHuiShou(createtime);
        }


        public static Int64 GetComHuiShou(string createtime,int recordType)
        {
            return DAL.WaterMarginDAL.GetComHuiShou(createtime, recordType);
        }

        public static int UpdateMary(int id, Int64 maryUPLimit, Int64 maryDownLimit, Int64 bibei1, Int64 bibei2, Int64 bibei3, Int64 bibei4, Int64 bibei5, Int64 bibein)
        {
            return DAL.WaterMarginDAL.UpdateMary(id, maryUPLimit, maryDownLimit, bibei1, bibei2, bibei3, bibei4,  bibei5, bibein);
        }

    }
}
