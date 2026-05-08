using FluentAssertions;
using SideQuest.BLL.Enums;
using SideQuest.BLL.Models;
using SideQuest.BLL.Services;
using Xunit;

namespace SideQuest_Test.SideQuestBLL.Tests.Services
{
    public class RegisterTests
    {
        private readonly RegisterService _registerService;

        public RegisterTests()
        {
            _registerService = new RegisterService();
        }

        [Fact]
        public void Register_ShouldSucceed_WhenDataIsValid()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20), 
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "alex.test@example.com",
                    PhoneNumber = 0722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!"
                })
                .Should().BeTrue();

        [Fact]
        public void Register_ShouldFail_WhenEmailHasNoAtSymbol()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "testtest.com",
                    PhoneNumber = 0722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenEmailHasNoDomain()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "test@gmail",
                    PhoneNumber = 0722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenEmailHasNoUsername()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "@gmail.com",
                    PhoneNumber = 0722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!"
                })
                .Should().BeFalse();

        [Theory]
        [InlineData("TEST@GMAIL.COM")]
        [InlineData("Test@gmail.com")]

        public void Register_ShouldSucced_WhenEmailHasUppercaseLetters(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = email,
                    PhoneNumber = 0722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!"
                })
                .Should().BeTrue();

        [Fact]
        public void Register_ShouldFail_WhenPasswordTooShort()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "alex.test@example.com",
                    PhoneNumber = 0722123456,
                    Password = "Abc125_", //under 8 characters
                    ConfirmPassword = "Abc125_"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoNumbers()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "alex.test@example.com",
                    PhoneNumber = 0722123456,
                    Password = "abc_Aambjhcgskjd_",
                    ConfirmPassword = "abc_Aambjhcgskjd_"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoLowercaseLetters()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "alex.test@example.com",
                    PhoneNumber = 0722123456,
                    Password = "123_A123455678656_",
                    ConfirmPassword = "123_A123455678656_"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoUppercaseLetters()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "alex.test@example.com",
                    PhoneNumber = 0722123456,
                    Password = "123abfgchhruvdbch_",
                    ConfirmPassword = "123abfgchhruvdbch_"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoSpecialCharacters()
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = "AlexSideQuest",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                    Email = "alex.test@example.com",
                    PhoneNumber = 0722123456,
                    Password = "123abfgchhruvdbcGGGGG",
                    ConfirmPassword = "123abfgchhruvdbcGGGGG"
                })
                .Should().BeFalse();

        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidRegisterData), MemberType = typeof(RegisterTestsData))]
        public void Register_ShouldFail_WhenAnyFieldIsInvalid(string email, string password, string username, DateTime birthdate, string confirmPassword, int phoneNumber, string profilePicture, List<string> selectedCategories)
            => _registerService
                .Register(new RegisterRequest
                {
                    Username = username,
                    BirthDate = birthdate,
                    ProfilePicture = profilePicture,
                    SelectedCategories = selectedCategories,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    ConfirmPassword = confirmPassword
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenEmailAlreadyExists_EvenIfOtherDataIsDifferent()
        {
            RegisterService.ClearUsers();

            var request1 = new RegisterRequest
            {
                Email = "unique@test.com",
                Password = "Abcd_1234!89",
                FullName = "User One"
            };

            var request2 = new RegisterRequest
            {
                Email = "unique@test.com",
                Password = "Different123!",
                FullName = "User Two"
            };

            _registerService.Register(request1).Should().BeTrue();
            _registerService.Register(request2).Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidFullNames), MemberType = typeof(RegisterTestsData))]
        public void Register_ShouldFail_WhenFullNameHasSpecialCharacters(string email, string password, string fullName)
        {
            _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    Password = password,
                    FullName = fullName
                })
                .Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidFullNameOnlySpecialCharacters), MemberType = typeof(RegisterTestsData))]
        public void Register_ShouldFail_WhenFullNameHasOnlyApostropheOrUnderscore(string email, string password, string fullName)
        {
            _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    Password = password,
                    FullName = fullName
                })
                .Should().BeFalse();
        }

        [Fact]
        public void Register_ShouldFail_WhenFullNameIsTooLong()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "Abc123456_123!",
                    FullName = "Alexandru Alexandru Alexandru Alexandru Alex Alexul"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenFullNameHasInvalidCapitalizationFormat()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "Abc123456_123!",
                    FullName = "AndREI maTEI"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenFullNameExceedsMaximumNumberOfWords()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "Abc123456_123!",
                    FullName = "Alex Alex Alex Alex Alex Alex"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenFullNameIsNotInTitleCase()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "Abc123456_123!",
                    FullName = "test user"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenFullNameHasOnlyOneWord()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "Abc123456_123!",
                    FullName = "Andrei"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenFullNameHasInvalidCapitalization()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "Abc123456_123!",
                    FullName = "TEST USER"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenInputDataHasLeadingOrTrailingSpaces()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = " test@test.com ",
                    Password = " Abc123456_123! ",
                    FullName = " Test User "
                })
                .Should().BeFalse();
    }
}