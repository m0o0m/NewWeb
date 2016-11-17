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
    public class FruitGameBLL
    {
        public static IEnumerable<FruitGameExplodeConfig> GetExplodeConfig()
        {
            return DAL.FruitGameDAL.GetExplodeConfig();
        }

        public static IEnumerable<FruitBibeiConfig> GetBiBeiConfig()
        {
            return DAL.FruitGameDAL.GetBiBeiConfig();
        }


        public static IEnumerable<FruitPotConfig> GetFruitPotConfig(string platid,int gameid)
        {
            return DAL.FruitGameDAL.GetFruitPotConfig(platid, gameid);
        }


        public static int UpdateExplodeConfig(FruitGameExplodeConfig models)
        {
            return DAL.FruitGameDAL.UpdateExplodeConfig(models);
        }


        public static int UpdateFruitBibei(List<FruitBibeiConfig> models)
        {
            return DAL.FruitGameDAL.UpdateFruitBibei(models);
        }

        public static int UpdateFruitPotConfig(List<FruitPotConfig> models)
        {
            return DAL.FruitGameDAL.UpdateFruitPotConfig(models);
        }

    }
}
