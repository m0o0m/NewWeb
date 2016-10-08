using System;
using System.Collections.Generic;
using System.Web;

namespace GL.Pay
{
    /**
    * 	配置账号信息
    */
    public class Config
    {
        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
#if Debug
        public const int LOG_LEVENL = 3;
#endif
#if P17
        public const int LOG_LEVENL = 3;
#endif
#if Test
        public const int LOG_LEVENL = 2;
#endif
#if Release
        public const int LOG_LEVENL = 1;
#endif

    }
}