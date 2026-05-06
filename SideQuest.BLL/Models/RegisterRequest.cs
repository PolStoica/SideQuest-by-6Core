using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}
