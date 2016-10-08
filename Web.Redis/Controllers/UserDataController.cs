using GL.Common;
using GL.Data.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using QCloud.PicApi.Api;
using QCloud.CosApi.Api;
using Newtonsoft.Json.Linq;
using GL.Data.Model;
using log4net;

namespace Web.Redis.Controllers
{
    [RoutePrefix("api/UserData")]
    public class UserDataController : ApiController
    {
        private ILog log = LogManager.GetLogger("UserData");

        [Route("DownCos")]
        [HttpGet]
        public object DownCos([FromUri]string userid, [FromUri]string url)
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                return Json(new { res = -1 });
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                return Json(new { res = -1 });
            }
            if (!Utils.IsUrl(url))
            {
                return Json(new { res = -1 });
            }
            try
            {

                WebClient wc = new WebClient();
                byte[] ab = wc.DownloadData(url);
                using (MemoryStream ms = new MemoryStream(ab, 0, ab.Length))
                {
                    using (Image returnImage = Image.FromStream(ms))
                    {
                        string Path = "";
#if DEBUG || P17
                        Path = @"2";
#endif
#if Test
                        Path = @"1";
#endif
#if Release
                        Path = @"0";
#endif

                        string tempPath = string.Format(@"{0}\{1}", Utils.ServerPath(@"\img\tmp"), Utils.GenerateNonceStr());
                        returnImage.Save(tempPath);
                        var tmpName = Utils.SHA1File(tempPath);
                        File.Delete(tempPath);

                        string Sha1Value = string.Empty;
                        string fileName = string.Format(@"{0}.jpg", tmpName); 
                        string filePath = string.Format(@"{0}\{1}", Utils.ServerPath(@"\img\" + Path), fileName);
                        string remotePath = string.Format(@"/{0}/{1}", Path, fileName);
                        var result = "";
                        var bucketName = "logo";
                        var cos = new CosCloud();

                        if (File.Exists(filePath))
                        {
                            Sha1Value = Utils.SHA1File(filePath);
                        }

                        var flag = tmpName == Sha1Value;



                        if (!flag)
                        {
                            //var user = RoleBLL.GetModelByIDList(userid).FirstOrDefault();
                            //if (user != null)
                            //{
                            //    string delfileName = user.FigureUrl;
                            //    log.Info(" DownCos delfileName:" + delfileName);
                            //    string delfilePath = string.Format(@"{0}\{1}", Utils.ServerPath(@"\img" + Path), delfileName);
                            //    if (File.Exists(delfilePath))
                            //    {
                            //        File.Delete(delfilePath);
                            //    }
                            //    result = cos.GetFileStat(bucketName, delfileName);
                            //    log.Info(" DownCos GetFileStat:" + result);
                            //    result = cos.DeleteFile(bucketName, delfileName);
                            //    log.Info(" DownCos DeleteFile:" + result);
                            //}
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            //result = cos.GetFileStat(bucketName, remotePath);
                            //if((int)((JObject)JsonConvert.DeserializeObject(result))["code"] == 0)
                            //{
                                result = cos.DeleteFile(bucketName, remotePath);
                            //}


                            returnImage.Save(filePath);
                            returnImage.Dispose();

                            result = cos.UploadFile(bucketName, remotePath, filePath);
                            var obj = (JObject)JsonConvert.DeserializeObject(result);
                            var code = (int)obj["code"];
                            if (code == 0)
                            {
                                //var data = obj["data"];
                                //var fileId = data["fileid"].ToString();
                                //var downloadUrl = data["download_url"].ToString();
                                //result = pic.Query(bucketName, fileId);
                                //result = pic.Copy(bucketName, fileId);
                                //result = pic.Delete(bucketName, fileId);
                                //result = pic.Detection(bucketName, downloadUrl);
                                //Console.WriteLine(result);
#if DEBUG
                                return Json(new { res = 1, fileId = obj });
#endif
                                return Json(new { res = 1, fileId = remotePath });
                            }
                            return Json(new { res = code, fileId = remotePath, error = obj["message"].ToString() });
                        }

                        returnImage.Dispose();
                        return Json(new { res = -18861, fileId = remotePath, error = "图片fileid已经存在" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { res = -1, error = ex.Message });
            }


        }

        [Route("DownImg")]
        [HttpGet]
        public object DownImg([FromUri]string userid, [FromUri]string url)
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                return Json(new { res = -1 });
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                return Json(new { res = -1 });
            }
            if (!Utils.IsUrl(url))
            {
                return Json(new { res = -1 });
            }
            try
            {

                WebClient wc = new WebClient();
                byte[] ab = wc.DownloadData(url);
                MemoryStream ms = new MemoryStream(ab, 0, ab.Length);
                Image returnImage = Image.FromStream(ms);
                string fileName = string.Format("{0}", Utils.MD5(userid + url));
                string filePath = string.Format(@"{0}{1}.jpg", Utils.ServerPath("\\img\\"), fileName);
                var result = "";
                var bucketName = "logo";
                var pic = new PicCloud();

                //if (!File.Exists(filePath))
                //{
                    //var user = RoleBLL.GetModelByIDList(userid).FirstOrDefault();
                    //if (user != null)
                    //{
                    //    string delfileName = string.Format("{0}", user.FigureUrl);
                    //    string delfilePath = string.Format(@"{0}{1}.jpg", Utils.ServerPath("\\img\\"), delfileName);
                    //    if (File.Exists(delfilePath))
                    //    {
                    //        File.Delete(delfilePath);
                    //        //result = pic.Query(bucketName, delfileName);
                    //        result = pic.Delete(bucketName, delfileName);
                    //    }
                    //}

                    returnImage.Save(filePath);
                    returnImage.Dispose();

                    result = pic.Upload(bucketName, filePath, fileName);
                    var obj = (JObject)JsonConvert.DeserializeObject(result);
                    var code = (int)obj["code"];
                    if (code == 0)
                    {
                        var data = obj["data"];
                        var fileId = data["fileid"].ToString();
                        //var downloadUrl = data["download_url"].ToString();
                        //result = pic.Query(bucketName, fileId);
                        //result = pic.Copy(bucketName, fileId);
                        //result = pic.Delete(bucketName, fileId);
                        //result = pic.Detection(bucketName, downloadUrl);
                        //Console.WriteLine(result);
#if DEBUG
                        return Json(new { res = 1, fileId = obj });
#endif
                        return Json(new { res = 1, fileId = obj["data"]["fileid"].ToString() });
                    }
                    return Json(new { res = code, fileId = fileName, error = obj["message"].ToString() });
                //}

                //return Json(new { res = -1886, fileId = fileName, error = "图片fileid已经存在" });


            }
            catch (Exception ex)
            {
                return Json(new { res = -1, error = ex.Message });
            }


        }




    }
}