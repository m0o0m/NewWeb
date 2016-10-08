using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.ProtocolBack
{
    [Serializable]
    public class Bodys
    {
        private byte[] buffer = null;
        private int length = 0;
        public Bodys(byte[] bs)
        {
            if (buffer != null)
            {
                buffer = null;
                GC.Collect();
            }
            length = bs.Length;
            buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = bs[i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bs">数据流</param>
        /// <param name="baseIndex">开始位置</param>
        public Bodys(byte[] bs, int baseIndex) 
         {
             if (buffer != null)
             {
                 buffer = null;
                 GC.Collect();
             }
             length = bs.Length - baseIndex;
             buffer = new byte[length];
 
             for (int i = 0; i < length; i++)
             {
                 buffer[i] = bs[baseIndex + i];
             } 
         }



        /// <summary>
        /// 输出字节流
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return buffer;
        }


        public byte[] Buffer
        {
            get { return buffer; }
        }

        public int Length
        {
            get { return length; }
        }


    }
}
