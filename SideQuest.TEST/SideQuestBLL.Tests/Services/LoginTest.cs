using FluentAssertions;
using Xunit;
using SideQuest.BLL.Services;
using SideQuest.BLL.Models;

namespace SideQuest_Test.SideQuestBLL.Tests.Services
{
    public class LoginTests
    {
        private readonly LoginService _loginService;
        private readonly FakeTimeProvider _time;

        public LoginTests()
        {
            _time = new FakeTimeProvider { UtcNow = DateTime.UtcNow };
            _loginService = new LoginService(_time);
        }

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Login_ShouldSucceed_WhenCredentialsAreValid()
            => _loginService.AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login("test@test.com", "Pass123!").Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenPasswordIsNotValid()
            => _loginService.AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login("test@test.com", "WrongPass!").Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenEmailDoesNotExistInDatabase()
            => _loginService.Login("unknown@test.com", "Pass123!").Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenEmailIsUppercaseLetters()
            => _loginService.AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login("TEST@TEST.COM", "Pass123!").Should().BeFalse();

        [Theory]
        [MemberData(nameof(LoginTestsData.InvalidLoginData), MemberType = typeof(LoginTestsData))]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenAnyFieldIsEmpty(string email, string password)
            => _loginService.Login(email, password).Should().BeFalse();

        [Theory]
        [MemberData(nameof(LoginTestsData.EmailAndPasswordContainsLeadingOrTrailingSpaces), MemberType = typeof(LoginTestsData))]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenEmailAndPasswordContainsLeadingOrTrailingSpaces(string email, string password)
            => _loginService.AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login(email, password).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_AfterMultipleInvalidAttempts()
            => _loginService.AddUserForTesting(CreateRequest("block@test.com", "Pass123!"))
                .Execute(_ => _.Login("block@test.com", "w1"))
                .Execute(_ => _.Login("block@test.com", "w2"))
                .Execute(_ => _.Login("block@test.com", "w3"))
                .Login("block@test.com", "Pass123!").Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldUnlock_After24Hours()
            => _loginService.AddUserForTesting(CreateRequest("timer@test.com", "Pass123!"))
                .Execute(_ => _.Login("timer@test.com", "w1"))
                .Execute(_ => _.Login("timer@test.com", "w2"))
                .Execute(_ => _.Login("timer@test.com", "w3"))
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddDays(1))
                .Login("timer@test.com", "Pass123!").Should().BeTrue();

        private RegisterRequest CreateRequest(string email, string password)
            => new RegisterRequest
            {
                FirstName = "Maria",
                LastName = "Ionescu",
                County = "Cluj",
                City = "Cluj-Napoca",
                Email = email,
                Password = password,
                ConfirmPassword = password,
                BirthDate = new DateTime(1995, 05, 20),
                PhoneNumber = 744111222,
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            };
    }

    public static class LoginExtensions
    {
        public static T Execute<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}