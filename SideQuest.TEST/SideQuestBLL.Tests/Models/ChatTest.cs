using FluentAssertions;
using SideQuest.BLL.Models;
using Xunit;
using System;
using System.Linq;

namespace SideQuest.Tests
{
    public class ChatTests
    {
        private readonly Chat _chat;
        private readonly User _admin;
        private readonly User _regularUser;

        public ChatTests()
        {
            _admin = new User { Email = "admin@test.com", FirstName = "Admin" };
            _regularUser = new User { Email = "user@test.com", FirstName = "User" };

            _chat = new Chat
            {
                Admin = _admin,
                Users = new List<User> { _admin, _regularUser }
            };
        }

        // --- CHAT IS CLOSED ---


        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void BlockUser_ShouldThrow_WhenChatIsClosed()
        => _chat
            .Invoking(c => c.BlockUser(_regularUser))
            .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void KickUser_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.KickUser(_regularUser))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void PinMessage_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.PinMessage("Test", _admin))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void CanSendMessage_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.CanSendMessage(_regularUser))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void UnblockUser_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.UnblockUser(_regularUser))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void ClearHistory_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.ClearHistory())
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void AddOrganizer_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.AddOrganizer(new User()))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void ChangeAdmin_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.ChangeAdmin(new User()))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void MuteUser_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.MuteUser(_regularUser))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void SendMessage_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.SendMessage("Salut", _regularUser))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void UnmuteUser_ShouldThrow_WhenChatIsClosed()
            => _chat
                .Invoking(c => c.UnmuteUser(_regularUser))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P2")]
        public void CloseChat_ShouldBeIdempotent_AndMaintainGuard()
            => _chat
                .Execute(c => c.CloseChat())
                .Execute(c => c.CloseChat())
                .Invoking(c => c.SendMessage("Test", _admin))
                .Should().Throw<InvalidOperationException>();


        // --- THE OPENING MOMENT ---


        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void OpenChat_ShouldSetIsOpenToTrue()
            => _chat
                .Execute(c => c.OpenChat())
                .IsOpen
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Transition")]
        [Trait("Priority", "P1")]
        public void OpenChat_ShouldEnableAllGuardedMethods()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c =>
                {
                    c.SendMessage("Test Message", _admin);
                    c.PinMessage("Important Announcement", _admin);
                    c.MuteUser(_regularUser);
                    c.UnmuteUser(_regularUser);
                    c.BlockUser(_regularUser);
                    c.UnblockUser(_regularUser);
                    c.KickUser(new User { Email = "temp@test.com" });
                    c.ChangeAdmin(new User { Email = "newadmin@test.com" });
                    c.AddOrganizer(new User { Email = "organizer@test.com" });
                    c.ClearHistory();
                    c.CanSendMessage(_admin);
                })
                .Should().NotThrow();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldNotResetUserList()
            => _chat
                .Execute(c => c.Users.Add(_regularUser))
                .Execute(c => c.OpenChat())
                .Users
                .Should().Contain(_regularUser);

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldNotClearPinnedMessageFromPreviousSession()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.PinMessage("Mesaj Vechi", _admin))
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .PinnedMessage
                .Should().Be("Mesaj Vechi");

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldBeIdempotent_WhenCalledTwice()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.OpenChat())
                .IsOpen
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Transition")]
        [Trait("Priority", "P1")]
        public void OpenChat_ShouldAllow_GetNumberOfUsers_Immediately()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c => c.GetNumberOfUsers())
                .Should().NotThrow();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldMaintain_AdminReference()
            => _chat
                .Execute(c => c.Admin = _admin)
                .Execute(c => c.OpenChat())
                .Admin
                .Should().Be(_admin);

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldKeep_CoOrganizerReference()
            => _chat
                .Execute(c => c.CoOrganizer = _regularUser)
                .Execute(c => c.OpenChat())
                .CoOrganizer
                .Should().Be(_regularUser);

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldRestore_BannedUsersList()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.BlockUser(_regularUser))
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .IsUserBanned(_regularUser.Email)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldInitialize_MessageHistory()
            => _chat
                .Execute(c => c.OpenChat())
                .Messages
                .Should().NotBeNull();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Transition")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldWork_AfterMultipleCloseOpenCycles()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .IsOpen
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldNotReset_CooldownHistory()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.SendMessage("Primul mesaj", _regularUser))
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .CanSendMessage(_regularUser, cooldown: 100.0)
                .Should().BeFalse("istoria de cooldown trebuie să persiste; 100s nu au trecut de la linia de SendMessage.");

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldKeep_MessagesFromPreviousSession()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.SendMessage("Mesaj 1", _admin))
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .Messages.Should().Contain("Mesaj 1", "istoria mesajelor nu trebuie să dispară când închidem/deschidem ușa.");

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void OpenChat_ShouldThrow_WhenAdminIsMissing()
        {
            var brokenChat = new Chat { Admin = null };

            brokenChat.Invoking(c => c.OpenChat())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("*Admin*"); 
        }

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldNotClear_MutedUsersList()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.MuteUser(_regularUser))
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .IsUserMuted(_regularUser.Email)
                .Should().BeTrue("utilizatorii cu mute trebuie să rămână restricționați și după redeschiderea chat-ului.");

        public void OpenChat_ShouldBePossible_OnlyIfEventIsNotNull()
        {
            var orphanChat = new Chat(); 
            orphanChat.Event = null;

            orphanChat.Invoking(c => c.OpenChat())
                .Should().Throw<InvalidOperationException>("nu poți deschide un chat care nu este legat de un eveniment valid.");
        }

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P3")]
        public void OpenChat_ShouldHandle_MassiveUserList()
        {
            for (int i = 0; i < 100; i++)
                _chat.Users.Add(new User { Email = $"user{i}@test.com" });

            _chat.Invoking(c => c.OpenChat())
                .Should().NotThrow("deschiderea chat-ului trebuie să fie performantă indiferent de numărul de participanți.");
        }

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldPreserve_MessageOrder_AcrossRestarts()
        {
            _chat.Execute(c => c.OpenChat())
                 .Execute(c => c.SendMessage("M1", _admin))
                 .Execute(c => c.CloseChat())
                 .Execute(c => c.OpenChat())
                 .Execute(c => c.SendMessage("M2", _admin));

            _chat.Messages.Should().ContainInOrder("M1", "M2");
        }

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P2")]
        public void OpenChat_ShouldNotAllow_BannedUsers_ToReappearInUserList()
            => _chat 
                .Execute(c => c.OpenChat())
                .Execute(c => c.BlockUser(_regularUser))
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .Users 
                .Should().NotContain(u => u.Email == _regularUser.Email);



        // --- THE OPERATING PHASE ---

        // COOLDOWN LOGIC



        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void CanSendMessage_ShouldReturnTrue_OnFirstMessage()
            => _chat
                .Execute(c => c.OpenChat())
                .CanSendMessage(_regularUser)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void CanSendMessage_ShouldReturnFalse_WhenCooldownIsActive()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.SendMessage("Primul", _regularUser))
                .CanSendMessage(_regularUser)
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void CanSendMessage_ShouldReturnTrue_AfterCooldownExpired()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.SendMessage("Primul", _regularUser))
                .CanSendMessage(_regularUser, cooldown: 0)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Constraint")]
        [Trait("Priority", "P1")]
        public void CanSendMessage_ShouldReturnFalse_WhenUserIsBlocked()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.BlockUser(_regularUser))
                .CanSendMessage(_regularUser)
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Constraint")]
        [Trait("Priority", "P1")]
        public void CanSendMessage_ShouldReturnFalse_WhenUserIsKicked()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.KickUser(_regularUser))
                .CanSendMessage(_regularUser)
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void CanSendMessage_ShouldIgnoreCooldown_ForAdmin()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.SendMessage("Anunt 1", _admin))
                .CanSendMessage(_admin)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void CanSendMessage_ShouldWork_ForCoOrganizer()
        {
            _chat.OpenChat();
            _chat.CoOrganizer = _regularUser;
            _chat.CanSendMessage(_regularUser)
                 .Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P3")]
        public void CanSendMessage_ShouldHandle_MassiveCooldownTime()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.SendMessage("Salut", _regularUser))
                .CanSendMessage(_regularUser, cooldown: 31536000) 
                .Should().BeFalse();


        // BLOCK/KICK/GetNumberOfUsers
        

        [Fact]
        [Trait("Feature", "Moderation")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void BlockUser_ShouldAddUserToBannedList()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.BlockUser(_regularUser))
                .IsUserBanned(_regularUser.Email)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void BlockUser_ShouldThrow_WhenTargetIsAdmin()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c => c.BlockUser(_admin))
                .Should().Throw<InvalidOperationException>();


        [Fact]
        [Trait("Feature", "Moderation")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void KickUser_ShouldRemoveUserFromList()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.KickUser(_regularUser))
                .Users
                .Should().NotContain(_regularUser);

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void KickUser_ShouldThrow_WhenTargetIsAdmin()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c => c.KickUser(_admin))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Moderation")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void KickUser_ShouldWork_ForRegularUser()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c => c.KickUser(_regularUser))
                .Should().NotThrow();

        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void GetNumberOfUsers_ShouldReflectActualCount()
            => _chat
                .Execute(c => c.OpenChat())
                .GetNumberOfUsers()
                .Should().Be(2);

        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void GetNumberOfUsers_ShouldDecrease_AfterKick()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.KickUser(_regularUser))
                .GetNumberOfUsers()
                .Should().Be(1);

        [Fact]
        [Trait("Feature", "Moderation")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P3")]
        public void BlockUser_ShouldWork_ForUserAlreadyKicked()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.KickUser(_regularUser))
                .Execute(c => c.BlockUser(_regularUser))
                .IsUserBanned(_regularUser.Email)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P2")]
        public void BlockUser_ShouldThrow_WhenUserIsNotInChat()
        {
            var stranger = new User { Email = "not_invited@test.com" };

            _chat.Execute(c => c.OpenChat())
                 .Invoking(c => c.BlockUser(stranger))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("*nu face parte din acest chat*");
        }

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P2")]
        public void KickUser_ShouldThrow_WhenUserIsNotInChat()
        {
            var stranger = new User { Email = "not_invited@test.com" };

            _chat.Execute(c => c.OpenChat())
                 .Invoking(c => c.KickUser(stranger))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("*nu face parte din acest chat*");
        }


        // LOGICA DE PIN MESSAGE 


        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void PinMessage_ShouldUpdatePinnedMessageField()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.PinMessage("Anunt Nou", _admin))
                .PinnedMessage
                .Should().Be("Anunt Nou");

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void PinMessage_ShouldOverwrite_ExistingPin()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.PinMessage("Vechi", _admin))
                .Execute(c => c.PinMessage("Nou", _admin))
                .PinnedMessage
                .Should().Be("Nou");

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P2")]
        public void PinMessage_ShouldThrow_WhenMessageIsNull()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c => c.PinMessage(null, _admin))
                .Should().Throw<ArgumentException>();

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P2")]
        public void PinMessage_ShouldThrow_WhenMessageIsWhiteSpace()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c => c.PinMessage("   ", _admin))
                .Should().Throw<ArgumentException>();

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P3")]
        public void PinMessage_ShouldHandle_LongStrings()
        {
            var longMessage = new string('A', 500);
            _chat.OpenChat();
            _chat.Execute(c => c.PinMessage(longMessage, _admin))
                 .PinnedMessage.Length
                 .Should().Be(5000);
        }

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P3")]
        public void PinMessage_ShouldHandle_SpecialCharacters()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.PinMessage("!@#$%^&*()_+", _admin))
                .PinnedMessage
                .Should().Be("!@#$%^&*()_+");

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void PinMessage_ShouldWork_ForCoOrganizer()
        {
            _chat.OpenChat();
            _chat.CoOrganizer = _regularUser;
            _chat.Invoking(c => c.PinMessage("Pin by CoOrg", _regularUser))
                 .Should().NotThrow();
        }

        [Fact]
        [Trait("Feature", "Security")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void PinMessage_ShouldThrow_ForRegularUser()
            => _chat
                .Execute(c => c.OpenChat())
                .Invoking(c => c.PinMessage("Hack", _regularUser))
                .Should().Throw<UnauthorizedAccessException>();

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P3")]
        public void PinMessage_ShouldAccept_Emojis()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.PinMessage("📌 Welcome! 🔥", _admin))
                .PinnedMessage
                .Should().Be("📌 Welcome! 🔥");

        [Fact]
        [Trait("Feature", "Messaging")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P3")]
        public void PinMessage_ShouldNotThrow_WhenMessageIsSameAsOldPin()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.PinMessage("Repetitie", _admin))
                .Invoking(c => c.PinMessage("Repetitie", _admin))
                .Should().NotThrow();



        // USER MANAGEMENT LOGIC



        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void UsersList_ShouldNotAllow_DuplicateEmails()
        {
            _chat
                 .OpenChat();
            var duplicate = new User { Email = _regularUser.Email };
            _chat.Users
                 .Add(duplicate);
            _chat.Users.GroupBy(u => u.Email)
                 .Any(g => g.Count() > 1)
                 .Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Admin_ShouldNotBeNullable_AfterInit()
            => _chat
                    .Admin  
                    .Should().NotBeNull();

        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void CoOrganizer_ShouldBeAssigned_DuringChat()
        {
            var newHelper = new User { Email = "helper@test.com" };
            _chat.OpenChat();
            _chat.CoOrganizer = newHelper;
            _chat.CoOrganizer
                 .Email.Should().Be("helper@test.com");
        }

        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void CoOrganizer_ShouldBeChangeable_DuringChat()
        {
            var firstHelper = new User { Email = "first@test.com" };
            var secondHelper = new User { Email = "second@test.com" };
            _chat.CoOrganizer = firstHelper;
            _chat.OpenChat();

            _chat.CoOrganizer = secondHelper;

            _chat.CoOrganizer.Email
                 .Should().Be("second@test.com");

            _chat.CoOrganizer.Email
                 .Should().NotBe("first@test.com");
        }

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void BannedList_ShouldPersist_BetweenSessions()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.BlockUser(_regularUser))
                .Execute(c => c.CloseChat())
                .Execute(c => c.OpenChat())
                .IsUserBanned(_regularUser.Email)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Users_ShouldContain_Admin_ByDefault()
            => _chat
                    .Users
                    .Should().Contain(u => u.Email == _admin.Email);
        [Fact]
        [Trait("Feature", "Management")]
        [Trait("Type", "State")]
        [Trait("Priority", "P3")]
        public void Chat_ShouldUpdate_LastActivityTimestamp()
        {
            _chat.OpenChat();
            var timeBeforeMessage = _chat.LastActivity;

            System.Threading.Thread.Sleep(1);

            _chat.SendMessage("Activity Check", _regularUser);

            _chat.LastActivity
                 .Should().BeAfter(timeBeforeMessage, "orice mesaj trimis trebuie să actualizeze marcajul temporal al activității.");
        }



        // POST - EVENT & CLOSEING


        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void CloseChat_ShouldSetIsOpenToFalse()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .IsOpen
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void CloseChat_ShouldBeIdempotent()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .Invoking(c => c.CloseChat())
                .Should().NotThrow();

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void SendReviewLink_ShouldWork_WhenChatIsClosed()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .Invoking(c => c.SendReviewLink("https://survey.com/123"))
                .Should().NotThrow();


        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void SendReviewLink_ShouldThrow_WhenLinkIsEmpty()
            => _chat
                .Invoking(c => c.SendReviewLink(""))
                .Should().Throw<ArgumentException>();

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void SendReviewLink_ShouldThrow_WhenLinkIsInvalidFormat()
            => _chat
                .Invoking(c => c.SendReviewLink("invalid-link"))
                .Should().Throw<UriFormatException>();

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P3")]
        public void SendReviewLink_ShouldWork_EvenIfNoUsersPresent()
        {
            _chat.Users.Clear();
            _chat.Invoking(c => c.SendReviewLink("https://survey.com"))
                 .Should().NotThrow();
        }

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void CloseChat_ShouldBlock_FurtherMessages()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .Invoking(c => c.SendMessage("Mai e cineva?", _admin))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P2")]
        public void CloseChat_ShouldBlock_FurtherPins()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .Invoking(c => c.PinMessage("Pin post-mortem", _admin))
                .Should().Throw<InvalidOperationException>();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void PinnedMessage_ShouldStillBeReadable_AfterClose()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.PinMessage("Anunt Final", _admin))
                .Execute(c => c.CloseChat())
                .PinnedMessage
                .Should().Be("Anunt Final");

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void GetNumberOfUsers_ShouldStillBeReadable_AfterClose()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .GetNumberOfUsers()
                .Should().BeGreaterThan(0);

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P3")]
        public void SendReviewLink_ShouldNotThrow_IfReviewStringIsVeryLong()
        {
            var longLink = "https://survey.com/" + new string('a', 2000);
            _chat
                .Invoking(c => c.SendReviewLink(longLink))
                .Should().NotThrow();
        }

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void CloseChat_ShouldNotClear_BannedList()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.BlockUser(_regularUser))
                .Execute(c => c.CloseChat())
                .IsUserBanned(_regularUser.Email)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void CloseChat_ShouldNotClear_UserList()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .Users
                .Should().NotBeEmpty();

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P3")]
        public void SendReviewLink_ShouldBeCallable_MultipleTimes()
            => _chat
                .Execute(c => c.SendReviewLink("https://link1.com"))
                .Invoking(c => c.SendReviewLink("https://link2.com"))
                .Should().NotThrow();

        [Fact]
        [Trait("Feature", "Lifecycle")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void ChatState_ShouldBeArchived_AfterClose()
            => _chat
                .Execute(c => c.OpenChat())
                .Execute(c => c.CloseChat())
                .IsArchived
                .Should().BeTrue("un chat închis trebuie marcat ca arhivat pentru baza de date.");

        [Fact]
        [Trait("Feature", "Cleanup")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P3")]
        public void FinalCleanup_ShouldRemove_AllTimestamps()
        {
            _chat.Execute(c => c.OpenChat())
                 .Execute(c => c.SendMessage("Test", _regularUser))
                 .Execute(c => c.CloseChat())
                 .Execute(c => c.FinalCleanup());

            _chat.LastActivity.Should().Be(DateTime.MinValue);
        }

    }

    public static class ChatTestExtensions
    {
        public static Chat Execute(this Chat chat, Action<Chat> action)
        {
            action(chat);
            return chat;
        }
    }

}