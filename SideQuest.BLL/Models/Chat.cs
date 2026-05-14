namespace SideQuest.BLL.Models;

public class Chat
{
    private const double DefaultCooldown = 0.5;
    public Event Event { get; set; }
    public List<User> Users { get; set; } = new();
    public User Admin { get; set; }
    public User? CoOrganizer { get; set; }
    public bool IsOpen { get; private set; } = false;
    public bool IsArchived { get; private set; }
    public bool IsUserMuted(string email)
        => _mutedUserEmails.Contains(email);
    public bool IsUserBanned(string email)
    => _bannedUserEmails.Contains(email);
    public List<string> Messages { get; private set; } = new();
    private HashSet<string> _bannedUserEmails = new();
    private Dictionary<string, DateTime> _lastMessageSentAt = new();
    private HashSet<string> _mutedUserEmails = new();
    public string? PinnedMessage { get; private set; }
    public DateTime LastActivity { get; private set; }

    private void UpdateActivity() => LastActivity = DateTime.UtcNow;

    public void OpenChat()
    {
        if (Admin == null)
            throw new InvalidOperationException("Chat-ul nu poate fi deschis fără un Admin setat.");

        IsOpen = true;
    }

    public void CloseChat()
    {
        if (!IsOpen) return;

        IsOpen = false;
        IsArchived = true; 

        UpdateActivity();
    }

    public void SyncWithEventStatus()
    {
        if (Event == null) return;

        if (Event.IsStarted && !Event.IsFinished)
        {
            OpenChat();
        }
        else if (Event.IsFinished)
        {
            CloseChat();
        }
    }

    private void EnsureChatIsOpen()
    {
        if (!IsOpen)
            throw new InvalidOperationException("Acțiunea este refuzată: Chat-ul este închis.");
    }


    public void ChangeAdmin(User newAdmin)
    {
        EnsureChatIsOpen();
        Admin = newAdmin;
    }

    public void AddOrganizer(User user)
    {
        EnsureChatIsOpen();
        EnsureUserIsNotBanned(user);
        CoOrganizer = user;
    }


    public void BlockUser(User user)
    {
        EnsureChatIsOpen();

        if (user.Email == Admin.Email)
            throw new InvalidOperationException("Nu poți bloca admin-ul.");

        _bannedUserEmails.Add(user.Email);

        Users.RemoveAll(u => u.Email == user.Email);
    }


    public void UnblockUser(User user)
    {
        EnsureChatIsOpen();
        _bannedUserEmails.Remove(user.Email);
    }

    private void EnsureUserIsNotBanned(User user)
    {
        if (_bannedUserEmails.Contains(user.Email))
            throw new InvalidOperationException($"Acces refuzat: Utilizatorul {user.Email} este banat.");
    }

    public void KickUser(User user)
    {
        EnsureChatIsOpen();

        if (user.Email == Admin.Email)
            throw new InvalidOperationException("Nu poți da kick admin-ului.");
        Users.Remove(user);
    }

    public void MuteUser(User user)
    {
        EnsureChatIsOpen();

        if (user.Email == Admin.Email)
            throw new InvalidOperationException("Nu poți pune mute admin-ului.");

        _mutedUserEmails.Add(user.Email);
    }

    public void UnmuteUser(User user)
    {
        EnsureChatIsOpen();
        _mutedUserEmails.Remove(user.Email);
    }

    public int GetNumberOfUsers()
    {
        return Users.Count;
    }

    public void PinMessage(string message, User user)
    {
        EnsureChatIsOpen();
        EnsureUserIsNotBanned(user);

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Mesajul nu poate fi gol.");
        PinnedMessage = message;
    }


    public bool CanSendMessage(User user, double cooldown = 0.5)
    {
        EnsureChatIsOpen();
        EnsureUserIsNotBanned(user);

        if (user.Email == Admin?.Email) return true;

        if (!_lastMessageSentAt.ContainsKey(user.Email))
            return true;

        var secondsSinceLastMessage = (DateTime.UtcNow - _lastMessageSentAt[user.Email]).TotalSeconds;
        return secondsSinceLastMessage >= cooldown;
    }

    public void SendMessage(string text, User user, double cooldown = 0.5)
    {
        EnsureChatIsOpen();
        EnsureUserIsNotBanned(user);

        if (!CanSendMessage(user, cooldown))
            throw new InvalidOperationException("Te rugăm să aștepți înainte de a trimite un nou mesaj.");

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Mesajul nu poate fi gol sau format doar din spații.");

        Messages.Add(text);
        UpdateActivity();
        _lastMessageSentAt[user.Email] = DateTime.UtcNow;
    }

    public void ClearHistory()
    {
        EnsureChatIsOpen();

        _lastMessageSentAt.Clear(); 
        Messages.Clear();        
    }

    public void FinalCleanup()
    {
        _lastMessageSentAt.Clear();
        LastActivity = DateTime.MinValue;
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

