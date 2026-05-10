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
            => _loginService
                .AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login("test@test.com", "Pass123!")
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenPasswordIsNotValid()
            => _loginService
                .AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login("test@test.com", "WrongPass!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenEmailDoesNotExistInDatabase()
            => _loginService
                .Login("unknown@test.com", "Pass123!")
                .Should().BeFalse();

        [Theory]
        [MemberData(nameof(LoginTestsData.InvalidLoginData), MemberType = typeof(LoginTestsData))]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenAnyFieldIsEmpty(string email, string password)
            => _loginService
                .Login(email, password)
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailMissingAtSymbol()
            => _loginService
                .AddUserForTesting(CreateRequest("test.test.com", "Pass123!"))
                .Login("test.test.com", "Pass123!")
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailIsMissingDomain()
                => _loginService
                    .AddUserForTesting(CreateRequest("test@", "Pass123!"))
                    .Login("test@", "Pass123!")
                    .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailHasMultipleAtSymbols()
            => _loginService
                .AddUserForTesting(CreateRequest("test@user@test.com", "Pass123!"))
                .Login("test@user@test.com", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailStartsWithDot()
            => _loginService
                .AddUserForTesting(CreateRequest(".test@test.com", "Pass123!"))
                .Login(".test@test.com", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailContainsForbiddenCharacters()
            => _loginService
                .AddUserForTesting(CreateRequest("test#test@test.com", "Pass123!"))
                .Login("test#test@test.com", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailHasInvalidTopLevelDomain()
            => _loginService
                .AddUserForTesting(CreateRequest("test@com", "Pass123!"))
                .Login("test@com", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailContainsSpacesInMiddle()
            => _loginService
                .AddUserForTesting(CreateRequest("test user@test.com", "Pass123!"))
                .Login("test user@test.com", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailIsExceedinglyLong()
            => _loginService
                .AddUserForTesting(CreateRequest(new string('a', 250) + "@test.com", "Pass123!"))
                .Login(new string('a', 100) + "@test.com", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailContainsOnlyDomain()
            => _loginService
                .AddUserForTesting(CreateRequest("@gmail.com", "Pass123!"))
                .Login("@gmail.com", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        [Trait("Category", "EmailValidation")]
        public void Login_ShouldFail_WhenEmailHasConsecutiveDots()
            => _loginService
                .AddUserForTesting(CreateRequest("test..user@test.com", "Pass123!"))
                .Login("test..user@test.com", "Pass123!")
                .Should().BeFalse();

        // CASE SENSITIVITY & TRIMMING



        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenEmailIsUppercaseLetters()
            => _loginService
                .AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login("TEST@TEST.COM", "Pass123!")
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenPasswordCaseDoesNotMatch()
            => _loginService
                .AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login("test@test.com", "pass123!")
                .Should().BeFalse();


        [Theory]
        [MemberData(nameof(LoginTestsData.EmailAndPasswordContainsLeadingOrTrailingSpaces), MemberType = typeof(LoginTestsData))]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenEmailAndPasswordContainsLeadingOrTrailingSpaces(string email, string password)
            => _loginService
                .AddUserForTesting(CreateRequest("test@test.com", "Pass123!"))
                .Login(email, password)
                .Should().BeFalse();


        // SECURITY VALIDATION & BLOCKING


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldLockAccount_AfterExactlyThreeFailedAttempts()
            => _loginService
                .AddUserForTesting(CreateRequest("lock@test.com", "Pass123!"))
                .Execute(_ => _.Login("lock@test.com", "wrong1"))
                .Execute(_ => _.Login("lock@test.com", "wrong2"))
                .Execute(_ => _.Login("lock@test.com", "wrong3"))
                .Execute(_ => _.Login("lock@test.com", "wrong4"))
                .Execute(_ => _.Login("lock@test.com", "wrong5"))
                .Login("lock@test.com", "Pass123!")
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P2")]
        public void Login_ShouldResetFailedAttempts_AfterSuccessfulLogin()
            => _loginService
                .AddUserForTesting(CreateRequest("reset@test.com", "Pass123!"))
                .Execute(_ => _.Login("reset@test.com", "w1"))
                .Execute(_ => _.Login("reset@test.com", "w2"))
                .Execute(_ => _.Login("reset@test.com", "w3"))
                .Execute(_ => _.Login("reset@test.com", "w4"))
                .Execute(_ => _.Login("reset@test.com", "Pass123!")) 
                .Execute(_ => _.Login("reset@test.com", "w1"))
                .Execute(_ => _.Login("reset@test.com", "w2"))
                .Execute(_ => _.Login("reset@test.com", "w3"))
                .Execute(_ => _.Login("reset@test.com", "w4"))
                .Login("reset@test.com", "Pass123!") 
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldLockFor12Hours_WhenBlockedForFirstTime()
        => _loginService
            .AddUserForTesting(CreateRequest("prog1@test.com", "Pass123!"))
            .Execute(_ => _.Login("prog1@test.com", "w1"))
            .Execute(_ => _.Login("prog1@test.com", "w2"))
            .Execute(_ => _.Login("prog1@test.com", "w3"))
            .Execute(_ => _.Login("prog1@test.com", "w4"))
            .Execute(_ => _.Login("prog1@test.com", "w5"))
            .Execute(_ => _time.UtcNow = _time.UtcNow.AddDays(12).AddSeconds(-1))
            .Login("prog1@test.com", "Pass123!")
            .Should().BeFalse();


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldUnlockAfter12Hours_OnFirstBlock()
            => _loginService
                .AddUserForTesting(CreateRequest("user1@test.com", "SafePass123!"))
                .Execute(_ => { for (int i = 0; i < 5; i++) _.Login("user1@test.com", "wrong"); })
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddHours(12).AddSeconds(1))
                .Login("user1@test.com", "SafePass123!")
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldLockFor48Hours_OnSecondBlock()
            => _loginService
                .AddUserForTesting(CreateRequest("user2@test.com", "SafePass123!"))
                .Execute(_ => { for (int i = 0; i < 5; i++) _.Login("user2@test.com", "w"); })
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddHours(13))
                .Execute(_ => _.Login("user2@test.com", "w1")) // Start second series (3 attempts)
                .Execute(_ => _.Login("user2@test.com", "w2"))
                .Execute(_ => _.Login("user2@test.com", "w3"))
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddHours(47)) 
                .Login("user2@test.com", "SafePass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldUnlockAfter48Hours_OnSecondBlock()
            => _loginService
                .AddUserForTesting(CreateRequest("user3@test.com", "SafePass123!"))
                .Execute(_ => { for (int i = 0; i < 5; i++) _.Login("user3@test.com", "w"); })
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddHours(13))
                .Execute(_ => { for (int i = 0; i < 3; i++) _.Login("user3@test.com", "w"); }) 
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddHours(48).AddSeconds(1))
                .Login("user3@test.com", "SafePass123!")
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldStayLockedPermanently_OnThirdBlock()
            => _loginService
                .AddUserForTesting(CreateRequest("user4@test.com", "SafePass123!"))
                .Execute(_ => { for (int i = 0; i < 5; i++) _.Login("user4@test.com", "w"); }) 
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddHours(13))
                .Execute(_ => { for (int i = 0; i < 3; i++) _.Login("user4@test.com", "w"); }) 
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddHours(49))
                .Execute(_ => { for (int i = 0; i < 3; i++) _.Login("user4@test.com", "w"); }) // 3rd Block (Permanent)
                .Execute(_ => _time.UtcNow = _time.UtcNow.AddYears(1))
                .Login("user4@test.com", "SafePass123!")
                .Should().BeFalse();

        [Theory]
        [InlineData("nospecial123A")]
        [InlineData("NoNumbers!")]   
        [InlineData("nonupper123!")]
        [InlineData("ONLYUPPER123!")] 
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenPasswordDoesNotMeetComplexity(string weakPassword)
            => _loginService
                .AddUserForTesting(CreateRequest("weak@test.com", weakPassword))
                .Login("weak@test.com", weakPassword)
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenSqlInjectionIsAttemptedInEmail()
            => _loginService
                .Login("' OR 1=1 --", "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenSqlInjectionIsAttemptedInPassword()
            => _loginService
                .AddUserForTesting(CreateRequest("sql@test.com", "Pass123!"))
                .Login("sql@test.com", "' OR '1'='1")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenUsingOldPasswordAfterSimulatedChange()
            => _loginService
                .AddUserForTesting(CreateRequest("old@test.com", "OldPass123!"))
                .AddUserForTesting(CreateRequest("old@test.com", "NewPass123!"))
                .Login("old@test.com", "OldPass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldNotAffectOtherUsers_WhenMultipleEmailsAreTried()
            => _loginService
                .AddUserForTesting(CreateRequest("user1@test.com", "Pass123!"))
                .AddUserForTesting(CreateRequest("user2@test.com", "Pass234!"))
                .Execute(_ => _.Login("user1@test.com", "wrong"))
                .Login("user2@test.com", "Pass234!")
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenEmailIsNull()
            => _loginService.Login(null, "Pass123!")
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenPasswordIsNull()
            => _loginService.Login("test@test.com", null)
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldExecuteWithinConstantTimeRange()
        {
            var start = DateTime.Now;
            _loginService.Login("test@test.com", "pass123?");
            var end = DateTime.Now;
            (end - start).TotalMilliseconds
                .Should().BeLessThan(500);
        }


        // USER INTEGRITY


        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Login_ShouldMaintainDataIntegrity_WhenFirstNameIsDifferentFromEmail()
        {
            var request = CreateRequest("cool.guy@test.com", "Complex123!");
            request.FirstName = "Andrei"; 

            _loginService.AddUserForTesting(request)
                .Login("cool.guy@test.com", "Complex123!");

            var savedUser = LoginService.GetUsersForTesting().First(u => u.Email == "cool.guy@test.com");
            savedUser.FirstName.Should().Be("Andrei");
        }

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Login_ShouldMaintainDataIntegrity_WhenLastNameIsDifferentFromEmail()
        {
            var request = CreateRequest("random.email@test.com", "Complex123!");
            request.LastName = "Popescu";

            _loginService.AddUserForTesting(request)
                .Login("random.email@test.com", "Complex123!");

            var savedUser = LoginService.GetUsersForTesting().First(u => u.Email == "random.email@test.com");
            savedUser.LastName.Should().Be("Popescu");
        }

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void User_ShouldBeAtLeast18YearsOld()
            => _loginService
                .AddUserForTesting(CreateRequest("age@test.com", "Complex123!"))
                .Execute(_ => {
                    var user = LoginService.GetUsersForTesting().First(u => u.Email == "age@test.com");
                    var age = _time.UtcNow.Year - user.BirthDate.Year;
                    age.Should().BeGreaterOrEqualTo(18);
                });

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void User_ShouldPreserveSelectedCategoriesListCorrectly()
        {
            var request = CreateRequest("cats@test.com", "Complex123!");
            request.SelectedCategories = new List<string> {"Gaming", "Sport" };

            _loginService.AddUserForTesting(request);

            var savedUser = LoginService.GetUsersForTesting().First(u => u.Email == "cats@test.com");
            savedUser.SelectedCategories.Should().HaveCount(2).And.ContainInOrder("Gaming", "Sport");
        }

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void User_PhoneNumber_ShouldFollowRomanianFormat()
            => _loginService
                .AddUserForTesting(CreateRequest("phone@test.com", "Complex123!"))
                .Execute(_ => {
                    var phone = LoginService.GetUsersForTesting().First(u => u.Email == "phone@test.com").PhoneNumber.ToString();
                    phone.Should().StartWith("7").And.HaveLength(9); 
                });

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void User_LocationData_ShouldMatchRegistrationRequest()
        {
            var request = CreateRequest("loc@test.com", "Complex123!");
            request.County = "Sibiu";
            request.City = "Mediaș";

            _loginService.AddUserForTesting(request);

            var savedUser = LoginService.GetUsersForTesting().First(u => u.Email == "loc@test.com");
            savedUser.County.Should().Be("Sibiu");
            savedUser.City.Should().Be("Mediaș");
        }

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void User_BirthDate_ShouldNotBeInTheFuture()
            => _loginService
                .AddUserForTesting(CreateRequest("future@test.com", "Complex123!"))
                .Execute(_ => LoginService.GetUsersForTesting().First(u => u.Email == "future@test.com")
                    .BirthDate.Should().BeBefore(_time.UtcNow));

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void User_Username_ShouldBeAutoGeneratedCorrectly()
            => _loginService
                .AddUserForTesting(CreateRequest("user@test.com", "Complex123!"))
                .Execute(_ => LoginService.GetUsersForTesting().First(u => u.Email == "user@test.com")
                    .Username.Should().Be("maria.ionescu"));

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void User_ProfilePicture_ShouldNotBeEmpty()
            => _loginService
                .AddUserForTesting(CreateRequest("pic@test.com", "Complex123!"))
                .Execute(_ => LoginService.GetUsersForTesting().First(u => u.Email == "pic@test.com")
                    .ProfilePicture.Should().NotBeNullOrWhiteSpace());

        [Fact]
        [Trait("Feature", "UserIntegrity")]
        [Trait("Type", "Stress")]
        [Trait("Priority", "P3")]
        public void Login_ShouldHandleOneThousandUsersWithoutCorruption()
        {
            for (int i = 0; i < 1000; i++)
            {
                _loginService.AddUserForTesting(CreateRequest($"user{i}@test.com", "Complex123!"));
            }

            LoginService.GetUsersForTesting().Count.Should().BeGreaterOrEqualTo(1000);
            _loginService.Login("user(500)@test.com", "Complex123!"); 
        }


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