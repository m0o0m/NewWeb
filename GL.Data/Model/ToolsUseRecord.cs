using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ToolsUseRecord
    {
        public int id { get; set; }
        public DateTime UseTime { get; set; }
        public int PlayerID { get; set; }
        public string UserName { get; set; }
        public int ToolsID { get; set; }
        public string ToolsName { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
        public int ExchangeStatus { get; set; }

    }
}
