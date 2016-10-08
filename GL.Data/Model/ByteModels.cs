using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using GL.Common;

namespace GL.Data.Model
{
    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐
    public struct Operator
    {
        public ushort id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)] // 声明一个字符数组，大小为11
        public char[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public char[] pass;
        public Operator(string user, string pass) // 初始化
        {
            this.id = 10000;
            this.name = user.PadRight(11, '\0').ToCharArray();
            this.pass = pass.PadRight(9, '\0').ToCharArray();
        }
    }



    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)] // 按1字节对齐
    public struct SerializeDataHeader
    {
        [MarshalAs(UnmanagedType.U4)]
        public int version;       //4个字节
        [MarshalAs(UnmanagedType.U4)]
        public int num;           //4个字节   所有数据的长度
        //char[] data;	     //这里是存所有类型的数据 包括vip、角色等等
        //public SerializeDataHeader(ushort version, ushort num) // 初始化
        //{
        //    this.version = version;
        //    this.num = num;
        //}

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public int Length
        {
            get
            {
                return (4 + 4);
            }
        }
    }

    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐
    public struct ExtData     //这个是存于SerializeDataHeader的data字段中
    {
        [MarshalAs(UnmanagedType.U4)]
        public int type;          //4个字节 比如说vip是1，其他类推
        [MarshalAs(UnmanagedType.U4)]
        public int num;           //4个字节 该类型存的是data的长度
        //char[] data;        //存该类型的数据

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public int Length
        {
            get
            {
                return (4 + 4);
            }
        }
    }

    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐
    public struct userVip
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 m_grade;            //4个字节 等级
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 m_current;          //4个字节 当前点数
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 m_last_get;         //4个字节 上次领取时间 1970年开始到现在的秒数

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public int Length
        {
            get
            {
                return (4 + 4 + 4);
            }
        }
    };

    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐
    public struct RLEVEL
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 level;                    //4个字节 等级
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 exp;                      //4个字节 经验点数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 168)]
        public char[] reaward_level;           //168个字节 下标数代表等级  值1为无

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public int Length
        {
            get
            {
                return (4 + 4 + 168);
            }
        }
    };

    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐
    public struct userPaijuInfo
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 totalJushu;                  //4个字节 玩的总局数
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 totalWinJushu;               //4个字节 赢的总局数
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 onlineTime;                  //4个字节 总在线时长

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public int Length
        {
            get
            {
                return (4 + 4 + 4);
            }
        }
    };

    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐
    public struct taskDoneInfo          //完成新手任务ID的领取状态
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 task_id;       //4个字节 任务ID
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 status;        //4个字节 奖励领取状态 0为没领  1为已经领取

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public int Length
        {
            get
            {
                return (4 + 4);
            }
        }
    }

    [Serializable] // 指示可序列化
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐
    public struct toolsData                //道具使用数据
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 tools_id;          //4个字节 道具ID
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 bmark;				//4个字节 道具标识用于同一道具存在多个数量
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 get_time;          //4个字节 获得的时间 1970年开始到现在的秒数
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 last_time;         //4个字节 上次使用时间 1970年开始到现在的秒数
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 usetimes;          //4个字节 上次使用的次数 1970年开始到现在的秒数
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 totaltimes;	    //4个字节 总使用次数

        /// <summary>
        /// 消息头的长度
        /// </summary>
        public int Length
        {
            get
            {
                return (4 + 4 + 4 + 4 + 4 + 4);
            }
        }
    }

}

