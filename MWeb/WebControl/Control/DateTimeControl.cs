using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWeb
{
    public  class DateTimeControl:Control
    {
        public override ControlType ControlType
        {
            get { return ControlType.DateTime; }
        }
    }
}
