using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class GUIModel
    {
        public uint dwUserID {get;set;}
        public string szAccount {get;set;}
        public string szNickName { get; set; }
        public string szTelNum { get; set; }
        public string szEmail { get; set; }
        public bool isOnline {get;set;}
        public bool isFreeze { get; set; }
        public string szTrueName { get; set; }
        public string szIdentity { get; set; }
        public uint sex {get;set;}
        public uint dwAgentID {get;set;}
        public string szCreateTime {get;set;}

        public uint isSearch { get; set; }

    }
}
