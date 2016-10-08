using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWeb
{
    public class Control
    {
      

        public string Name { get; set; }

        public string ID { get; set; }

        public string ViewName { get; set; }

        public string Style { get; set; }

        public virtual ControlType ControlType { get;  }


    }

}
