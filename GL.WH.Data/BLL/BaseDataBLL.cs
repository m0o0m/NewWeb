using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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




        public static IEnumerable<BaseDataInfoForBankruptcyRate> GetBankruptcyRate(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetBankruptcyRate(vbd);
        }



        public static IEnumerable<BaseDataInfoForDailyOutput> GetDailyOutput(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetDailyOutput(vbd);
        }
    }
}
