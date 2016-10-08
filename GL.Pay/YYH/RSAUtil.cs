using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.YYH
{
    public class RSAUtil
    {
        /**
         * 
         */
        public const int NUMBIT = 64;

        /**
         * 获取随机质数
         * 
         * @return
         */
        public static BigInteger getPrimes()
        {
            return BigInteger.ProbablePrime(NUMBIT, new Random());
        }

        /**
         * 通过P,Q计算N值
         * 
         * @param 一个素数p
         * @param 一个素数q
         * @return 返回P*Q的值n
         */
        public static BigInteger getN(BigInteger p, BigInteger q)
        {
            return p.Multiply(q);
        }

        /**
         * 通过P,Q计算ran值 modkey
         * 
         * @param 一个素数p
         *            ,不能为空
         * @param 一个素数q
         *            ,不能为空
         * @return 返回(P-1)*(Q-1)的值ran
         */
        public static BigInteger getRan(BigInteger p, BigInteger q)
        {
            return (p.Subtract(BigInteger.One))
                    .Multiply(q.Subtract(BigInteger.One));
        }

        /**
         * 获取公钥(128位)
         * <p>
         * 具体代码如下: <code><font color="red"><br>
         * BigInteger p = RSAUtil.getPrimes();<br>
         * BigInteger q = RSAUtil.getPrimes();<br>
         * BigInteger ran = RSAUtil.getRan(p, q);<br>
         * BigInteger n = RSAUtil.getN(p, q);//modkey -- N值<br>
         * BigInteger pKey = RSAUtil.getPublicKey(ran);//publicKey -- 公钥<br>
         * </font></code>
         * 
         * @param ran
         *            通过getRan静态方法计算出来的值
         * @return
         */
        public static BigInteger getPublicKey(BigInteger ran)
        {
            BigInteger temp = null;
            BigInteger e = BigInteger.Zero;
            do
            {
                temp = BigInteger.ProbablePrime(NUMBIT, new Random());
                /*
                 * 随机生成一个素数，看他是否与ran的公约数为1，如果为1，e=temp退出循环
                 */
                if ((temp.Gcd(ran)).Equals(BigInteger.One))
                {
                    e = temp;
                }
            } while (!((temp.Gcd(ran)).Equals(BigInteger.One)));

            return e;
        }

        /**
         * 获取私钥(128位)
         * <p>
         * 具体代码如下: <code><font color="red"><br>
         * BigInteger priKey = RSAUtil.getPrivateKey(ran,pKey);//ran是产生公钥的ran变量,pKey是公钥<br>
         * </font></code>
         * 
         * @param ran
         *            通过getRan静态方法计算出来的值
         * @param publicKey
         *            公钥
         * @return
         */
        public static BigInteger getPrivateKey(BigInteger ran, BigInteger publicKey)
        {
            return publicKey.ModInverse(ran);
        }

        /**
         * 对明文进行加密，通过公式 密文=(明文（e次幂） mod m)
         * 
         * @param 明文em
         *            不为空
         * @param 公钥e
         * @param 模数n
         * @return 加密后的密文encodeM
         */
        private static BigInteger[] encodeRSA(byte[][] em, BigInteger e,
                BigInteger n)
        {
            BigInteger[] encodeM = new BigInteger[em.GetLength(0)];
            for (int i = 0; i < em.GetLength(0); i++)
            {
                encodeM[i] = new BigInteger(em[i]);
                encodeM[i] = encodeM[i].ModPow(e, n);
            }
            return encodeM;
        }

        /**
         * 对密文进行解密，通过公式 明文 = （密文（d次幂）mod m）
         * 
         * @param 密文encodeM
         *            不为空
         * @param 密钥d
         * @param 模数n
         * @return 解密后的明文dencodeM
         */
        private static byte[][] dencodeRSA(BigInteger[] encodeM, BigInteger d,
                BigInteger n)
        {
            byte[][] dencodeM = new byte[encodeM.GetLength(0)][];
            int i;
            int lung = encodeM.GetLength(0);
            for (i = 0; i < lung; i++)
            {
                dencodeM[i] = encodeM[i].ModPow(d, n).ToByteArray();
            }
            return dencodeM;
        }

        /**
         * 将数组byte[]arrayByte,转化为二维数组,分段加密/解密
         * 
         * @param arrayByte
         * @param numBytes
         * @return arrayEm 不会为空
         */
        private static byte[][] byteToEm(byte[] arrayByte, int numBytes)
        {
            /**
             * 分段
             */
            int total = arrayByte.GetLength(0);
            int dab = (total - 1) / numBytes + 1, iab = 0;
            byte[][] arrayEm = new byte[dab][];
            int i, j;
            for (i = 0; i < dab; i++)
            {
                arrayEm[i] = new byte[numBytes];

                for (j = 0; j < numBytes && iab < total; j++, iab++)
                {
                    arrayEm[i][j] = arrayByte[iab];
                }
                /**
                 * 补齐空格字符(ox20=32)
                 */
                for (; j < numBytes; j++)
                {
                    arrayEm[i][j] = Convert.ToByte(' ');
                }
            }
            return arrayEm;
        }

        /**
         * 
         * 将二维数组转化为一维数组
         * 
         * @param arraySenS
         * @return
         */
        private static byte[] StringToByte(byte[][] arraySenS)
        {
            int i, dab = 0;
            for (i = 0; i < arraySenS.GetLength(0); i++)
            {
                if (arraySenS[i] == null)
                {
                    return null;
                }
                dab = dab + arraySenS[i].GetLength(0);
            }
            List<Byte> listByte = new List<Byte>();
            int j;
            for (i = 0; i < arraySenS.GetLength(0); i++)
            {
                for (j = 0; j < arraySenS[i].GetLength(0); j++)
                {
                    if (arraySenS[i][j] != 0x20)
                    {
                        listByte.Add(arraySenS[i][j]);
                    }
                }
            }
            Byte[] arrayByte = listByte.ToArray();//.toArray(new Byte[0]);
            byte[] result = new byte[arrayByte.GetLength(0)];
            for (int k = 0; k < arrayByte.GetLength(0); k++)
            {
                result[k] = (arrayByte[k]);//.byteValue();
            }
            listByte = null;
            arrayByte = null;
            return result;
        }

        /**
         *<font color="red"> 加密方法(如果使用了产生密钥功能,则需要同步使用此方法加密)</font>
         * 
         * @param source
         *            ： 明文
         * @param e
         *            公钥
         * @param n
         *            modkey
         * @return 密文 带","
         * @
         */
        public static String encrypt(String source, BigInteger e, BigInteger n)
        {
            return encrypt(source, e, n, NUMBIT * 2);
        }


        /**
         ** 加密方法
         * 
         * @param source
         *            ： 明文
         * @param e
         *            公钥
         * @param n
         * @return 密文 带","
         * @
         */
        public static String encrypt(String source, BigInteger e, BigInteger n,
                int numBit)
        {
            String text = System.Web.HttpUtility.UrlEncode(source, Encoding.UTF8);//URLEncoder.encode(source, "UTF-8");// 为了支持汉字、汉字和英文混排
            if (text == null || "".Equals(text))
            {
                throw new Exception("明文转换为UTF-8,导致转换异常!!!");
            }
            byte[] arraySendM = Encoding.UTF8.GetBytes(text);//text.GetBytes("UTF-8");
            if (arraySendM == null)
            {
                throw new Exception("明文转换为UTF-8,导致转换异常!!!");
            }
            if (numBit <= 1)
            {
                throw new Exception("随机数位数不能少于2!!!");
            }
            int numeroByte = (numBit - 1) / 8;
            byte[][] encodSendM = RSAUtil.byteToEm(arraySendM, numeroByte);
            BigInteger[] encodingM = RSAUtil.encodeRSA(encodSendM, e, n);
            StringBuilder encondSm = new StringBuilder();
            foreach (BigInteger em in encodingM)
            {
                encondSm.Append(em.ToString(16));
                encondSm.Append(" ");
            }
            return encondSm.ToString();
        }

        /**
         * <font color="red"> 解密算法(如果使用了产生密钥功能,则需要同步使用此方法解密)</font>
         * 
         * @param cryptograph
         *            :密文,带","
         * @param d
         *            私钥
         * @param n
         *            modkey
         * @return
         * @
         */
        public static String decrypt(String cryptograph, BigInteger d, BigInteger n)
        {
            return decrypt(cryptograph, d, n, NUMBIT * 2);
        }

        /**
         * 解密算法
         * 
         * @param cryptograph
         *            :密文,带","
         * @param d
         *            私钥
         * @param n
         * @param numBit
         *            位数
         * @return
         * @
         */
        public static String decrypt(String cryptograph, BigInteger d,
                BigInteger n, int numBit)
        {
            String[] chs = cryptograph.Split(' ');
            if (chs == null || chs.GetLength(0) <= 0)
            {
                throw new Exception("密文不符合要求!!!");
            }
            int numeroToken = chs.GetLength(0);
            BigInteger[] StringToByte = new BigInteger[numeroToken];
            for (int i = 0; i < numeroToken; i++)
            {
                StringToByte[i] = new BigInteger(chs[i], 16);
            }
            byte[][] encodeM = RSAUtil.dencodeRSA(StringToByte, d, n);
            byte[] sendMessage = RSAUtil.StringToByte(encodeM);
            //String message = new String(sendMessage, "UTF-8");
            string message = System.Text.Encoding.Default.GetString(sendMessage);
            String result = System.Web.HttpUtility.UrlDecode(message, Encoding.UTF8);//.UrlDecode.decode(message, "UTF-8");
            return result;
        }

      
    }
}
