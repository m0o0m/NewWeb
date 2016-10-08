using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GL.Instance
{

    public class OnlineStatic
    {
        public int Id { get; set; }
        public DateTime LastTime { get; set; }
        public bool IsLeave
        {
            get {
                return DateTime.Compare(DateTime.Now, LastTime.AddMinutes(10)) > 0;
            }
        }
    }



}