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
        if (!map.Events.Any(e => e.Title == this.Title && e.Date == this.Date))
            map.Events.Add(this);
    }

    public void CreateForm(Form form)
    {
        form = new Form();
    }

    public void ApproveRequest(User user)
    {
        if (AvailableSpots == 0)
            throw new InvalidOperationException("Nu mai poti adauga participanti noi");
        Participants.Add(user);
        AvailableSpots--;
        Requests.Remove(user);
    }

    public void DenyRequest(User user)
    {
        Requests.Remove(user);
    }

    public void LeaveReview(Review review)
    {
        if (!Participants.Any(p => p.Email == review.Description))
            Grade = review.Grade;
        Grade = review.Grade;
        EventReview = review.Description;
        Organizer.Reviews.Add(review);
    }

    public void SetRecurrence()
    {
    } // complex
}