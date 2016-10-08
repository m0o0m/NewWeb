using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWeb
{
    public class SelectControl: Control
    {
        public override ControlType ControlType
        {
            get { return ControlType.Select; }
        }

        public string Url { get; set; }

    }
}
