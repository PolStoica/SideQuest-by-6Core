using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Models
{
    public class RegisterRequest
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }


        public string County { get; set; }
        public string City { get; set; }


        public DateTime BirthDate { get; set; }

        public long PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }

        public string Email { get; set; }


        public string Password { get; set; }

        public string ConfirmPassword { get; set; }


        public List<string> SelectedCategories { get; set; } = new List<string>();

    }
}
