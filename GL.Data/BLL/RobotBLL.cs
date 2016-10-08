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
using GL.Data.View;

namespace GL.Data.BLL
{
    public class RobotBLL
    {
      

        public static IEnumerable<PotGold> GetPotGoldList() {
            return DAL.RobotDAL.GetPotGoldList();
        }


        public static PotGold GetPotGoldByType(int gameType) {
            return DAL.RobotDAL.GetPotGoldByType(gameType);
        }


        public static IEnumerable<RobotOutPut> GetRobotOutPutList(GameRecordView vbd)
        {
            return DAL.RobotDAL.GetRobotOutPutList(vbd);
        }


    }
}
