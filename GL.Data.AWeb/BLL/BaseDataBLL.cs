using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Data.Model;
namespace GL.Data.BLL
{
    public class BaseDataBLL
    {
        public static IEnumerable<BaseDataInfo> GetRegisteredUsers(BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetRegisteredUsers(bdv);
        }


        public static IEnumerable<BaseDataInfo> GetActiveUsers(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetActiveUsers(vbd);
        }

        public static IEnumerable<BaseDataInfoForOnlinePlay> GetOnlinePlay(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetOnlinePlay(vbd);
        }
        public static int GetAllPlayCount(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetAllPlayCount(vbd);
        }
        public static object GetAllUser(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetAllUser(vbd);
        }
        public static IEnumerable<BaseDataInfo> GetPlayCount(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetPlayCount(vbd);
        }
        public static IEnumerable<BaseDataInfoForRetentionRates> GetRetentionRates(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetRetentionRates(vbd);
        }



        public static IEnumerable<BaseDataInfoForBankruptcyRate> GetBankruptcyRate(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetBankruptcyRate(vbd);
        }


        public static IEnumerable<QQZoneRechargeCount> GetQQZoneRechargeCount(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetQQZoneRechargeCount(vbd);
        }

        public static IEnumerable<GameOutput> GetGameOutput(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameOutput(vbd);
        }


        public static GameOutputDetail GetGameOutputDetail(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameOutputDetail(vbd);
        }
        public static IEnumerable<GameOutput> GetGameOutput2(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameOutput2(vbd);
        }

        public static GameOutputDetail GetGameOutputDetail2(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameOutputDetail2(vbd);
        }

        public static IEnumerable<PotRecord> GetPotRecord()
        {
            return DAL.BaseDataDAL.GetPotRecord();
        }
        public static IEnumerable<PotRecord> GetPotRecord(int Num)
        {
            return DAL.BaseDataDAL.GetPotRecord(Num);
        }

        public static IEnumerable<JiFen> GetScoreboard(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetScoreboard(vbd);
        }

        public static IEnumerable<OpenFuDai> GetOpenFuDai(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetOpenFuDai(vbd);
        }



        public static BaseDataInfoForUsersDiamondDistributionRatio GetUsersDiamondDistributionRatio()
        {
            return DAL.BaseDataDAL.GetUsersDiamondDistributionRatio();
        }

        public static BaseDataInfoForUsersGoldDistributionRatio GetUsersGoldDistributionRatio()
        {
            return DAL.BaseDataDAL.GetUsersGoldDistributionRatio();
        }




        public static IEnumerable<BaseDataInfoForPotRakeback> GetPotRakeback(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetPotRakeback(vbd);
        }

        public static BaseDataInfoForVIPDistributionRatio GetVIPDistributionRatio()
        {
            return DAL.BaseDataDAL.GetVIPDistributionRatio();
        }

        public static IEnumerable<BaseDataInfo> GetNoviceTask(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetNoviceTask(vbd);
        }

    }
}
