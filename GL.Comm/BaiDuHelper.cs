using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GL.Common
{
    public  class BaiDuHelper
    {

        public static string TransLongUrlToTinyUrl(string longurl) {
            var data = "source=3271760578&url_long=" + longurl;
            byte[] postData = Encoding.UTF8.GetBytes(data);
            var url = "http://api.t.sina.com.cn/short_url/shorten.json";
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            // var rs = client.UploadString(url, "POST", postData);

            byte[] responseData = client.UploadData(url, "POST", postData);
            var rs = Encoding.UTF8.GetString(responseData);


            TinyUrl tini = Newtonsoft.Json.JsonConvert.DeserializeObject<TinyUrl>(rs.Trim('[').Trim(']'));




            return tini.url_short;
        }
    }



    public class TinyUrl
    {
        public string url_short { get; set; }
    
        public string url_long { get; set; }

        public int type { get; set; }

    }

}
