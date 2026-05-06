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
            _time = new FakeTimeProvider
            {
                UtcNow = DateTime.UtcNow
            };

            _loginService = new LoginService(_time);
        }

        [Fact]
        public void Login_ShouldSucceed_WhenCredentialsAreValid()
        {
            var request = new RegisterRequest
            {
                Email = "test@test.com",
                Password = "Abc123456!",
                FullName = "Test User"
            };

            _loginService.AddUserForTesting(request);

            _loginService.Login(request.Email, request.Password)
                .Should().BeTrue();
        }

        [Fact]
        public void Login_ShouldFail_WhenPasswordIsNotValid()
        {
            var request = new RegisterRequest
            {
                Email = "test@test.com",
                Password = "Abc123456!",
                FullName = "Test User"
            };

            _loginService.AddUserForTesting(request);

            _loginService.Login(request.Email, "WrongPassword124!")
                .Should().BeFalse();
        }

        [Fact]
        public void Login_ShouldFail_WhenEmailDoesNotExistInDatabase()
        {
            _loginService
                .Login("notregistered@test.com", "Abc123456!")
                .Should().BeFalse();
        }

        [Fact]
        public void Login_ShouldFail_WhenEmailIsUppercaseLetters()
        {
            var request = new RegisterRequest
            {
                Email = "test@test.com",
                Password = "Abc123456!",
                FullName = "Test User"
            };

            _loginService.AddUserForTesting(request);

            _loginService.Login("TEST@TEST.COM", request.Password)
                .Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(LoginTestsData.InvalidLoginData), MemberType = typeof(LoginTestsData))]
        public void Login_ShouldFail_WhenAnyFieldIsEmpty(string email, string password)
        {
            _loginService.Login(email, password)
                .Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(LoginTestsData.EmailAndPasswordContainsLeadingOrTrailingSpaces), MemberType = typeof(LoginTestsData))]
        public void Login_ShouldFail_WhenEmailAndPasswordContainsLeadingOrTrailingSpaces(string email, string password)
        {
            var request = new RegisterRequest
            {
                Email = "test@test.com",
                Password = "Abc123456!",
                FullName = "Test User"
            };

            _loginService.AddUserForTesting(request);

            _loginService.Login(email, password)
                .Should().BeFalse();
        }

        [Fact]
        public void Login_ShouldFail_AfterMultipleInvalidAttempts()
        {
            var email = "block@test.com";

            _loginService.AddUserForTesting(new RegisterRequest
            {
                Email = email,
                Password = "Abc123456!",
                FullName = "Test User"
            });

            _loginService.Login(email, "wrong1").Should().BeFalse();
            _loginService.Login(email, "wrong2").Should().BeFalse();
            _loginService.Login(email, "wrong3").Should().BeFalse();

            _loginService.Login(email, "Abc123456!")
                .Should().BeFalse();
        }

        [Fact]
        public void Login_ShouldUnlock_After24Hours()
        {
            var email = "timer@test.com";

            _loginService.AddUserForTesting(new RegisterRequest
            {
                Email = email,
                Password = "Abc123456!",
                FullName = "Test User"
            });

            _loginService.Login(email, "wrong1").Should().BeFalse();
            _loginService.Login(email, "wrong2").Should().BeFalse();
            _loginService.Login(email, "wrong3").Should().BeFalse();

            _time.UtcNow = _time.UtcNow.AddDays(1);

            _loginService.Login(email, "Abc123456!")
                .Should().BeTrue();
        }
    }
}