using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public  class Resource
    {
        public string No { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public bool Checked { get; set; }

        public string PNo { get; set; }

        public string Id { get; set; }

        public string Action { get; set; }

        public int Group { get; set; }

        public string LiId { get; set; }

        public int Type { get; set; }

        public string Mark { get; set; }
    }

    public class AspNetUser {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string NickName { get; set; }

    }

    public class AspNetRole {
        public string Id { get; set; }
    }


    public class AspNetUserRoles {
        public string RoleName { get; set; }

    }
}
