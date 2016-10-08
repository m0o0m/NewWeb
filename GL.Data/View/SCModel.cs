using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class SCModel
    {
        public bool Static { get; set; }
        public bool ServerIsNotResponding { get; set; }
    }

    public class ModelBaseData
    {
        private string _displayText = "查询结果为空";
        public int ID { get; set; }
        public string Model { get; set; }
        public object DataList { get; set; }
        public string ModelName { get; set; }
        public string Para { get; set; }
        public DateTime Createtime { get; set; }
        public int CheckCount { get; set; }
        public string isError { get { return _displayText; } set { _displayText = value; } }
        
    }
}
