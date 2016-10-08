using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWeb
{
    public class TextControl: Control
    {
        public string Text { get; set; }

        public string Placeholder { get; set; }

        public override ControlType ControlType {
            get { return ControlType.Text; } }
    }
}
