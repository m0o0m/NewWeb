using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class Config
    {
        public bool debugMode {get;set;}
        public int port {get;set;}
        public string serverIp {get;set;}
        public string version {get;set;}
        public string clientVer {get;set;}
        public decimal agent {get;set;}

    }
}
