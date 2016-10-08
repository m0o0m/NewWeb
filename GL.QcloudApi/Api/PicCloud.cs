using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QCloud.PicApi.Common;
using System.Web;
using System.IO;

namespace QCloud.PicApi.Api
{
    public class PicCloud
    {
        const string PICAPI_CGI_URL = "http://web.image.myqcloud.com/photos/v2/";
        const string DETECTIONAPI_CGI_URL = "http://service.image.myqcloud.com/detection/pornDetect";
        const int appId = 10005601;
        const string secretId = "AKIDHH8zK7Vpl1d9k5vIh55MNSXVw4lTpve0";
        const string secretKey = "1wngrhHJQoxSWCx8u8SNqv9pyFuobXQ9";
        private int timeOut;

        public PicCloud(int timeOut = 60)
        {
            this.timeOut = timeOut;
        }

        /// <summary>
        /// 图片鉴黄
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="url">需要鉴黄图片地址</param>
        /// <returns></returns>
        public string Detection(string bucketName, string url)
        {
            var header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            var expired = DateTime.Now.ToUnixTime() / 1000 + 60;
            var sign = Sign.DetectionSignature(appId, secretId, secretKey, expired, bucketName, url);
            header.Add("Authorization", sign);
            var data = new Dictionary<string, object>();
            data.Add("appid", appId);
            data.Add("bucket", bucketName);
            data.Add("url", url);

            return Request.SendRequest(DETECTIONAPI_CGI_URL, data, HttpMethod.Post, header, timeOut);
        }

        /// <summary>
        /// 图片查询
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="fileId">资源存储的唯一标识</param>
        /// <returns></returns>
        public string Query(string bucketName, string fileId)
        {
            var url = PICAPI_CGI_URL + appId + "/" + bucketName + "/0/" + HttpUtility.UrlEncode(fileId);
            return Request.SendRequest(url, null, HttpMethod.Get, null, timeOut);
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="localPath">本地图片路径</param>
        /// <param name="fileId">资源存储的唯一标识</param>
        /// <returns></returns>
        public string Upload(string bucketName, string localPath, string fileId = null)
        {
            var url = PICAPI_CGI_URL + appId + "/" + bucketName + "/0/" + (string.IsNullOrEmpty(fileId) ? "" :  HttpUtility.UrlEncode(fileId));
            var header = new Dictionary<string, string>();
            var expired = DateTime.Now.ToUnixTime() / 1000 + 60;
            var sign = Sign.Signature(appId, secretId, secretKey, expired, bucketName);
            header.Add("Authorization", sign);
            return Request.SendRequest(url, null, HttpMethod.Post, header, timeOut, localPath);
        }

        /// <summary>
        /// 图片复制
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="fileId">资源存储的唯一标识</param>
        /// <returns></returns>
        public string Copy(string bucketName, string fileId)
        {
            var url = PICAPI_CGI_URL + appId + "/" + bucketName + "/0/" + HttpUtility.UrlEncode(fileId) + "/copy";
            var sign = Sign.SignatureOnce(appId, secretId, secretKey, bucketName, fileId);
            var header = new Dictionary<string, string>();
            header.Add("Authorization", sign);
            return Request.SendRequest(url, null, HttpMethod.Post, header, timeOut);
        }

        /// <summary>
        /// 图片删除
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="fileId">资源存储的唯一标识</param>
        /// <returns></returns>
        public string Delete(string bucketName, string fileId)
        {
            var url = PICAPI_CGI_URL + appId + "/" + bucketName + "/0/" + HttpUtility.UrlEncode(fileId) + "/del";
            var sign = Sign.SignatureOnce(appId, secretId, secretKey, bucketName, fileId);
            var header = new Dictionary<string, string>();
            header.Add("Authorization", sign);
            return Request.SendRequest(url, null, HttpMethod.Post, header, timeOut);
        }
    }
}
