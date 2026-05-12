namespace SideQuest.BLL.Models;

public class Chat
{
    public Event Event { get; set; }
    public List<User> Users { get; set; } = new();
    public User Admin { get; set; }
    public User? CoOrganizer { get; set; }
    public bool IsOpen { get; private set; } = false;
    private HashSet<string> _bannedUserEmails = new();
    private Dictionary<string, DateTime> _lastMessageSentAt = new();
    public string? PinnedMessage { get; private set; }
    public void OpenChat()
    {
        IsOpen = true;
    }

    public void BlockUser(User user)
    {
        if (user.Email == Admin.Email)
            throw new InvalidOperationException("Nu poți bloca admin-ul.");
        _bannedUserEmails.Add(user.Email);
    }
    

    public void KickUser(User user)
    {
        if (user.Email == Admin.Email)
            throw new InvalidOperationException("Nu poți da kick admin-ului.");
        Users.Remove(user);
    }

    public int GetNumberOfUsers()
    {
        return Users.Count;
    }

    public void PinMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Mesajul nu poate fi gol.");
        PinnedMessage = message;
    }

    public bool CanSendMessage(double cooldown, User user)
    {
        if (!Users.Any(u => u.Email == user.Email))
            return false;

        if (!_lastMessageSentAt.TryGetValue(user.Email, out var lastSent))
            return true;

        return (DateTime.UtcNow - lastSent).TotalSeconds >= cooldown;
    }

    public void SendReviewLink(string review)
    {
        if (string.IsNullOrWhiteSpace(review))
            throw new ArgumentException("Link-ul nu poate fi gol.");
        foreach (var user in Users)
        {
            Console.WriteLine($"Review link trimis catre {user.Email}: {review}");
        }
    } // called after event ends
}