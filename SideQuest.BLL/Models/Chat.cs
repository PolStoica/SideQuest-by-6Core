namespace SideQuest.BLL.Models;

public class Chat
{
    public Event Event { get; set; }
    public List<User> Users { get; set; } = new();
    public User Admin { get; set; }
    public User? CoOrganizer { get; set; }

    public void OpenChat()
    {
    }

    public void BlockUser(User user)
    {
    }

    public void KickUser(User user)
    {
    }

    public int GetNumberOfUsers()
    {
        return 0;
    }

    public void PinMessage(string message)
    {
    }

    public bool CanSendMessage(double cooldown, User user)
    {
        return false;
    }

    public void SendReviewLink(string review)
    {
    } // called after event ends
}