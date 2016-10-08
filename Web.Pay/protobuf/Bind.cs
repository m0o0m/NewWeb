using System;
using System.Text;
using ProtoCmd.BackRecharge;

namespace GL.Protocol
{
    /// <summary>
    /// Bind消息
    /// </summary>
    [Serializable]
    public class Bind : AbstractBase
    {
        private CenterCmd centerCmd;
        private byte[] p;

        /// <summary>
        /// 初始Bind命令的消息头
        /// </summary>
        /// <param name="Sequence">序列号</param>
        public Bind(BR_Cmd Command, byte[] _body)
        {
            header = new Head(Command);
            body = new Bodys(_body);
            header.Length = (ushort)(Head.HeaderLength + body.Length);
        }
        public Bind(CenterCmd Command, byte[] _body)
        {
            header = new Head(Command);
            body = new Bodys(_body);
            header.Length = (ushort)(Head.HeaderLength + body.Length);
        }


        public Bind(byte[] receive)
        {
            //initValue = new byte[receive.Length];
            //receive.CopyTo(initValue, 0);

            header = new Head(receive);
            body = new Bodys(receive, Head.HeaderLength);
            header.Length = (ushort)(Head.HeaderLength + body.Length);
        }

        public Bind(Head _header, byte[] _body)
        {
            header = _header;
            body = new Bodys(_body);
            header.Length = (ushort)(Head.HeaderLength + body.Length);
        }


        public int Length
        {
            get
            {
                return header.Length;
            }
        }


        /// <summary>
        /// 把消息结构转换成字节数组
        /// </summary>
        /// <returns>结果字节数组</returns>
        public override byte[] ToBytes()
        {
            byte[] retValue = new byte[this.header.Length];
            int index = 0;

            //填充消息头
            header.ToBytes().CopyTo(retValue, index);

            index += Head.HeaderLength;
            body.ToBytes().CopyTo(retValue, index);

            //移位16位, 填充密码
            //index += 16;
            //Encoding.ASCII.GetBytes(loginPassword).CopyTo(retValue, index);
            return retValue;
        }

    }


}
