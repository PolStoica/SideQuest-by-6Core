using SideQuest.BLL.Enums;

namespace SideQuest.BLL.Models;

public class Event
{
    public string Title { get; set; }
    public string? Description { get; set; } // Markdown
    public bool IsRecurring { get; set; }
    public Mood Mood { get; set; }
    public Category Category { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public int Distance { get; set; }
    public string Emoji { get; set; }
    public string? Picture { get; set; }
    public DateTime Date { get; set; }
    public double Duration { get; set; }
    public int AvailableSpots { get; set; }
    public int? Points { get; set; }
    public User Organizer { get; set; }
    public List<User> Participants { get; set; } = new();
    public List<User> Requests { get; set; } = new();
    public Form? Form { get; set; }

    // Post-event
    public int? Grade { get; set; }
    public string? EventReview { get; set; }

    public void SetPin(Map map)
    {
    }

    public void CreateForm(Form form)
    {
    }

    public void ApproveRequest(User user)
    {
    }

    public void DenyRequest(User user)
    {
    }

    public void LeaveReview(Review review)
    {
    }

    public void SetRecurrence()
    {
    } // complex
}