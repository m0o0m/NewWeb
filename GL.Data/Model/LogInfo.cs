using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class LogInfo
    {
        public string UserAccount { get; set; }

        public DateTime CreateTime { get; set; }

        public string LoginIP { get; set; }

        public string OperModule { get; set; }

        public string Content { get; set; }

        public string Detail { get; set; }

        public string NickName { get; set; }
    }
}
