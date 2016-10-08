using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.YYH
{
    public class YYHSignCheck
    {
        public static bool validSign(String transdata, String sign, String key)
        {
            /*
             $trans_data = '{"exorderno":"10004200000001100042","transid":"02113013118562300203","waresid":1,"appid":"20004600000001200046","feetype":0,"money":3000,"count":1,"result":0,"transtype":0,"transtime":"2013-01-31 18:57:27","cpprivate":"123456"}';
 $key = 'MjhERTEwQkFBRDJBRTRERDhDM0FBNkZBMzNFQ0RFMTFCQTBCQzE3QU1UUTRPRFV6TkRjeU16UTVNRFUyTnpnek9ETXJNVE15T1RRME9EZzROVGsyTVRreU1ETXdNRE0zTnpjd01EazNNekV5T1RJek1qUXlNemN4';
 $sign = '28adee792782d2f723e17ee1ef877e7 166bc3119507f43b06977786376c0434 633cabdb9ee80044bc8108d2e9b3c86e';
            */
            // transdata = "{\"exorderno\":\"10004200000001100042\",\"transid\":\"02113013118562300203\",\"waresid\":1,\"appid\":\"20004600000001200046\",\"feetype\":0,\"money\":3000,\"count\":1,\"result\":0,\"transtype\":0,\"transtime\":\"2013-01-31 18:57:27\",\"cpprivate\":\"123456\"},\"paytype\":401";
            //  key = "MjhERTEwQkFBRDJBRTRERDhDM0FBNkZBMzNFQ0RFMTFCQTBCQzE3QU1UUTRPRFV6TkRjeU16UTVNRFUyTnpnek9ETXJNVE15T1RRME9EZzROVGsyTVRreU1ETXdNRE0zTnpjd01EazNNekV5T1RJek1qUXlNemN4";
            // sign = "28adee792782d2f723e17ee1ef877e7 166bc3119507f43b06977786376c0434 633cabdb9ee80044bc8108d2e9b3c86e";





            //jsonStr = transdata ={ "exorderno":"YYHPay20160302194522756","transid":"01416030219452647215","waresid":1,"appid":"5000805433","feetype":0,"money":6,"count":1,"result":0,"transtype":0,"transtime":"2016-03-02 19:45:51","cpprivate":"515游戏-60000游戏币","paytype":401}
            //&sign = 7a532a31a1834b57499c4724939f1a16 5c5b1e091f08b714ead4ebf1a9915ae3 2cc8204e3d228da1aea4dace9bc5fdab
        




            //transdata = "{\"exorderno\":\"YYHPay20160302194522756\",\"transid\":\"01416030219452647215\",\"waresid\":1,\"appid\":\"5000805433\",\"feetype\":0,\"money\":6,\"count\":1,\"result\":0,\"transtype\":0,\"transtime\":\"2016-03-02 19:45:51\",\"cpprivate\":\"515游戏-60000游戏币\",\"paytype\":401}";
           // key = "OEQxODM1M0Q2QjMwNDAzOUYwRDJEMkE4OUVEMjIyMjgwMUU3QzY1RE1UYzJORFkyT0RJMU1UWXdNVGd5TXpnNU16TXJNekU1T0RrNE9ETTRPRGcyT0RnMk5EY3pOVEUyTXprd01UYzBOekV4TnpVMU5UUTRNakU1";
           // sign = "7a532a31a1834b57499c4724939f1a16 5c5b1e091f08b714ead4ebf1a9915ae3 2cc8204e3d228da1aea4dace9bc5fdab";

            try
            {

                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(Encoding.UTF8.GetBytes(transdata.Trim()));
                String md5str = BitConverter.ToString(output).Replace("-", "");

                String decodeBaseStr = Base64.DecodeBase64(key.Trim());

                String[] decodeBaseVec = decodeBaseStr.Replace('+', '#').Split('#');

                String privateKey = decodeBaseVec[0];
                String modkey = decodeBaseVec[1];

                String reqMd5 = RSAUtil.decrypt(sign.Trim(), new BigInteger(privateKey),
                        new BigInteger(modkey));
           
                if ((md5str.ToLower()).Equals(reqMd5.ToLower()))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {

            }

            return false;

        }

    }
}
