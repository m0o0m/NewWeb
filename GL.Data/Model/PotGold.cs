using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class PotGold
    {
        public int GameType { get; set; }

        public decimal Gold { get; set; }

        public decimal Standard { get; set; }

        public DateTime LastModify { get; set; }
    }
}
