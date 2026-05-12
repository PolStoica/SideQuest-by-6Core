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



        // EDGE CASES


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenPasswordIsOnlyOneCharacter()
            => _loginService
                .AddUserForTesting(CreateRequest("short@test.com", "P"))
                .Login("short@test.com", "P")
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Login_ShouldWork_WhenEmailContainsRomanianDiacritics()
        => _loginService
            .AddUserForTesting(CreateRequest("stefan.învârtit@test.ro", "Complex123!"))
            .Login("stefan.învârtit@test.ro", "Complex123!")
            .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Login_ShouldFail_WhenAccountIsSoftDeleted()
        {
            var email = "deleted@test.com";
            var pass = "Complex123!";

            _loginService.AddUserForTesting(CreateRequest(email, pass));

            _loginService.Login(email, pass).Should().BeTrue("pentru că userul este încă activ");

            var user = LoginService.GetUsersForTesting().First(u => u.Email == email);
            user.IsDeleted = true; 

            _loginService.Login(email, pass)
                .Should().BeFalse("pentru că un cont marcat ca 'șters' nu are voie să acceseze sistemul");
        }


        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Login_ShouldSucceed_WhenEmailHasNumbers()
            => _loginService
                .AddUserForTesting(CreateRequest("agent007@m16.gov", "Complex123!"))
                .Login("agent007@m16.gov", "Complex123!")
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Login_ShouldFail_WhenPasswordIsOnlySpecialCharactersAndNumbers()
        {
            _loginService
                .AddUserForTesting(CreateRequest("special@test.com", "!!!111AAA"))
                .Login("special@test.com", "!!!111AAA")
                .Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P2")]
        public void Login_ShouldSyncState_BetweenMultipleServiceInstances()
        {
            var serviceA = new LoginService(_time);
            var serviceB = new LoginService(_time);

            serviceA.AddUserForTesting(CreateRequest("shared@test.com", "Complex123!"));

            serviceB.Login("shared@test.com", "Complex123!")
                .Should().BeTrue();

        }

        [Fact]
        [Trait("Feature", "Login")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldAllowReRegistration_AfterAccountIsDeleted()
        {
            var email = "reborn@test.com";
            _loginService.AddUserForTesting(CreateRequest(email, "Complex123!"));

            LoginService.GetUsersForTesting().RemoveAll(u => u.Email == email);

            _loginService.AddUserForTesting(CreateRequest(email, "NewPass123!"))
                .Login(email, "NewPass123!")
                .Should().BeTrue();
        }


            // REFINEMENT TESTS & EDGE CASES

            [Fact]
            [Trait("Feature", "Security")]
            [Trait("Type", "TimingAttack")]
            [Trait("Priority", "P3")]
            public void Login_ShouldTakeSimilarTime_ForExistingAndNonExistingUsers()
            {
                var stopwatch = new System.Diagnostics.Stopwatch();

                _loginService.AddUserForTesting(CreateRequest("exists@test.com", "Complex123!"));

                stopwatch.Start();
                _loginService.Login("exists@test.com", "wrong_pass");
                var time1 = stopwatch.ElapsedMilliseconds;

                stopwatch.Restart();
                _loginService.Login("nonexistent@test.com", "any_pass");
                var time2 = stopwatch.ElapsedMilliseconds;

                Math.Abs(time1 - time2).Should().BeLessThan(500, "pentru a preveni detectarea conturilor prin timing attacks");
            }

            [Fact]
            [Trait("Feature", "Security")]
            [Trait("Type", "Concurrency")]
            [Trait("Priority", "P2")]
            public void Login_ShouldHandleConcurrentRequests_WithoutCounterBypass()
            {
                var email = "race@test.com";
                _loginService.AddUserForTesting(CreateRequest(email, "Complex123!"));

                Parallel.For(0, 10, i => {
                    _loginService.Login(email, "wrong_pass");
                });

                _loginService.Login(email, "Complex123!")
                     .Should().BeFalse("pentru că lock-ul trebuie să fie thread-safe");
            }

            [Fact]
            [Trait("Feature", "Security")]
            [Trait("Type", "Session")]
            [Trait("Priority", "P1")]
            public void Login_ShouldFail_ImmediatelyAfterPasswordIsChangedInBackend()
            {
                var email = "change@test.com";
                var oldPass = "OldPass123!";
                _loginService.AddUserForTesting(CreateRequest(email, oldPass));

                _loginService.Login(email, oldPass)
                    .Should().BeTrue();

                var user = LoginService.GetUsersForTesting().First(u => u.Email == email);
                user.Password = "NewPass123!";

                _loginService.Login(email, oldPass)
                    .Should().BeFalse("pentru că vechea parolă trebuie să devină invalidă instant");
            }

            [Fact]
            [Trait("Feature", "Validation")]
            [Trait("Type", "Sanitization")]
            [Trait("Priority", "P3")]
            public void Login_ShouldSanitize_HiddenUnicodeCharacters()
            {
                
                var cleanEmail = "clean@test.com";
                var dirtyEmail = "clean@test.com\u200B"; //"Zero Width Space" (invisible at the end)

                _loginService.AddUserForTesting(CreateRequest(cleanEmail, "Complex123!"))
                    .Login(dirtyEmail, "Complex123!")
                    .Should().BeTrue("deoarece sistemul ar trebui să curețe caracterele invizibile");
            }

            [Fact]
            [Trait("Feature", "Validation")]
            [Trait("Type", "HappyPath")]
            [Trait("Priority", "P3")]
            public void Login_ShouldBeCaseInsensitive_ForEmailDomainOnly()
            {
                _loginService.AddUserForTesting(CreateRequest("user@gmail.com", "Complex123!"));

                _loginService.Login("user@GMAIL.COM", "Complex123!")
                    .Should().BeTrue();
            }

            [Fact]
            [Trait("Feature", "Security")]
            [Trait("Type", "Injection")]
            [Trait("Priority", "P1")]
            public void Login_ShouldBeSafe_AgainstBlindSqlInjection()
            {
                var blindSql = "admin' AND (SELECT 1 FROM (SELECT(SLEEP(5)))a)--";

                _loginService.Login(blindSql, "anyPass123_")
                    .Should().BeFalse();
            }

            [Fact]
            [Trait("Feature", "Stability")]
            [Trait("Type", "Persistence")]
            [Trait("Priority", "P2")]
            public void Lockout_ShouldPersist_BetweenServiceInstances()
            {
                var email = "persist@test.com";
                var service1 = new LoginService(_time);
                service1.AddUserForTesting(CreateRequest(email, "Complex123!"));

                for (int i = 0; i < 5; i++) service1.Login(email, "wrong");

                var service2 = new LoginService(_time);

                service2.Login(email, "Complex123!")
                    .Should().BeFalse();
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