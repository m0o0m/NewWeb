using GL.Common;
using GL.Data;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using GL.Protocol;
using MWeb.protobuf.SCmd;
using ProtoCmd.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Net;
using ThoughtWorks.QRCode.Codec;

namespace MWeb.Controllers
{
    public class FruitMachinesController : Controller
    {
        // GET: FruitMachines
        public ActionResult Index()
        {
            return View();
        }

        [QueryValues]
        /// <summary>
        /// 水果机控制台首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigIndex(Dictionary<string, string> queryvalues) {

            string par = queryvalues.ContainsKey("par") ? queryvalues["par"].Trim(',') : "";
            string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : "";
            string operType = queryvalues.ContainsKey("operType") ? queryvalues["operType"] : "";
            if (par.Contains("'"))
            {
                return Content("-4");
            }
            switch (operType)
            {
                case "1": //说明是保存配置1，2，3
                          //type说明是保存的配置
                    string[] pars = par.Trim(',').Split('|');
           
                    string[] par1 = pars[0].Split(',');
                    string[] par2 = pars[1].Split(',');
                    string[] par3 = pars[2].Split(',');
                    string[] par4 = pars[3].Split(',');
                    string[] par5 = pars[4].Split(',');
                    string[] par6 = pars[5].Split(',');
                    string[] par7 = pars[6].Split(',');

                    FruitGameExplodeConfig modelExp = new FruitGameExplodeConfig()
                    {
                        Normal = Convert.ToInt32(par1[0]),
                        Lucky = Convert.ToInt32(par1[1]),
                        Random = Convert.ToInt32(par1[2]),
                        SmallThree = Convert.ToInt32(par1[3]),


                        BigThree = Convert.ToInt32(par2[0]),
                        Bigfour = Convert.ToInt32(par2[1]),
                        Zong = Convert.ToInt32(par2[2]),
                        TianNv = Convert.ToInt32(par2[3]),


                        TianLong = Convert.ToInt32(par3[0]),
                        JiuBao = Convert.ToInt32(par3[1]),
                        GrandSlam = Convert.ToInt32(par3[2]),
                        OpenTrain = Convert.ToInt32(par3[3]),


                        SmallApple = Convert.ToInt32(par4[0]),
                        SmallOrange = Convert.ToInt32(par4[1]),
                        SmallMango = Convert.ToInt32(par4[2]),
                        SmallRing = Convert.ToInt32(par4[3]),

                        SmallWatermalon = Convert.ToInt32(par5[0]),
                        SmallDoubleStar = Convert.ToInt32(par5[1]),
                        SmallDoubleSeven = Convert.ToInt32(par5[2]),
                        Apple = Convert.ToInt32(par5[3]),


                        Orange = Convert.ToInt32(par6[0]),
                        Mango = Convert.ToInt32(par6[1]),
                        Ring = Convert.ToInt32(par6[2]),
                        Watermalon = Convert.ToInt32(par6[3]),


                        DoubleStar = Convert.ToInt32(par7[0]),
                        DoubleSeven = Convert.ToInt32(par7[1]),
                        SmallBar = Convert.ToInt32(par7[2]),
                        BigBar = Convert.ToInt32(par7[3]),
                        Type = Convert.ToInt32(type),
                        CreateTime = DateTime.Now.ToString()
                    };




                    int res = FruitGameBLL.UpdateExplodeConfig(modelExp);
                    if (res > 0)
                    {//修改成功
                     // return Content("1");

                        int callback = MessageSlot();
                        return Content(callback.ToString());
                    }
                    else
                    {
                        return Content("-3");
                    }
                case "2":  //说明是修改比倍

                    List<FruitBibeiConfig> modelList = new List<FruitBibeiConfig>();

                    string[] parsbibei = par.Trim(',').Split('|');
                    for (int i = 0; i <= parsbibei.Length - 1; i++)
                    {
                        FruitBibeiConfig m = new FruitBibeiConfig();
                        string[] ps = parsbibei[i].Split(',');
                        m.SeasonID = i + 1;
                        m.GameNum1 = Convert.ToInt32( Convert.ToDouble(ps[0] ) * 1000);
                        m.GameNum2 = Convert.ToInt32(Convert.ToDouble(ps[1]) * 1000) ;
                        m.GameNum3 = Convert.ToInt32(Convert.ToDouble(ps[2]) * 1000) ;
                        m.GameNumn = Convert.ToInt32(Convert.ToDouble(ps[3]) * 1000) ;
                        modelList.Add(m);
                    }


                    int resbibei = FruitGameBLL.UpdateFruitBibei(modelList);
                    if (resbibei > 0)
                    {//修改成功
                     // return Content("1");

                        int callback = MessageSlot();
                        return Content(callback.ToString());
                    }
                    else
                    {
                        return Content("-3");
                    }
                case "3": //修改奖池

                    IEnumerable<FruitPotConfig> potDBData = FruitGameBLL.GetFruitPotConfig("1,2,3,4,5,6", 20);
                    string potStr = "";
                    string[] parsPot = par.Split('|');
                    List<FruitPotConfig> modelListPot = new List<FruitPotConfig>();
                    for (int i = 0; i <= parsPot.Length - 1; i++)
                    {
                        string[] ite = parsPot[i].Split(',');

                        FruitPotConfig m = new FruitPotConfig();
                        m.Id = Convert.ToInt32(ite[0]);
                        m.Open = Convert.ToInt32(ite[1]);
                        m.CurPot = Convert.ToInt64(ite[2]);
                        m.Critical = Convert.ToInt64(ite[3]);
                        FruitPotConfig exist = potDBData.Where(k => k.Id ==m.Id && k.CurPot!=m.CurPot ).FirstOrDefault();
                        if (exist != null) //说明奖池被修改了
                        {
                            potStr = potStr + m.Id + "," + m.CurPot + "|";
                        }
                        modelListPot.Add(m);
                    }
                    int resPot = FruitGameBLL.UpdateFruitPotConfig(modelListPot);
                    if (resPot > 0)
                    {

                        int callback = MessagePot(potStr);
                        return Content(callback.ToString());

                    }
                    else
                    {
                        return Content("-3");
                    }
                case "4": //作弊
                    string[] parsZuobi = par.Split('|');
                    List<ArcadeGameStock> modelListArcade = new List<ArcadeGameStock>();

                    IEnumerable<ArcadeGameStock> zuobiDBData = WaterMarginBLL.GetArcadeGameStockList("'11','12','13','14','15','16'");

                    string zuobiStr = "";

                    for (int i = 0; i <= parsZuobi.Length - 1; i++)
                    {
                        string[] ite = parsZuobi[i].Split(',');

                        ArcadeGameStock m = new ArcadeGameStock();
                        m.ID = Convert.ToInt32(ite[0]);
                        m.StockValue = Convert.ToInt64(ite[1]);
                        m.StockCordon = Convert.ToInt64(ite[2]);
                        m.StockIsOpen = Convert.ToInt32(ite[3]);
                        m.Param1 = Convert.ToInt64(ite[4]);//个人盈利控制开关
                        m.Param2 = Convert.ToInt64(ite[5]);//充值玩家增益开关
                        m.Param3 = Convert.ToInt64(ite[6]);//新手玩家增益开关
                        m.Param4 = 0;//小玛丽触发概率
                        m.Param5 = 0;//小玛丽开关
                        m.Param6 = 0;//连输局数
                        m.Param7 =0;//全盘奖开关
                        m.CreateTime = DateTime.Now;
                        modelListArcade.Add(m);

                        ArcadeGameStock zuobidbData = zuobiDBData.Where(k => k.ID == m.ID).FirstOrDefault();
                        if (zuobidbData != null)
                        {
                            zuobiStr = zuobiStr + m.ID + "," + m.StockValue + "|";
                        }


                    }
                    int resZuobi = WaterMarginBLL.UpdateArcadeGameStock(modelListArcade);
                    if (resZuobi > 0)
                    {
                        int callback = MessageZuobi(zuobiStr);
                        return Content(callback.ToString());

                    }
                    else
                    {
                        return Content("-3");
                    }

            }




            //爆灯配置
            IEnumerable<FruitGameExplodeConfig> data = FruitGameBLL.GetExplodeConfig();
         
            //比倍配置
            IEnumerable<FruitBibeiConfig> bibeiConfig = FruitGameBLL.GetBiBeiConfig();



            //彩池配置
            IEnumerable<FruitPotConfig> potData = FruitGameBLL.GetFruitPotConfig("1,2,3,4,5,6",20);

            //水果机作弊库存配置
            IEnumerable<ArcadeGameStock> zuobiData = WaterMarginBLL.GetArcadeGameStockList("'11','12','13','14','15','16'");




            WaterMarginView model = new WaterMarginView();
            model.DataOne = data;
          
            model.DataTwo2 = bibeiConfig;
            model.DataThree = potData;
            model.DataFour = zuobiData;
            return View(model);
        }



        private int MessageSlot()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_NTF_UPDATE_DBPVPCFG, new byte[0] { }));
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NTF_UPDATE_DBPVPCFG:
                    {
                        DbPvpCfg_S DbPvpCfg_S = DbPvpCfg_S.ParseFrom(tbind.body.ToBytes());

                        if (DbPvpCfg_S.Suc == true)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }

                    }
                case CenterCmd.CS_CONNECT_ERROR:
                    return -1;

            }
            return -2;
        }

        private int MessageZuobi(string zuobiStr)
        {
            bool res2 = true; bool res3 = true;
            if (!string.IsNullOrEmpty(zuobiStr))
            {//说明库存值有问题
                zuobiStr = zuobiStr.Trim('|');
                string[] zuobis = zuobiStr.Split('|');
                for (int i = 0; i < zuobis.Length; i++)
                {
                    string[] s = zuobis[i].Split(',');


                    ArcadeGameStock_C ArcadeGameStock_C2 = ArcadeGameStock_C.CreateBuilder()
                     .SetMod(Convert.ToInt32(s[0]))
                     .SetType(1)
                     .SetValue(Convert.ToInt32(s[1]))
                     .Build();

                    Bind tbind2 = Cmd.runClient(new Bind(ServiceCmd.SC_NTF_UPDATE_ARCADEGAMESTOCK, ArcadeGameStock_C2.ToByteArray()));
                    switch ((CenterCmd)tbind2.header.CommandID)
                    {
                        case CenterCmd.CS_NTF_UPDATE_ARCADEGAMESTOCK:
                            {
                                ArcadeGameStock_S ArcadeGameStock_S2 = ArcadeGameStock_S.ParseFrom(tbind2.body.ToBytes());
                                if (ArcadeGameStock_S2.Suc == true)
                                {

                                }
                                else
                                {
                                    res2 = false;
                                }
                            }
                            break;
                        case CenterCmd.CS_CONNECT_ERROR:
                            res2 = false;
                            break;
                        default:
                            res2 = false;
                            break;

                    }
                }
            }


            ArcadeGameStock_C ArcadeGameStock_C3 = ArcadeGameStock_C.CreateBuilder()
                  .SetType(0)
                  .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_NTF_UPDATE_ARCADEGAMESTOCK, ArcadeGameStock_C3.ToByteArray()));
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NTF_UPDATE_ARCADEGAMESTOCK:
                    {
                        ArcadeGameStock_S ArcadeGameStock_S = ArcadeGameStock_S.ParseFrom(tbind.body.ToBytes());
                        if (ArcadeGameStock_S.Suc == true)
                        {

                        }
                        else
                        {
                            res3 = false;
                        }
                    }
                    break;
                case CenterCmd.CS_CONNECT_ERROR:
                    res3 = false; break;

                default:
                    res3 = false; break;

            }
            if (res2 == true && res3 == true)
            {
                return 1; //全部成功
            }
            else if (res2 == true && res3 == false)
            {
                return 2; //其他数据有问题
            }
            else if (res2 == false && res3 == true)
            {
                return 3; //修改库存有问题
            }
            else
            {
                return 4;//全部有问题
            }

        }



        private int MessagePot(string potStr)
        { //通知服务器去拉去数据库数据
            bool res2 = true; bool res3 = true;
            if (!string.IsNullOrEmpty(potStr))
            {//说明奖池被修改了
                potStr = potStr.Trim('|');
                string[] pots = potStr.Split('|');
                for (int i = 0; i < pots.Length; i++)
                {
                    string[] s = pots[i].Split(',');

                    FruiteBigPot_C FruiteBigPot_C2 = FruiteBigPot_C.CreateBuilder()
                     .SetMod(Convert.ToInt32(s[0]))
                     .SetType(1)
                     .SetValue(Convert.ToInt32(s[1]))
                     .Build();


                    Bind tbind2 = Cmd.runClient(new Bind(ServiceCmd.SC_FruiteBigPot, FruiteBigPot_C2.ToByteArray()));
                    switch ((CenterCmd)tbind2.header.CommandID)
                    {
                        case CenterCmd.CS_FruiteBigPot:
                            {
                                FruiteBigPot_S FruiteBigPot_S2 = FruiteBigPot_S.ParseFrom(tbind2.body.ToBytes());
                                if (FruiteBigPot_S2.Suc == true)
                                {

                                }
                                else
                                {
                                    res2 = false;
                                }
                            }
                            break;
                        case CenterCmd.CS_CONNECT_ERROR:
                            res2 = false; break;
                        default:
                            res2 = false; break;

                    }

                }

            }
            //res3

            FruiteBigPot_C FruiteBigPot_C3 = FruiteBigPot_C.CreateBuilder()
            .SetType(0)
            .Build();


            Bind tbind3 = Cmd.runClient(new Bind(ServiceCmd.SC_FruiteBigPot, FruiteBigPot_C3.ToByteArray()));




            switch ((CenterCmd)tbind3.header.CommandID)
            {
                case CenterCmd.CS_FruiteBigPot:
                    {
                        FruiteBigPot_S FruiteBigPot_S3 = FruiteBigPot_S.ParseFrom(tbind3.body.ToBytes());
                        if (FruiteBigPot_S3.Suc == true)
                        {

                        }
                        else
                        {
                            res3 = false;
                        }
                    }
                    break;
                case CenterCmd.CS_CONNECT_ERROR:
                    res3 = false;
                    break;
                default:
                    res3 = false;
                    break;

            }
            if (res2 == true && res3 == true)
            {
                return 1; //全部成功
            }
            else if (res2 == true && res3 == false)
            {
                return 2; //其他数据有问题
            }
            else if (res2 == false && res3 == true)
            {
                return 3; //修改彩池有问题
            }
            else
            {
                return 4;//全部有问题
            }

        }



    }
}