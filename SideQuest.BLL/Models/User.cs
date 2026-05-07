using SideQuest.BLL.Enums;

namespace SideQuest.BLL.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public bool Permissions { get; set; }
        public List<Interest> Interests { get; set; } = new();
        public Mood Mood { get; set; }

        public Zone UserZone { get; set; }
        public string? ProfilePicture { get; set; }
        public int Points { get; set; }
        public List<Review> Reviews { get; set; } = new();
        public double Lat { get; set; }
        public double Long { get; set; }
        public bool? IsMajor { get; set; }
 
        public void SetName(string name) { }
        public void SetInterests(List<Interest> interests) { }
        public void SetProfilePicture(string picture) { }
        public void SetEmail(string email) { }
        public void SetPassword(string password) { }
        public void SetLocation() { } // complex - API
    }
}