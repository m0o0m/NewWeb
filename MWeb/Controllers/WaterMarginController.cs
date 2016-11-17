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

  

    [Authorize]
    public class WaterMarginController : Controller
    {
        [QueryValues]
        // GET: WaterMargin
        public ActionResult Index()
        {
            return View();
        }



        [QueryValues]
        /// <summary>
        /// 水浒传控制台首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigIndex(Dictionary<string, string> queryvalues) {
            //从数据库中获取配置参数 



            //QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //qrCodeEncoder.QRCodeScale = 4;
            //qrCodeEncoder.QRCodeVersion = 8;
            //qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            ////System.Drawing.Image image = qrCodeEncoder.Encode("4408810820 深圳－广州 小江");
            //System.Drawing.Image image = qrCodeEncoder.Encode("http://www.baidu.com");
            //string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
            //string filepath = Server.MapPath(@"~\Upload") + "\\" + filename;
            //System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            //image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

            //fs.Close();
            //image.Dispose();
            //二维码解码



         //   string s = BaiDuHelper.TransLongUrlToTinyUrl("http://www.515.com/tg/?via=asdf");





            string par = queryvalues.ContainsKey("par") ? queryvalues["par"].Trim(',') : "";
            string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : ""; 
            string operType = queryvalues.ContainsKey("operType") ? queryvalues["operType"] : ""; 
            if(par.Contains("'")){
                return Content("-4");
            }
            switch (operType)
            {
                case "1": //说明是保存配置1，2，3
                          //type说明是保存的配置
                    string[] pars = par.Split('|');
                    List<WaterMargin> modelList = new List<WaterMargin>();
                    for (int i = 0; i < pars.Length - 1; i++)
                    {
                        WaterMargin m = new WaterMargin();
                        string[] ps = pars[i].Split(',');
                        m.ColumnNO = i + 1;
                        m.Type = Convert.ToInt32(type);
                        m.Hatchet = Convert.ToInt32(ps[0]);
                        m.Gun = Convert.ToInt32(ps[1]);
                        m.Knife = Convert.ToInt32(ps[2]);
                        m.Lu = Convert.ToInt32(ps[3]);
                        m.Lin = Convert.ToInt32(ps[4]);
                        m.Song = Convert.ToInt32(ps[5]);
                        m.God = Convert.ToInt32(ps[6]);
                        m.Hall = Convert.ToInt32(ps[7]);
                        m.Outlaws = Convert.ToInt32(ps[8]);
                        m.Wine = Convert.ToInt64( Convert.ToDouble(pars[pars.Length - 1]) * 1000);
                        m.CreateTime = DateTime.Now;
                        modelList.Add(m);
                    }



                    int res = WaterMarginBLL.UpdateWatermargin(modelList);
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
                case "2":  //说明是修改小玛丽
                   
                    Int64 uplimit = queryvalues.ContainsKey("uplimit") ? Convert.ToInt64(queryvalues["uplimit"]) : 0;
                    Int64 downlimit = queryvalues.ContainsKey("downlimit") ? Convert.ToInt64(queryvalues["downlimit"]) : 0;

                    Int64 bibei1 = queryvalues.ContainsKey("bibei1") ? Convert.ToInt64(queryvalues["bibei1"]) : 0;
                    Int64 bibei2 = queryvalues.ContainsKey("bibei2") ? Convert.ToInt64(queryvalues["bibei2"]) : 0;
                    Int64 bibei3 = queryvalues.ContainsKey("bibei3") ? Convert.ToInt64(queryvalues["bibei3"]) : 0;
                    Int64 bibei4= queryvalues.ContainsKey("bibei4") ? Convert.ToInt64(queryvalues["bibei4"]) : 0;
                    Int64 bibei5 = queryvalues.ContainsKey("bibei5") ? Convert.ToInt64(queryvalues["bibei5"]) : 0;
                    Int64 bibein = queryvalues.ContainsKey("bibein") ? Convert.ToInt64(queryvalues["bibein"]) : 0;




                    int marryRes = WaterMarginBLL.UpdateMary(1, uplimit, downlimit,bibei1,bibei2,bibei3,bibei4,bibei5,bibein);
                    if (marryRes > 0)
                    {//修改成功
                       // return Content("1");
                        int marryCallback = MessageSlot();
                        return Content(marryCallback.ToString());
                    }
                    else
                    {
                        return Content("-3");
                    }
                case "3": //修改奖池
                    string[] parsPot = par.Split('|');
                    List<SSwitch> modelListPot = new List<SSwitch>();
                    string potStr = "";

                    IEnumerable<SSwitch> potDBData = WaterMarginBLL.GetSSwitchList("'7001','7002','7003','7004','7005','7006'");

                    for (int i = 0; i <= parsPot.Length - 1; i++)
                    {
                        string[] ite = parsPot[i].Split(',');

                        SSwitch m = new SSwitch();
                        m.ID = Convert.ToInt32(ite[0]);
                        m.ISOpen = ite[1]=="1"?true:false;
                        m.para1 = Convert.ToInt32( Convert.ToDouble( ite[2])*1000);
                        m.para2 = Convert.ToInt64( ite[3]);
                        m.para6 = Convert.ToInt64( ite[4]);

                        SSwitch exist = potDBData.Where(k => k.ID == m.ID && k.para2!=m.para2).FirstOrDefault();
                        if (exist != null) //说明奖池被修改了
                        {
                            potStr = potStr+ m.ID + "," + m.para2 + "|";
                        }
                        modelListPot.Add(m);
                    }
                    int resPot =  WaterMarginBLL.UpdateSSwitch(modelListPot);
                    if (resPot > 0)
                    {
                        int callback = MessagePot(potStr);
                        return Content(callback.ToString());
                    }
                    else {
                        return Content("-3");
                    }
                case "4": //作弊
                    string[] parsZuobi = par.Split('|');
                    List<ArcadeGameStock> modelListArcade = new List<ArcadeGameStock>();

                    IEnumerable<ArcadeGameStock> zuobiDBData = WaterMarginBLL.GetArcadeGameStockList("'1','2','3','4','5','6'");

                    string zuobiStr = "";

                    for (int i = 0; i <= parsZuobi.Length - 1; i++)
                    {
                        string[] ite = parsZuobi[i].Split(',');

                        ArcadeGameStock m = new ArcadeGameStock();
                        m.ID = Convert.ToInt32( ite[0]);
                        m.StockValue = Convert.ToInt64(ite[1]);
                        m.StockCordon = Convert.ToInt64(ite[2]);
                        m.StockIsOpen = Convert.ToInt32(ite[3]);
                        m.Param1 = Convert.ToInt64(ite[4]);//个人盈利控制开关
                        m.Param2 = Convert.ToInt64(ite[5]);//充值玩家增益开关
                        m.Param3 = Convert.ToInt64(ite[6]);//新手玩家增益开关
                        m.Param4 = Convert.ToInt64( Convert
                            .ToDouble(ite[7])*1000);//小玛丽触发概率
                        m.Param5 = Convert.ToInt64(ite[8]);//小玛丽开关
                        m.Param6 = Convert.ToInt64(ite[9]);//连输局数
                        m.Param7 = Convert.ToInt64(ite[10]);//全盘奖开关
                        m.CreateTime = DateTime.Now;
                        modelListArcade.Add(m);

                        ArcadeGameStock zuobidbData = zuobiDBData.Where(k => k.ID == m.ID).FirstOrDefault();
                        if (zuobidbData != null) {
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




            //slot配置
            IEnumerable<WaterMargin> data =  WaterMarginBLL.GetWaterMarginList();
            //小玛丽
            MarryConfig maryConfig = WaterMarginBLL.GetMarryConfig(1);
            //比倍配置
            IEnumerable<BiBeiConfig> bibeiConfig = WaterMarginBLL.GetBiBeiConfig();

          
            //彩池配置
            IEnumerable<SSwitch> potData =WaterMarginBLL.GetSSwitchList("'7001','7002','7003','7004','7005','7006'");

            IEnumerable<ArcadeGameStock> zuobiData = WaterMarginBLL.GetArcadeGameStockList("'1','2','3','4','5','6'");




            WaterMarginView model = new WaterMarginView();
            model.DataOne = data;
            model.DataTwo = maryConfig;
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



        private int MessagePot(string potStr)
        { //通知服务器去拉去数据库数据
            bool res2 = true; bool res3 = true;
            if (!string.IsNullOrEmpty(potStr)) {//说明奖池被修改了
                potStr = potStr.Trim('|');
                string[] pots = potStr.Split('|');
                for (int i = 0; i < pots.Length; i++) {
                    string[] s = pots[i].Split(',');

                    ShuiHuPot_C ShuiHuPot_C2 = ShuiHuPot_C.CreateBuilder()
                     .SetMod( Convert.ToInt32(s[0]))
                     .SetType(1)
                     .SetValue(Convert.ToInt32(s[1]))
                     .Build();


                    Bind tbind2 = Cmd.runClient(new Bind(ServiceCmd.SC_NTF_UPDATE_SHUIHUPOT, ShuiHuPot_C2.ToByteArray()));
                    switch ((CenterCmd)tbind2.header.CommandID)
                    {
                        case CenterCmd.CS_NTF_UPDATE_SHUIHUPOT:
                            {
                                ShuiHuPot_S ShuiHuPot_S2 = ShuiHuPot_S.ParseFrom(tbind2.body.ToBytes());
                                if (ShuiHuPot_S2.Suc == true)
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

            ShuiHuPot_C ShuiHuPot_C3 = ShuiHuPot_C.CreateBuilder()
            .SetType(0)
            .Build();


            Bind tbind3 = Cmd.runClient(new Bind(ServiceCmd.SC_NTF_UPDATE_SHUIHUPOT, ShuiHuPot_C3.ToByteArray()));




            switch ((CenterCmd)tbind3.header.CommandID)
            {
                case CenterCmd.CS_NTF_UPDATE_SHUIHUPOT:
                    {
                        ShuiHuPot_S ShuiHuPot_S3 = ShuiHuPot_S.ParseFrom(tbind3.body.ToBytes());
                        if (ShuiHuPot_S3.Suc == true)
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
            else {
                return 4;//全部有问题
            }
          
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


    }
}