using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Common;
using GL.Data.Model;
using Webdiyer.WebControls.Mvc;
using GL.Data.View;
namespace GL.Data.BLL
{
    public class MemberCenterBLL
    {

        public static PagedList<Role> GetAllListByPage(int page, string userID)
        {
            return DAL.MemberCenterDAL.GetAllListByPage(page, userID);
        }

        public static PagedList<Role> GetDataForAbnormalUser(int page, string type)
        {
            return DAL.MemberCenterDAL.GetDataForAbnormalUser(page, type);
        }


        public static PagedList<BlackList> GetListByPageForMac(int page, ValueView vv)
        {
            return DAL.MemberCenterDAL.GetListByPageForMac(page, vv);
        }


        public static PagedList<BlackList> GetListByPageFor(int page, ValueView vv)
        {
            return DAL.MemberCenterDAL.GetListByPageFor(page, vv);
        }


        public static int GetSendEmailCount(string id)
        {
            return DAL.MemberCenterDAL.GetSendEmailCount(id);
        }

        public static int UpdateRemarksname(string id, string name)
        {
            return DAL.MemberCenterDAL.UpdateRemarksname(id, name);
        }

        public static string GetRemarksNameByID(string id)
        {
            return DAL.MemberCenterDAL.GetRemarksNameByID(id);
        }

        public static int UpdateNickName(string id, string name)
        {
            return DAL.MemberCenterDAL.UpdateNickName(id, name);
        }

        public static long GetScaleForZyinkui(int zID)
        {
            return DAL.MemberCenterDAL.GetScaleForZyinkui(zID);
        }

        public static long GetListForZodiacyinkui(int zID)
        {
            return DAL.MemberCenterDAL.GetListForZodiacyinkui(zID);
        }

        public static long GetListForCaryinkui(int zID)
        {
            return DAL.MemberCenterDAL.GetListForCaryinkui(zID);
        }

        public static long GetListForHundredyinkui(int zID)
        {
            return DAL.MemberCenterDAL.GetListForHundredyinkui(zID);
        }

        public static PagedList<TexasGameRecord> GetListByPageForTexas(GameRecordView model)
        {
            return DAL.MemberCenterDAL.GetListByPageForTexas(model);
        }


        public static PagedList<ScaleGameRecord> GetListByPageForScale(GameRecordView model)
        {
            return DAL.MemberCenterDAL.GetListByPageForScale(model);
        }
    }
}
