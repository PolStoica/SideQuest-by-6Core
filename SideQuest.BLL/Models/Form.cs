namespace SideQuest.BLL.Models
{
    public class Form
    {
        public int? RealLifeExperience { get; set; }
        public int? Difficulty { get; set; }
        public int? PointsExperience { get; set; }
        public double? Price { get; set; }
        public List<FormQuestion> Questions { get; set; } = new(); // empty by default
    }
    
}