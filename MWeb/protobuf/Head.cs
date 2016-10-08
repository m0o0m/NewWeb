using System;
using ProtoCmd.Service;
using GL.Common;

namespace GL.Protocol
{
    /// <summary>
    /// 消息头
    /// </summary>
    [Serializable]
    public class Head
    {
        private byte[] initValue = new byte[Head.HeaderLength];

        public Head(ServiceCmd CommandID)
        {
            Converter.Int16ToBytes((ushort)CommandID).CopyTo(initValue, 2);
        }

        public Head(CenterCmd CommandID)
        {
            Converter.Int16ToBytes((ushort)CommandID).CopyTo(initValue, 2);
        }

        public Head(byte[] bs)
        {
            int length = Head.HeaderLength;

            for (int i = 0; i < length; i++)
            {
                initValue[i] = bs[i];
            }
        }


        public Head(byte[] bs, int baseIndex) 
         {
             int length = Head.HeaderLength;
 
             for (int i = 0; i < length; i++)
             {
                 initValue[i]=bs[baseIndex+i];
             } 
         }

        /// <summary>
        /// 消息的整个长度
        /// </summary>
        public ushort Length
        {
            get
            {
                //return (Converter.BytesToUInt(initValue, 4));
                return (Converter.BytesToUInt16(initValue, 0));
            }
            set
            {
                byte[] byt = Converter.Int16ToBytes(value);
                for (int i = 0; i < 2; i++)
                {
                    initValue[i] = byt[i];
                }
            }
        }

        /// <summary>
        /// 命令类型
        /// </summary>
        public ushort CommandID
        {
            get
             {
                 return (Converter.BytesToUInt16(initValue, 2));
             }
            set
            {
                byte[] t = Converter.Int16ToBytes(value);
                for (int i = 0; i < 2; i++)
                {
                    initValue[i+2] = t[i];
                }
            }
        }


        /// <summary>
        /// 输出字节流
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return initValue;
        }

        /// <summary>
        /// 从字节流中转换
        /// </summary>
        /// <param name="bs"></param>
        public void FromBytes(byte[] bs)
        {
            for (int i = 0; i < Head.HeaderLength; i++)
            {
                initValue[i] = bs[i];
            }
        }

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public static int HeaderLength
        {
            get
            {
                return (2 + 2);
            }
        }
    }
}
