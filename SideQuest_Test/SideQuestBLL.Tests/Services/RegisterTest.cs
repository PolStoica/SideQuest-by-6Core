using FluentAssertions;
using Xunit;
using SideQuest.BLL.Services;
using SideQuest.BLL.Models;

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
                    Email = "test@test.com",
                    Password = "Abc123456_123!", 
                    FullName = "Test User"
                })
                .Should().BeTrue();

        [Fact]
        public void Register_ShouldFail_WhenEmailHasNoAtSymbol()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "testtest.com",
                    Password = "Abc123456_123!",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenEmailHasNoDomain()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@gmail",
                    Password = "Abc123456_123!",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenEmailHasNoUsername()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "@gmail.com",
                    Password = "Abc123456_123!",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenEmailHasUppercaseLetters()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "TEST@GMAIL.COM",
                    Password = "Abc123456_123!",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordTooShort()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "1Ab_",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoNumbers()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "abc_Aambjhcgskjd_",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoLowercaseLetters()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "123_A123455678656_",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoUppercaseLetters()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "123abfgchhruvdbch_",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenPasswordHasNoSpecialCharacters()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@test.com",
                    Password = "123abfgchhruvdbcGGGGG",
                    FullName = "Test User"
                })
                .Should().BeFalse();

        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidRegisterData), MemberType = typeof(RegisterTestsData))]
        public void Register_ShouldFail_WhenAnyFieldIsInvalid(string email, string password, string fullName)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    Password = password,
                    FullName = fullName
                })
                .Should().BeFalse();

        [Fact]
        public void Register_ShouldFail_WhenEmailAlreadyExists_EvenIfOtherDataIsDifferent()
        {
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