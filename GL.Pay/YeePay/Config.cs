using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.YeePay
{
    public class Config
    {

        static Config()
        {
            ////商户账户编号
            //merchantAccount = "10000418926";

            ////商户RSA密钥对——公钥，(请见“RSA密钥对生成说明.txt”)，该公钥需要在商户后台向易宝支付报备
            ////商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://www.yeepay.com)
            //merchantPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCwlpjGSN7c2iVDYA6liFvAdghPDQnWTEvvmlBZQb1ZnqTLZoRhcK/fCjs9IvuqeNnP0VF0scqBPy92jBds2aVIiXJ0ZknYi6T2CNlQ93qDVHtq3iIChfNrr7KCIeBCrG7KD2W3EMb2IxUHt6oI1Tza/FfdZs/r2yyJJubd0j7DFQIDAQAB";

            ////商户RSA密钥对——私钥，(请见“RSA密钥对生成说明.txt”)
            //merchantPrivatekey = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBALCWmMZI3tzaJUNgDqWIW8B2CE8NCdZMS++aUFlBvVmepMtmhGFwr98KOz0i+6p42c/RUXSxyoE/L3aMF2zZpUiJcnRmSdiLpPYI2VD3eoNUe2reIgKF82uvsoIh4EKsbsoPZbcQxvYjFQe3qgjVPNr8V91mz+vbLIkm5t3SPsMVAgMBAAECgYAdF6JxwF2fCv1qnS+si8t56LgztdUyDf3Qqp6kJdV5J07FB821c+g1mazqxJGrox9XQofl7siLBIrgP/I4B59YDtoX7zr9l2YRrHUpBJOQYg7PIOWzo6CcFluK83Hne+PGnJpWrJiJd53N/8MNOAK8RrErRn8GO+UZNDSPIhkwXQJBANyR7uNI5zYJVk8djR9O/++B8UZMQ51V5NZTY0jHwtgJ9/qOi9GZInim+ivR8EOCuZCXqq+zuWNdH37or29yxpcCQQDM9BZr3T0+2qTn1IfvTCO7fSJAWRaIuZtDsoCVBuJf4BQbB7KD+zQvyzhCPwXWLgUHkHJETD2XDW++GLR5ycUzAj83JESUjaU/3RW2sayWJynUtqea63X7331WF4K6rzYGzHcyLHDH9YCoqRXh3poyRnwdqc0CH+w46w70qzcwpYECQQCPlJAAkMVPOy07nBB+/AAsYMWV/tNihWTYUDz0KhZ8xCZRqVrOSzWMJfoLrssP+L1dRzxFzIN5Rth5fCUzDL8xAkEAt9mmIOOEeZppG7iw/RqtjCB1vngYJnIm8dVWRQGVg4t4qMXaxazKewT6rfd9xU3494SdM9x4AXq5ukj4it/ajA==";

            ////易宝支付分配的公钥，该公钥由商户进入商户后台先上报自己的公钥再获取，商户后台目录为（产品管理——RSA公钥管理）
            ////商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://www.yeepay.com)
            //yibaoPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCdaAde+egFkLwV7THPum4nPSBAJ2MGOaYBBldbKdbnCX8emCqXtp8OB9WIWbDVQfpNAH/s53Z/NW1pmjhLbbgOGcsEGd/feh/QIL80Wv26afqlLG/lTvUavnSdQs732/5viT+G/C9YWWp4MxqKTd8Va1b9BkzfpuvqcmAtiHkPBwIDAQAB";
            //商户账户编号



#if Debug
            merchantAccount = "YB01000000144";

            //商户RSA密钥对——公钥，(请见“RSA密钥对生成说明.txt”)，该公钥需要在商户后台向易宝支付报备
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            merchantPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDxhOgx8q1FAIKnqf6BqjCLKyXXSTRSNSfwS9Nc6E2ffmIpbieyN7mB7XqQKY/icnOB34vtPAjEmQUx4uc1h5R0ApdFm3RJEsWokV/beGjEtd1i2EoSgYwGSXaC32ExpcmsPrZu1hvzEflVmpJD19KcXxvnbmQEHiA6AS1Xy/vooQIDAQAB";

            //商户RSA密钥对——私钥，(请见“RSA密钥对生成说明.txt”)
            merchantPrivatekey = "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAPGE6DHyrUUAgqep/oGqMIsrJddJNFI1J/BL01zoTZ9+YiluJ7I3uYHtepApj+Jyc4Hfi+08CMSZBTHi5zWHlHQCl0WbdEkSxaiRX9t4aMS13WLYShKBjAZJdoLfYTGlyaw+tm7WG/MR+VWakkPX0pxfG+duZAQeIDoBLVfL++ihAgMBAAECgYAw2urBV862+5BybA/AmPWy4SqJbxR3YKtQj3YVACTbk4w1x0OeaGlNIAW/7bheXTqCVf8PISrA4hdL7RNKH7/mhxoX3sDuCO5nsI4Dj5xF24CymFaSRmvbiKU0Ylso2xAWDZqEs4Le/eDZKSy4LfXA17mxHpMBkzQffDMtiAGBpQJBAPn3mcAwZwzS4wjXldJ+Zoa5pwu1ZRH9fGNYkvhMTp9I9cf3wqJUN+fVPC6TIgLWyDf88XgFfjilNKNz0c/aGGcCQQD3WRxwots1lDcUhS4dpOYYnN3moKNgB07Hkpxkm+bw7xvjjHqI8q/4Jiou16eQURG+hlBZlZz37Y7P+PHF2XG3AkAyng/1WhfUAfRVewpsuIncaEXKWi4gSXthxrLkMteM68JRfvtb0cAMYyKvr72oY4Phyoe/LSWVJOcW3kIzW8+rAkBWekhQNRARBnXPbdS2to1f85A9btJP454udlrJbhxrBh4pC1dYBAlz59v9rpY+Ban/g7QZ7g4IPH0exzm4Y5K3AkBjEVxIKzb2sPDe34Aa6Qd/p6YpG9G6ND0afY+m5phBhH+rNkfYFkr98cBqjDm6NFhT7+CmRrF903gDQZmxCspY";

            //易宝支付分配的公钥，该公钥由商户进入商户后台先上报自己的公钥再获取，商户后台目录为（产品管理——RSA公钥管理）
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            yibaoPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCxnYJL7fH7DVsS920LOqCu8ZzebCc78MMGImzW8MaP/cmBGd57Cw7aRTmdJxFD6jj6lrSfprXIcT7ZXoGL5EYxWUTQGRsl4HZsr1AlaOKxT5UnsuEhA/K1dN1eA4lBpNCRHf9+XDlmqVBUguhNzy6nfNjb2aGE+hkxPP99I1iMlQIDAQAB";
#endif

#if P17
            merchantAccount = "YB01000000144";

            //商户RSA密钥对——公钥，(请见“RSA密钥对生成说明.txt”)，该公钥需要在商户后台向易宝支付报备
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            merchantPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDxhOgx8q1FAIKnqf6BqjCLKyXXSTRSNSfwS9Nc6E2ffmIpbieyN7mB7XqQKY/icnOB34vtPAjEmQUx4uc1h5R0ApdFm3RJEsWokV/beGjEtd1i2EoSgYwGSXaC32ExpcmsPrZu1hvzEflVmpJD19KcXxvnbmQEHiA6AS1Xy/vooQIDAQAB";

            //商户RSA密钥对——私钥，(请见“RSA密钥对生成说明.txt”)
            merchantPrivatekey = "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAPGE6DHyrUUAgqep/oGqMIsrJddJNFI1J/BL01zoTZ9+YiluJ7I3uYHtepApj+Jyc4Hfi+08CMSZBTHi5zWHlHQCl0WbdEkSxaiRX9t4aMS13WLYShKBjAZJdoLfYTGlyaw+tm7WG/MR+VWakkPX0pxfG+duZAQeIDoBLVfL++ihAgMBAAECgYAw2urBV862+5BybA/AmPWy4SqJbxR3YKtQj3YVACTbk4w1x0OeaGlNIAW/7bheXTqCVf8PISrA4hdL7RNKH7/mhxoX3sDuCO5nsI4Dj5xF24CymFaSRmvbiKU0Ylso2xAWDZqEs4Le/eDZKSy4LfXA17mxHpMBkzQffDMtiAGBpQJBAPn3mcAwZwzS4wjXldJ+Zoa5pwu1ZRH9fGNYkvhMTp9I9cf3wqJUN+fVPC6TIgLWyDf88XgFfjilNKNz0c/aGGcCQQD3WRxwots1lDcUhS4dpOYYnN3moKNgB07Hkpxkm+bw7xvjjHqI8q/4Jiou16eQURG+hlBZlZz37Y7P+PHF2XG3AkAyng/1WhfUAfRVewpsuIncaEXKWi4gSXthxrLkMteM68JRfvtb0cAMYyKvr72oY4Phyoe/LSWVJOcW3kIzW8+rAkBWekhQNRARBnXPbdS2to1f85A9btJP454udlrJbhxrBh4pC1dYBAlz59v9rpY+Ban/g7QZ7g4IPH0exzm4Y5K3AkBjEVxIKzb2sPDe34Aa6Qd/p6YpG9G6ND0afY+m5phBhH+rNkfYFkr98cBqjDm6NFhT7+CmRrF903gDQZmxCspY";

            //易宝支付分配的公钥，该公钥由商户进入商户后台先上报自己的公钥再获取，商户后台目录为（产品管理——RSA公钥管理）
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            yibaoPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCxnYJL7fH7DVsS920LOqCu8ZzebCc78MMGImzW8MaP/cmBGd57Cw7aRTmdJxFD6jj6lrSfprXIcT7ZXoGL5EYxWUTQGRsl4HZsr1AlaOKxT5UnsuEhA/K1dN1eA4lBpNCRHf9+XDlmqVBUguhNzy6nfNjb2aGE+hkxPP99I1iMlQIDAQAB";
#endif
#if Test
            merchantAccount = "10012433141";

            //商户RSA密钥对——公钥，(请见“RSA密钥对生成说明.txt”)，该公钥需要在商户后台向易宝支付报备
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            merchantPublickey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCYqf6h7RE3PEDzwPRFQZ/N8TGWF00eO/Zob4poFEFc3GGrFQR7eyZ9yokDoP7pw4WdxSWLtgf0WlAUQPRwGxUePir52zWeABTcVW9nPhFTam5wmmFEuHDJRMwy66kiCZGXtW7kIcTu+G54xJdWuJIEi4odmyN4SAF96t1qKKJp1wIDAQAB";

            //商户RSA密钥对——私钥，(请见“RSA密钥对生成说明.txt”)
            merchantPrivatekey = @"MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAJip/qHtETc8QPPA9EVBn83xMZYXTR479mhvimgUQVzcYasVBHt7Jn3KiQOg/unDhZ3FJYu2B/RaUBRA9HAbFR4+KvnbNZ4AFNxVb2c+EVNqbnCaYUS4cMlEzDLrqSIJkZe1buQhxO74bnjEl1a4kgSLih2bI3hIAX3q3WooomnXAgMBAAECgYBSf8szBkG/b7hKAYP/yS7qw+TgD0eFhzHpzh0lkYyg+hdttLXvZOWwJLtWUrJu8VJLqDZaAczap9OOnmt6CainESWnRh0QrDbaa2Sn1fw3LPLLNdhY+Hgt3+TYPW1O5K9vRkZ/UnFvHf9mzgD18sO1OwtUGS1h6qgiUX716u0nwQJBANLQbWkwThqJtcL7HoyoOqh/3Sh4IPa13yyWNNwpBsH1CghtLPcWkwZ1sWmwLXmh0Ry6U00Z4LfM35sd9H/7sosCQQC5YtCYnihVcTI8pUxCWnyslF1fQ8NacV7YwUvv4wRsdZt0hp6GkAvqskDArh165JPlsywOw5lbDDgelfGvGwtlAkEAjvk6as+O+OKSehVTh7OEbMijFhI60PdRz2xjlzjf02U7k7FvgHg36HajhvksLkS3jJM1caHuTNlOgWYUb0QltwJAY+EeO6uyVV5YT55LONChrSV+LO5IWPkNvcBe1k68OmceqyhAToVbNkZ1ZpooXea63B2tVgMCI7Cwp6HnhY0PlQJAT3mM63DcXHqCZdWJToU0//7RRdy886E8VqDmH4EVewB5ebFzcYBtQEn1q4oJPClYZ/x/70eksY3Ky92/kjncVg==";

            //易宝支付分配的公钥，该公钥由商户进入商户后台先上报自己的公钥再获取，商户后台目录为（产品管理——RSA公钥管理）
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            yibaoPublickey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDB3+9xr+vG/Ss3/nBQeDvKEoSW/H51F7jPw+mjqWdwGh1vIogE75xY5C83kv3QpRcYOKh89VD61a1HfsbDruJg/WeyvjT9JjRDaXMYmO5vdXVVP6Afr5FmuS++AWeXeG/sZsxQhWHhdweDuQ3HnN7soqilF8bS5k4oLR+1zFtyJQIDAQAB";
#endif
#if Release
            merchantAccount = "10012433141";

            //商户RSA密钥对——公钥，(请见“RSA密钥对生成说明.txt”)，该公钥需要在商户后台向易宝支付报备
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            merchantPublickey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCYqf6h7RE3PEDzwPRFQZ/N8TGWF00eO/Zob4poFEFc3GGrFQR7eyZ9yokDoP7pw4WdxSWLtgf0WlAUQPRwGxUePir52zWeABTcVW9nPhFTam5wmmFEuHDJRMwy66kiCZGXtW7kIcTu+G54xJdWuJIEi4odmyN4SAF96t1qKKJp1wIDAQAB";

            //商户RSA密钥对——私钥，(请见“RSA密钥对生成说明.txt”)
            merchantPrivatekey = @"MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAJip/qHtETc8QPPA9EVBn83xMZYXTR479mhvimgUQVzcYasVBHt7Jn3KiQOg/unDhZ3FJYu2B/RaUBRA9HAbFR4+KvnbNZ4AFNxVb2c+EVNqbnCaYUS4cMlEzDLrqSIJkZe1buQhxO74bnjEl1a4kgSLih2bI3hIAX3q3WooomnXAgMBAAECgYBSf8szBkG/b7hKAYP/yS7qw+TgD0eFhzHpzh0lkYyg+hdttLXvZOWwJLtWUrJu8VJLqDZaAczap9OOnmt6CainESWnRh0QrDbaa2Sn1fw3LPLLNdhY+Hgt3+TYPW1O5K9vRkZ/UnFvHf9mzgD18sO1OwtUGS1h6qgiUX716u0nwQJBANLQbWkwThqJtcL7HoyoOqh/3Sh4IPa13yyWNNwpBsH1CghtLPcWkwZ1sWmwLXmh0Ry6U00Z4LfM35sd9H/7sosCQQC5YtCYnihVcTI8pUxCWnyslF1fQ8NacV7YwUvv4wRsdZt0hp6GkAvqskDArh165JPlsywOw5lbDDgelfGvGwtlAkEAjvk6as+O+OKSehVTh7OEbMijFhI60PdRz2xjlzjf02U7k7FvgHg36HajhvksLkS3jJM1caHuTNlOgWYUb0QltwJAY+EeO6uyVV5YT55LONChrSV+LO5IWPkNvcBe1k68OmceqyhAToVbNkZ1ZpooXea63B2tVgMCI7Cwp6HnhY0PlQJAT3mM63DcXHqCZdWJToU0//7RRdy886E8VqDmH4EVewB5ebFzcYBtQEn1q4oJPClYZ/x/70eksY3Ky92/kjncVg==";

            //易宝支付分配的公钥，该公钥由商户进入商户后台先上报自己的公钥再获取，商户后台目录为（产品管理——RSA公钥管理）
            //商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://ok.yeepay.com/merchant)
            yibaoPublickey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDB3+9xr+vG/Ss3/nBQeDvKEoSW/H51F7jPw+mjqWdwGh1vIogE75xY5C83kv3QpRcYOKh89VD61a1HfsbDruJg/WeyvjT9JjRDaXMYmO5vdXVVP6Afr5FmuS++AWeXeG/sZsxQhWHhdweDuQ3HnN7soqilF8bS5k4oLR+1zFtyJQIDAQAB";
#endif

        }

        public static string merchantAccount
        { get; set; }

        public static string merchantPublickey
        { get; set; }

        public static string merchantPrivatekey
        { get; set; }

        public static string yibaoPublickey
        { get; set; }
    }
}
