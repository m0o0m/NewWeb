using GL.Command.DBUtility;
using GL.ProtocolBack;
using ProtoCmd.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace MWeb.protobuf.SCmd
{
    public class Cmd
    {
        public static readonly string serverip = PubConstant.GetConnectionString("serverhost");
        public static readonly int serverport = PubConstant.GetConnectionString("serverport")==""?0:
            Convert.ToInt32(PubConstant.GetConnectionString("serverport"));
     

        internal static Bind runClient(Bind tbind)
        {
            

            Bind bind;
            try
            {

                byte[] bit = new byte[tbind.header.Length];
                tbind.ToBytes().CopyTo(bit, 0);
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse(serverip), serverport));


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