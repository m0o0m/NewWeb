using System;

namespace GL.ProtocolBack
{
    /// <summary>
    /// AbstractBase 的摘要说明。
    /// </summary>

    [Serializable]
    public abstract class AbstractBase
    {
        protected byte[] initValue;
        public Head header;
        public Bodys body;

        public AbstractBase()
        {

        }

        public virtual byte[] ToBytes()
        {
            return null;
        }
    }
}