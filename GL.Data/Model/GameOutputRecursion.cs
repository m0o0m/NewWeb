using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class GameOutputRecursion
    {
        public int RecordType { get; set; }

        public int PRecordType { get; set; }
        public string RecordName { get; set; }
        public decimal RecordValue { get; set; }
        public int RecordGroup { get; set; }
    }
}
