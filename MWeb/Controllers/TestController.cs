using GL.Data.BLL;
using GL.Data.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace MWeb.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            //GL.Data.Model.Role role = RoleBLL.GetModelByID(new GL.Data.Model.Role() { ID = 34801 });
            
            ////  KRoleBaseInfo f= MarshalExtend.GetObject<KRoleBaseInfo>(data, data.Count());
            //KRoleBaseInfo f = new KRoleBaseInfo();
            //f.nVersion = 2;
            //f.nRemainTime = 23324;
            //byte[] bs = MarshalExtend.GetData(f);
            //byte[] data = bs;
            //KRoleBaseInfo ff = MarshalExtend.GetObject<KRoleBaseInfo>(data, data.Count());

          
            ////713

            //byte[] data2 = role.BaseInfo;

            //byte[] data3 = role.ExtInfo;

            //KRoleBaseInfo f2 = MarshalExtend.GetObject<KRoleBaseInfo>(data2, data2.Count());

            return View();
        }
    }






    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct QianShou
    {

        /// BYTE[5]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
        public byte[] cbStatus;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct YellowVipAward
    {

        /// BYTE->unsigned char
        public byte cbFirstGet;

        /// time_t->__time32_t->int
        public int lastLoginTime_normal;

        /// time_t->__time32_t->int
        public int lastLoginTime_year;
    }



    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct KRoleBaseInfo
    {
   
        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string szVerifyInfo;

        /// char[256]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szCarryInfo;

        /// BYTE->unsigned char
        public byte bySex;

        /// WORD->unsigned short
        public ushort wFaceID;

        /// char[256]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szFaceUrl;

        /// DWORD->unsigned int
        public uint dwTelFare;

        /// DWORD->unsigned int
        public uint dwCard;

        /// DWORD->unsigned int
        public uint dwAppleReview;

        /// BYTE->unsigned char
        public byte byFirstLogin;

        /// BYTE->unsigned char
        public byte byFirstRecharge;

        /// DWORD->unsigned int
        public uint dwHorn;

        /// DWORD->unsigned int
        public uint dwShowGift;

        /// DWORD->unsigned int
        public uint dwMaxNoble;

        /// char[120]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 120)]
        public string zPersonSign;

        /// time_t->__time32_t->int
        public int nLastGetFreeGoldTime;

        /// time_t->__time32_t->int
        public int nRemainTime;

        /// WORD->unsigned short
        public ushort wGetFreeGoldCount;

        /// QianShou
        public QianShou qianShouInfo;

        /// time_t->__time32_t->int
        public int continueLoginTime;

        /// time_t->__time32_t->int
        public int lastLoginTime;

        /// YellowVipAward
        public YellowVipAward yellowVipGet;

        public int nVersion;
    }


    static class MarshalExtend
    {
        public static T GetObject<T>(byte[] data, int size)
        {
          
            IntPtr pnt = Marshal.AllocHGlobal(size);

            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(data, 0, pnt, size);
                return (T)Marshal.PtrToStructure(pnt, typeof(T));
            }
           
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }
        }

        public static byte[] GetData(object obj)
        {
            var size = Marshal.SizeOf(obj.GetType());
            var data = new byte[size];
            IntPtr pnt = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(obj, pnt, true);
                // Copy the array to unmanaged memory.
                Marshal.Copy(pnt, data, 0, size);
                return data;
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }
        }

        public static T ReadMarshal<T>(this System.IO.BinaryReader reader)
        {
            var length = Marshal.SizeOf(typeof(T));
            var data = reader.ReadBytes(length);
            return GetObject<T>(data, data.Length);
        }

        public static void WriteMarshal<T>(this System.IO.BinaryWriter writer, T obj)
        {
            writer.Write(GetData(obj));
        }
    }
}