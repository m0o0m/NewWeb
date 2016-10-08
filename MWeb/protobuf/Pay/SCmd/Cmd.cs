using GL.Protocol;
using ProtoCmd.BackRecharge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace GL.Protocol.Pay
{
    public class Cmd
    {
        internal static Bind runClient(Bind tbind)
        {
            Bind bind;
            try
            {

                byte[] bit = new byte[tbind.header.Length];
                tbind.ToBytes().CopyTo(bit, 0);
                using (TcpClient client = new TcpClient())
                {
                    //10.237.154.102
#if Debug
                    client.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.17"), 5500));
#endif



#if TEST
                    //  10.237.225.129
                    //client.Connect(new IPEndPoint(IPAddress.Parse("10.105.46.18"), 5500));
                    client.Connect(new IPEndPoint(IPAddress.Parse("10.237.225.129"), 5500));
#endif
#if R17
                    client.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.17"), 5500));
#endif
#if Release
                    //client.Connect(new IPEndPoint(IPAddress.Parse("10.237.154.102"), 5500));
#endif

                    using (NetworkStream stream = client.GetStream())
                    {   
                        stream.ReadTimeout = 30000;
                     
                        stream.WriteTimeout = 30000;
                        stream.Write(bit, 0, bit.Length);

                        //List<byte> response = new List<byte>();
                        byte[] ResponseHeadBuffer = new byte[4];
                        int myResponseLength = 0;
                        myResponseLength = stream.Read(ResponseHeadBuffer, 0, ResponseHeadBuffer.Length);
                        Head hhh = new Head(ResponseHeadBuffer);

                        byte[] ResponseBodyBuffer = new byte[hhh.Length - 4];
                        if (hhh.Length > 4)
                        {
                            do
                            {
                                myResponseLength = stream.Read(ResponseBodyBuffer, 0, ResponseBodyBuffer.Length);
                            }
                            while (stream.DataAvailable);
                        }

                        bind = new Bind(hhh, ResponseBodyBuffer);
                        stream.Close();
                    }
                    client.Close();
                }
                return bind;

            }
            catch (Exception error)
            {
                //Bind bind = new Bind(CenterCmd.CS_ACCOUNT_ERROR, new byte[] { });
                return new Bind(CenterCmd.CS_CONNECT_ERROR, new byte[] { });

                //throw new Exception(string.Format("ERROR : {0}", error.ToString()));
            }
            return new Bind(CenterCmd.CS_CONNECT_ERROR, new byte[] { });
        }

    }
}