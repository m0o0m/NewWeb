using System;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWeb.Models
{
    public class MLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
