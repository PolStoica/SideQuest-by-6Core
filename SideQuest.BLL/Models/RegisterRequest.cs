using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Models
{
    public class RegisterRequest
    {
        public string Username { get; set; }

        public DateTime BirthDate { get; set; }

        public int PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }

        public string Email { get; set; }

        public List<string> SelectedCategories { get; set; } = new List<string>();


        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
