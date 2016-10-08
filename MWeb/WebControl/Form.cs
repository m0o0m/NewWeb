using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWeb
{
    public class Form
    {
        public string FormName { get; set; }

        public List<Control> Controls { get; set; }

        public string Url { get; set; }

        public string ValidateFunc { get; set; }

        public string SucCallBackFunc { get; set; }
      //  public List<EventConfig> EventConfigs { get; set; }
    }
}
