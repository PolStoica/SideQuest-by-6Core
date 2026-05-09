using SideQuest.BLL.Enums;

namespace SideQuest.BLL.Models;

public class FormQuestion
{
    public string QuestionText { get; set; } // "Do you have experience?"
    public QuestionType Type { get; set; } // Bool, Int, Double, String
    public bool IsRequired { get; set; } = false; // optional by default
    public object? Answer { get; set; } = null; // null until answered
}