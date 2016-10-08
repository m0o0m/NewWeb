using GL.Common;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Pay.AliPay;
using GL.Pay.QQPay;
using GL.Pay.WxPay;
using GL.Pay.YeePay;
using GL.Protocol;
using log4net;
using ProtoCmd.BackRecharge;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Pay.JsonMapper;
using Web.Pay.Models;
using Web.Pay.protobuf.SCmd;
using System.Collections;
using GL.Pay.Baidu;
using Newtonsoft.Json;
using GL.Pay.Unicom;
using GL.Pay.XY;
using GL.Pay.Hippocampi;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Data;
using GL.Pay.ZY;
using GL.Pay.YYH;
using System.Web.Script.Serialization;
using GL.Pay.MeiZu;
using System.Threading;

namespace Web.Pay.Controllers
{
    public class PayMoney
    {
        public  string PayTest(int UserID, uint gold, uint dia,uint rmb,bool firstGif,string billNO, int iF2, int myType) {


            return "1";


        
        }
    }
}