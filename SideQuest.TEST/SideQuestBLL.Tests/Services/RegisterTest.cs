using FluentAssertions;
using SideQuest.BLL.Enums;
using SideQuest.BLL.Models;
using SideQuest.BLL.Services;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
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
        [Trait("Feature", "UserRegistration")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P0")]
        public void Register_ShouldSucceed_WhenDataIsValid()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }

                })
                .Should().BeTrue();


        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidFieldsData), MemberType = typeof(RegisterTestsData))]
        [Trait("Feature", "DataIntegrity")]
        [Trait("Type", "ParametrizedTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenAnyRequiredFieldIsMissing(string propertyName, object invalidValue)
        {
            var request = new RegisterRequest
            {
                LastName = "Pop",
                FirstName = "Alin",
                County = "Cluj",
                City = "Cluj",
                BirthDate = new DateTime(2000, 1, 1),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "test@test.com",
                PhoneNumber = 0722123456,
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                SelectedCategories = new List<string> { "Sport" }
            };

            var property = typeof(RegisterRequest).GetProperty(propertyName);
            property.SetValue(request, invalidValue);

            var result = _registerService.Register(request);

            result.Should().BeFalse(because: $"câmpul {propertyName} nu a fost completat corect");
        }


        // EMAIL


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenEmailHasNoAtSymbol()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.testexample.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailHasNoDomain()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "test@gmail",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailHasNoUsername()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "@gmail.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Theory]
        [InlineData("TEST@GMAIL.COM")]
        [InlineData("Test@gmail.com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "CompatibilityTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucced_WhenEmailHasUppercaseLetters(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = email,
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AccountUniqueness")]
        [Trait("Type", "IntegrationLogic")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenEmailAlreadyExists_EvenIfOtherDataIsDifferent()
        {
            RegisterService.ClearUsers();

            var request1 = new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Primul",
                County = "Bucuresti",
                City = "Sector 1",
                BirthDate = new DateTime(1990, 1, 1),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "duplicate@test.com",
                PhoneNumber = 722111222,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Gaming" }
            };

            var request2 = new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Aldoilea",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(1995, 5, 10),
                ProfilePicture = "base64_string_sau_url",
                Email = "duplicate@test.com",
                PhoneNumber = 733444555,
                Password = "AnotherPassword456!",
                ConfirmPassword = "AnotherPassword456!",
                SelectedCategories = new List<string> { "Sport" }
            };

            _registerService.Register(request1).Should().BeTrue();

            _registerService.Register(request2).Should().BeFalse();
        }


        [Fact]
        [Trait("Feature", "AccountUniqueness")]
        [Trait("Type", "DataIntegrity")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenEmailAlreadyExistsWithDifferentCasing()
        {
            RegisterService.ClearUsers();

            _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Primul",
                County = "Bucuresti",
                City = "Sector 1",
                BirthDate = new DateTime(1990, 1, 1),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "User.Test@Example.com",
                PhoneNumber = 722111222,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Gaming" }
            }).Should().BeTrue();

            _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Primul",
                County = "Bucuresti",
                City = "Sector 1",
                BirthDate = new DateTime(1990, 1, 1),
                ProfilePicture = "base64_string_sau_url",
                Email = "user.test@example.com",
                PhoneNumber = 722111222,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Gaming" }
            }).Should().BeFalse();
        }


        [Theory]
        [InlineData("andrei.popescu@domeniu.ro")]
        [InlineData("user+extra@gmail.com")]
        [InlineData("prenume-nume@sub.domain.com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenEmailContainsValidSpecialCharacters(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeTrue();


        [Theory]
        [InlineData("alex @test.com")]
        [InlineData("alex@ test.com")]
        [InlineData("alex@test .com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailContainsInternalSpaces(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("test..test@gmail.com")]
        [InlineData("test.@gmail.com")]
        [InlineData("test@gmail..com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenEmailHasInvalidDotStructure(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenEmailIsExceedinglyLong()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = new string('a', 53) + "@example.com", //Can't be bigger than 65 characters
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("alex<script>@example.com")]
        [InlineData("alex'ionescu@example.com")]
        [InlineData("alex\"ionescu@example.com")]
        [InlineData("alex(test)@example.com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailContainsProhibitedSpecialCharacters(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("user@domain.")]
        [InlineData("user@domain.c")] // TLDs usually have minimum 2 characters
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailHasInvalidTLDStructure(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P1")]

        public void Register_ShouldFail_WhenEmailUsesIpAddressInsteadOfDomain()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "user@127.0.0.1",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData(".alex@test.com")]
        [InlineData("alex.@test.com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailPrefixStartsOrEndsWithDot(string email)
                => _registerService
                    .Register(new RegisterRequest
                    {
                        Email = email,
                        LastName = "Popescu",
                        FirstName = "Andrei",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BirthDate = new DateTime(2005, 5, 20),
                        ProfilePicture = "base64_string_sau_url_aici",
                        PhoneNumber = 722123456,
                        Password = "Abc123456_123!",
                        ConfirmPassword = "Abc123456_123!",
                        SelectedCategories = new List<string> { "Muzică" }
                    })
                    .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenEmailContainsNonAsciiCharacters()
                => _registerService
                    .Register(new RegisterRequest
                    {
                        Email = "utilizatorîn@exemplu.ro", // "î" este non-ASCII
                        LastName = "Popescu",
                        FirstName = "Andrei",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BirthDate = new DateTime(2000, 1, 1),
                        ProfilePicture = "base64_string_sau_url_aici",
                        PhoneNumber = 722123456,
                        Password = "Abc123456_123!",
                        ConfirmPassword = "Abc123456_123!",
                        SelectedCategories = new List<string> { "Muzică" }
                    })
                    .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailContainsMultipleAtSymbols()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@@example.com",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenEmailHasMultipleSubdomains()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "alex.ionescu@facultate.universitate.ro",
                    LastName = "Ionescu",
                    FirstName = "Alex",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Gaming" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailUsesDomainLiteral()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "admin@[127.0.0.1]",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailLocalPartIsSingleCharacter()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "a@example.com",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("1+1=2@example.com")]
        [InlineData("user%test@example.com")]
        [InlineData("user*name@example.com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailContainsUnusualSymbols(string email)
                => _registerService
                    .Register(new RegisterRequest
                    {
                        Email = email,
                        LastName = "Popescu",
                        FirstName = "Andrei",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BirthDate = new DateTime(2005, 5, 20),
                        ProfilePicture = "base64_string_sau_url_aici",
                        PhoneNumber = 722123456,
                        Password = "Abc123456_123!",
                        ConfirmPassword = "Abc123456_123!",
                        SelectedCategories = new List<string> { "Muzică" }
                    })
                    .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailIsQuotedString()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "\"nume spatiu\"@example.com",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailDomainIsPurelyNumeric()
                => _registerService
                    .Register(new RegisterRequest
                    {
                        Email = "test@123.456",
                        LastName = "Popescu",
                        FirstName = "Andrei",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BirthDate = new DateTime(2000, 1, 1),
                        ProfilePicture = "base64_string_sau_url_aici",
                        PhoneNumber = 722123456,
                        Password = "Abc123456_123!",
                        ConfirmPassword = "Abc123456_123!",
                        SelectedCategories = new List<string> { "Muzică" }
                    })
                    .Should().BeFalse();


        [Theory]
        [InlineData("-alex@test.com")]
        [InlineData("_alex@test.com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailStartsWithSpecialCharacter(string email)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = email,
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("alex@my-domain.com")] // Valid
        [InlineData("alex@-domain.com")]   // Invalid
        [InlineData("alex@domain-.com")]   // Invalid
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldValidateHyphensInDomainCorrectly(string email)
        {
            var result = _registerService.Register(new RegisterRequest
            {
                Email = email,
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 1, 1),
                ProfilePicture = "base64_string_sau_url_aici",
                PhoneNumber = 722123456,
                Password = "Abc123456_123!",
                ConfirmPassword = "Abc123456_123!",
                SelectedCategories = new List<string> { "Muzică" }
            });

            if (email.Contains("@-") || email.Contains("-.com"))
                result.Should().BeFalse();
            else
                result.Should().BeTrue();
        }


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P1")]

        public void Register_ShouldFail_WhenEmailIsLocalhost()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "admin@localhost",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenEmailHasNumbersInLocalPart()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "user123.test456@example.com",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenEmailHasNumericSubdomain()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@student.123.ro",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeTrue();


        [Theory]
        [InlineData("alex._test@example.com")]
        [InlineData("alex--test@example.com")]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenEmailHasConsecutiveSpecialCharacters(string invalidEmail)
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = invalidEmail,
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "EmailValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenEmailDomainSegmentIsTooLong()
            => _registerService
                .Register(new RegisterRequest
                {
                    Email = "test@" + new string('a', 64) + ".com",
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeFalse();


        // PASSWORD


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenPasswordTooShort()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc125_",
                    ConfirmPassword = "Abc125_",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPasswordHasNoNumbers()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "abc_Aambjhcgskjd_",
                    ConfirmPassword = "abc_Aambjhcgskjd_",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPasswordHasNoLowercaseLetters()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "123_A123455678656_",
                    ConfirmPassword = "123_A123455678656_",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPasswordHasNoUppercaseLetters()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "123abfgchhruvdbch_",
                    ConfirmPassword = "123abfgchhruvdbch_",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPasswordHasNoSpecialCharacters()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "123abfgchhruvdbcGGGGG",
                    ConfirmPassword = "123abfgchhruvdbcGGGGG",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenPasswordContainsOnlySpacesAndSpecialChar()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "space.pass@test.com",
                    PhoneNumber = 722123456,
                    Password = "        !", 
                    ConfirmPassword = "        !",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenPasswordIsTooLong()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "long.pass@test.com",
                    PhoneNumber = 722123456,
                    Password = new string('A', 50) + "1!a", // maximum of 50 characters
                    ConfirmPassword = new string('A', 200) + "1!a",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenPasswordIsEqualToEmail()
        {
            var email = "Andrei123!"; 
            _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = email,
                    PhoneNumber = 722123456,
                    Password = email, 
                    ConfirmPassword = email,
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();
        }


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenPasswordContainsInternalSpaces()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "space.internal@test.com",
                    PhoneNumber = 722123456,
                    Password = "Abc 123 !@#", 
                    ConfirmPassword = "Abc 123 !@#",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPasswordHasNoAlphabeticCharacters()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "no.letters@test.com",
                    PhoneNumber = 722123456,
                    Password = "1234567890!@#", 
                    ConfirmPassword = "1234567890!@#",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenPasswordContainsEmojis()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "emoji.pass@test.com",
                    PhoneNumber = 722123456,
                    Password = "Password123!😊", 
                    ConfirmPassword = "Password123!😊",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();

        // I don't have "GetUsers"

        //[Fact]
        //[Trait("Feature", "DataIntegrity")]
        //[Trait("Type", "FunctionalTest")]
        //[Trait("Priority", "P2")]
        //public void Register_ShouldStorePasswordExactlyAsProvided()
        //{
        //    RegisterService.ClearUsers();
        //    var originalPass = "ExactPass123!";

        //    _registerService.Register(new RegisterRequest
        //    {
        //        LastName = "Popescu",
        //        FirstName = "Andrei",
        //        County = "Cluj",
        //        City = "Cluj-Napoca",
        //        BirthDate = new DateTime(2005, 5, 20),
        //        ProfilePicture = "base64_string_sau_url_aici",
        //        Email = "integrity@test.com",
        //        PhoneNumber = 722123456,
        //        Password = originalPass,
        //        ConfirmPassword = originalPass,
        //        SelectedCategories = new List<string> { "Sport" }
        //    });

        //    var storedUser = RegisterService.GetUsers().First(u => u.Email == "integrity@test.com");
        //    storedUser.Password.Should().Be(originalPass);
        //}


        // PASSWORD AND CONF PASSWORD


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPasswordAndConfirmPasswordDoNotMatch()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "mismatch@test.com",
                    PhoneNumber = 722123456,
                    Password = "StrongPassword123!",
                    ConfirmPassword = "StrongPassword123?X",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PasswordPolicy")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenConfirmPasswordHasDifferentCasing()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "casing.mismatch@test.com",
                    PhoneNumber = 722123456,
                    Password = "Password123!",
                    ConfirmPassword = "PASSWORD123!", 
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        // FIRST AND LAST NAME


        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidNames), MemberType = typeof(RegisterTestsData))]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenLastNameHasSpecialCharacters(string name)
        {
            _registerService
                .Register(new RegisterRequest
                {
                    LastName = name,
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidNames), MemberType = typeof(RegisterTestsData))]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WheFirstNameHasSpecialCharacters(string name)
        {
            _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Andrei",
                    FirstName = name,
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenLastNameIsTooLong()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Wolfeschlegelsteinhausenbergerdorfff",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenFirstNameIsTooLong()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Andrei",
                    FirstName = "Wolfeschlegelsteinhausenbergerdorfff",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenLastNameHasInvalidCapitalizationFormat()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "AndREI",
                    FirstName = "Ion",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenFirstNameHasInvalidCapitalizationFormat()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Ion",
                    FirstName = "AndREI",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenLastNameIsNotInTitleCase()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "andrei",
                    FirstName = "Ion",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenFirstNameIsNotInTitleCase()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Ion",
                    FirstName = "andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidSpaceBeforeOrAfterName), MemberType = typeof(RegisterTestsData))]
        [Trait("Feature", "DataSanitization")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenLastNameHasSpaceBeforeOrAfter(string name)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = name,
                    FirstName = "Ion",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Theory]
        [MemberData(nameof(RegisterTestsData.InvalidSpaceBeforeOrAfterName), MemberType = typeof(RegisterTestsData))]
        [Trait("Feature", "DataSanitization")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenFirstNameHasSpaceBeforeOrAfter(string name)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Ion",
                    FirstName = name,
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenLastNameHasInvalidCapitalization()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "MARCEL",
                    FirstName = "Ion",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FormattingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenFirstNameHasInvalidCapitalization()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Ion",
                    FirstName = "MARCEL",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" },
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("Popescu-Ionescu")]
        [InlineData("Popa-Văduva")]
        [InlineData("St. John")]
        [InlineData("D'Artagnan")]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenLastNameContainsHyphenOrInternalSpace(string name)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = name,
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "duplicate@test.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeTrue();


        [Theory]
        [InlineData("Mărgărit")]
        [InlineData("Lăzărescu")]
        [InlineData("Țuțuianu")]
        [InlineData("Vrânceanu")]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenNameContainsRomanianDiacritics(string lastName)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = lastName,
                    FirstName = "Lăcrămioara",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "diacritice@test.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenLastNameIsSingleCharacter()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "A",
                    FirstName = "Balan",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "test.scurt@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenFirstNameIsSingleCharacter()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Alex",
                    FirstName = "B",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "test.scurt@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("Popescu7")]
        [InlineData("12345")]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenLastNameContainsDigits(string name)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = name,
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "cifre@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("Popescu7")]
        [InlineData("12345")]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenFirstNameContainsDigits(string name)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Andrei",
                    FirstName = name,
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "cifre@example.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData("Müller")]
        [InlineData("François")]
        [InlineData("José")]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenLastNameContainsInternationalCharacters(string lastName)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = lastName,
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "andrei@test.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Theory]
        [InlineData("Müller")]
        [InlineData("François")]
        [InlineData("José")]
        [Trait("Feature", "NameValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenFirstNameContainsInternationalCharacters(string firstName)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Andrei",
                    FirstName = firstName,
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "andrei@test.com",
                    PhoneNumber = 722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        // PHONE NUMBER


        [Fact]
        [Trait("Feature", "UserValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenPhoneNumberHasMoreThan10Digits()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 72212345678, 
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PhoneNumberValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenPhoneNumberHasFewerThan9Digits()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 722123,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();


        [Theory]
        [InlineData(123456789)]
        [InlineData(234345555)]
        [Trait("Feature", "PhoneNumberValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenPhoneNumberDoesNotStartWithValidRomanianPrefix(long phoneNumber)
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = phoneNumber,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PhoneNumberValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenPhoneNumberIsZero()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 0,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PhoneNumberValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPhoneNumberIsNegative()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = -722123456,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AntiFraud")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPhoneNumberConsistsOfSameRepeatedDigit()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 5, 20),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "alex.test@example.com",
                    PhoneNumber = 777777777,
                    Password = "Abc123456_123!",
                    ConfirmPassword = "Abc123456_123!",
                    SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AccountUniqueness")]
        [Trait("Type", "DataIntegrityTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenPhoneNumberAlreadyExists()
        {
            RegisterService.ClearUsers();

            var request1 = new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2005, 5, 20),
                ProfilePicture = "url_1",
                Email = "primul@example.com",
                PhoneNumber = 722123456,
                Password = "Abc123456_123!",
                ConfirmPassword = "Abc123456_123!",
                SelectedCategories = new List<string> { "Muzică" }
            };

            var request2 = new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Bucuresti",
                City = "Sector 1",
                BirthDate = new DateTime(1998, 3, 15),
                ProfilePicture = "url_2",
                Email = "al_doilea@example.com",
                PhoneNumber = 722123456,
                Password = "AnotherPassword123!",
                ConfirmPassword = "AnotherPassword123!",
                SelectedCategories = new List<string> { "Sport" }
            };

            _registerService.Register(request1).Should().BeTrue();

            _registerService.Register(request2).Should().BeFalse();
        }


        [Fact]
        [Trait("Feature", "PhoneNumberValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenPhoneNumberDoesNotStartWithSeven()
    => _registerService
        .Register(new RegisterRequest
        {
            LastName = "Popescu",
            FirstName = "Andrei",
            County = "Cluj",
            City = "Cluj-Napoca",
            BirthDate = new DateTime(2005, 5, 20),
            ProfilePicture = "base64_string_sau_url_aici",
            Email = "alex.test@example.com",
            PhoneNumber = 072212345,
            Password = "Abc123456_123!",
            ConfirmPassword = "Abc123456_123!",
            SelectedCategories = new List<string> { "Muzică", "Sport", "Gaming" }
        })
        .Should().BeFalse();


        [Fact]
        [Trait("Feature", "PhoneNumberValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenPhoneNumberIsLongMaxValue()
    => _registerService
        .Register(new RegisterRequest
        {
            PhoneNumber = long.MaxValue,
            LastName = "Popescu",
            FirstName = "Andrei",
            County = "Cluj",
            City = "Cluj-Napoca",
            BirthDate = new DateTime(2005, 5, 20),
            ProfilePicture = "base64_string_sau_url_aici",
            Email = "alex.test@example.com",
            Password = "Abc123456_123!",
            ConfirmPassword = "Abc123456_123!",
            SelectedCategories = new List<string> { "Muzică" }
        })
        .Should().BeFalse();


        // BIRTHDATE


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenUserIsUnder18YearsOldToday()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-17),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "exactly14@example.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenUserIsExactly18YearsOldToday()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-18),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "exactly18.unique@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenUserIs18YearsMinusOneDay()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-18).AddDays(1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "minor.boundary@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenUserIs18YearsPlusOneDay()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-18).AddDays(-1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "almost14.plus@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenUserIs17YearsAnd11MonthsOld()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-18).AddMonths(1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "almost.adult@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenUserIs18YearsAnd1MonthOld()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-18).AddMonths(-1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "almost.adult@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenUserIsExactly40YearsOldToday()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-40),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "exactly35.happy@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenUserIs40YearsPlusOneDay()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-40).AddDays(-1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "over35.limit@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenUserIs39YearsAnd11Months()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-40).AddMonths(1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "almost35.valid@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenBirthDateIsUnrealistic()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(1876, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "unrealistic.age@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "SecurityTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenBirthDateIsInTheFuture()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddDays(1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "future.born@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "SafetyTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenBirthDateIsDefaultValue()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = default(DateTime),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "default.date.safety@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "SafetyTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenBirthDateIsMinValue()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.MinValue,
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "min.datetime.safety@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "SafetyTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenBirthDateIsSystemMaxValue()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(9999, 12, 31),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "system.max@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenUserWasBornOnLeapDayInLeapYear()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2008, 2, 29),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "leapday.valid@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenLeapDayUserIsExactly18YearsOld()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2008, 2, 29),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "leapday.18years@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenLeapDayCalculatedIncorrectlyForMinor()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2012, 2, 29).AddDays(1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "leapday.minor.fail@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenUserIsBornOnLastDayOfFebruaryInNonLeapYear()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2005, 2, 28),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "feb28.valid@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "SafetyTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenBirthDateIsLeapDayInFutureLeapYear()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2028, 2, 29),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "future.leapday@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "IntegrityTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenBirthDateIsInvalidLeapDay()
        {
            Action act = () => new DateTime(2007, 2, 29);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "NegativeTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenBirthDateIsFebruary30()
        {
            Action act = () => new DateTime(2026, 2, 30);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenUserBornOnLeapDayIsExactly18OnMarch1st()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2008, 2, 29),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "leapday.majorat.ok@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenUserBornOnLeapDayIs18MinusOneDayOnFebruary28th()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2008, 2, 29),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "leapday.almost18@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenUserIsExactly18AndBornOnLastDayOfFebruary()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2008, 2, 28),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "feb28.18years@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenUserIs17YearsAnd364DaysOld()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.Now.AddYears(-18).AddDays(1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "one.day.to.adult@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenUserIsOver18AndBornOnLeapDay()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2004, 2, 29),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "leapday.adult.ok@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "FunctionalTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenUserWasBornOnNewYearsDay()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(DateTime.Now.Year - 20, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "newyear.day@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenUserIsExactly18OnNewYearsDay()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(DateTime.Now.Year - 18, 1, 1),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "newyear.18years@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Tehnologie" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LocalizationTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenBirthDateIsAtMidnightInDifferentTimeZone()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.FromHours(2)).DateTime,
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "timezone.midnight@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LocalizationTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenTimeZoneShiftMakesUserUnderage()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.SpecifyKind(DateTime.Now.AddYears(-14).Date, DateTimeKind.Utc),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "timezone.boundary@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "ParsingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenYearIsProvidedWithTwoDigits()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.ParseExact("01/01/05", "dd/MM/yy", null),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "two.digit.year@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "ParsingTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenTwoDigitYearIsInterpretedInWrongCentury()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = DateTime.ParseExact("01/01/50", "dd/MM/yy", null),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "century.check@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeFalse();


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "ParsingTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenYearIsZero()
        {
            Action act = () => DateTime.ParseExact("01/01/00", "dd/MM/yy", null);
            act.Should().Throw<FormatException>();
        }


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "ParsingTest")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenDayAndMonthAreSwapped()
        {
            Action act = () => new DateTime(2005, 31, 01);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "IntegrityTest")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenMonthIsInvalid()
        {
            Action act = () => new DateTime(2005, 13, 01);

            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("*Month must be between 1 and 12*");
        }


        [Fact]
        [Trait("Feature", "AgeValidation")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenUserIsBornInYear2000()
            => _registerService
                .Register(new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 01, 01),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "millennium.baby@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                })
                .Should().BeTrue();


        // SELECTED CATEGORY


        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenExactly1CategoryIsSelected()
            => _registerService
                .Register( new RegisterRequest
                {
                    LastName = "Popescu",
                    FirstName = "Andrei",
                    County = "Cluj",
                    City = "Cluj-Napoca",
                    BirthDate = new DateTime(2000, 01, 01),
                    ProfilePicture = "base64_string_sau_url_aici",
                    Email = "millennium.baby@test.com",
                    PhoneNumber = 722123456,
                    Password = "SafePassword123!",
                    ConfirmPassword = "SafePassword123!",
                    SelectedCategories = new List<string> { "Sport" }
                }).Should().BeTrue();


        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_When2CategoriesAreSelected()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test2@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Gaming", "Natură" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenAllCategoriesAreSelected()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test3@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Muzică", "Sport", "Artă", "Food", "Gaming", "Cultură", "Natură", "Dans", "Teatru" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Boundary")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenNoCategoryIsSelected()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test4@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Boundary")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenCategoryListIsNull()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test5@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = null
            }).Should().BeFalse();


        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Integrity")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenCategoryNameIsInvalid()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test6@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "HobbyInexistent" }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenCategoryIsEmptyString()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test7@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "" }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenCategoryIsOnlyWhitespace()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test8@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "   " }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Priority", "P1")]
        public void Register_ShouldBeCaseInsensitive_ForValidCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test9@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "gAmInG" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenDuplicateCategoriesAreUsedToCheatCount()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test10@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport", "Sport" }
            }).Should().BeFalse();


        [Fact]
        public void Register_ShouldSucceed_WithArtCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test11@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Artă" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WithArtăCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test11@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Artă" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenListContainsNullElement()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test13@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { null }
            }).Should().BeFalse();


        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCategoryNameIsTooLong()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test14@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { new string('A', 100) }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WithTeatruCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test15@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Teatru" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenScriptInjectionIsAttemptedInCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test16@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "<script>alert(1)</script>" }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WithCulturăCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test17@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Cultură" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenNumericValuesAreSentAsCategories()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test18@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "123", "456" }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Functional")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenMixOfValidAndEmptyCategoriesIsSent()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test19@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport", "" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenSpecialCharactersAreUsedInName()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test21@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport!" }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WithDansCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test22@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Dans" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenAllSelectedCategoriesAreInvalid()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test23@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Hobby1", "Hobby2" }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WithFoodCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test24@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Food" }
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenCategoryListContainsOnlyEmptyStrings()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test25@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { " ", "  " }
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "CategorySelection")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WithGamingCategory()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                ProfilePicture = "base64_string_sau_url_aici",
                Email = "cat.test22@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Gaming" }
            }).Should().BeTrue();


        // PROFILE PICTURE


        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenProfilePictureIsValidJpegPath()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test1@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "C:/Images/avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePictureIsValidPngPath()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test2@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.png"
            }).Should().BeTrue();


        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureHasInvalidExtension()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test6@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "document.pdf"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureIsExeFile()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test7@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "virus.exe"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenProfilePicturePathContainsXss()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test8@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "<script>alert(1)</script>.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Boundary")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenProfilePicturePathIsTooLong()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test9@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = new string('a', 2000) + ".jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePictureIsBase64String()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test10@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureHasNoExtension()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test11@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "my_photo_without_extension"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureIsJustAnExtension()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test12@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = ".jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenProfilePicturePathContainsDirectoryTraversal()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test13@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "../../../etc/passwd"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePictureHasMultipleDots()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test14@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "user.profile.v1.final.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureExtensionIsTooShort()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test15@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "image.j"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePicturePathHasSpaces()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test16@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "my profile picture.png"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureContainsNullByte()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test17@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "image.jpg\0.exe"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePicturePathIsUrl()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test18@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "https://example.com/images/user123.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePicturePathIsJustSlash()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test19@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "/"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureHasInvalidChars()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test20@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "image|*.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePictureIsBmp()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test21@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "legacy.bmp"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenProfilePictureIsTooSmallBase64()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test22@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "data:image/png;base64,"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldBeCaseInsensitive_ForProfilePictureExtension()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test23@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "IMAGE.JPG"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePicturePathHasSpecialSymbols()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test24@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "photo-2026_@_v1.png"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenProfilePictureIsNumericString()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test25@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "123456789"
            }).Should().BeFalse();


        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureIsSvgWithEmbeddedScript()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test26@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "<svg onload=alert(1)>"
            }).Should().BeFalse();


        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePictureIsWebPFormat()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test27@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "modern_image.webp"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureHasDoubleExtension()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test28@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "image.png.txt"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Boundary")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenProfilePicturePathContainsOnlyDots()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test29@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "...."
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenProfilePictureIsGif()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test30@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "animated_avatar.gif"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenProfilePicturePathContainsCommandInjection()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test31@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "image.jpg; rm -rf /"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePictureIsNetworkPath()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test32@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = @"\\Server\Shares\Images\profile.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureIsHiddenFile()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test33@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = ".htaccess"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePictureNameHasNonAsciiCharacters()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test34@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "poză_profil_2026.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureIsSystemReservedName()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test35@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "CON.png"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePicturePathHasTrailingSpaces()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test36@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg   "
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenProfilePictureIsEmailAddress()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test37@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "test@test.com"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePictureIsAvifFormat()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test38@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "profile.avif"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureContainsEncodedDirectoryTraversal()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test39@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "%2e%2e%2f%2e%2e%2fetc%2fpasswd"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePictureHasQueryString()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test40@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "https://cdn.com/img.jpg?size=large&format=png"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureIsJustTheProtocol()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test41@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "https://"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePictureNameHasEmoji()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test42@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "me_😊.png"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePicturePathIsRelativeAndEscapesRoot()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test43@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "~/../../windows/system32/cmd.exe"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePictureIsHeicFormat()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test44@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "iphone_photo.heic"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenProfilePictureIsZipFile()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test45@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "photos.zip"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePictureIsTiff()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test46@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "high_quality.tiff"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenProfilePictureHasLdapInjection()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test47@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "admin*)(|(password=*))"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenProfilePictureIsSdrFormat()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test48@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "image_sdr.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenProfilePictureIsDataUriWithInvalidMime()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test49@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "data:text/plain;base64,SGVsbG8="
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "ProfilePicture")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenProfilePictureIsJfif()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Popescu",
                FirstName = "Andrei",
                County = "Cluj",
                City = "Cluj-Napoca",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "pic.test50@test.com",
                PhoneNumber = 722123456,
                Password = "SafePassword123!",
                ConfirmPassword = "SafePassword123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "photo.jfif"
            }).Should().BeTrue();


        // CITY & COUNTY


        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenCityHasDiacritics()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Brasov",
                City = "Făgăraș",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test2@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();


        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P1")]
        public void Register_ShouldSucceed_WhenCountyHasDiacritics()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Brașov",
                City = "Fagaras",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test2@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenCityHasHyphen()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Suceava",
                City = "Vatra-Dornei",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test6@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenCountyNameIsTooShort()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "A",
                City = "Oraș",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test7@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenCityNameIsTooLong()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Dolj",
                City = new string('A', 200),
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test8@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCountyContainsNumbers()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Cluj123",
                City = "Cluj",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test9@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenCityHasMultipleSpaces()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Satu Mare",
                City = "Baia Sprie",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test10@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCityContainsSpecialCharacters()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Bihor",
                City = "Oradea!",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test11@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenCountyContainsSqlInjection()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Cluj'; DROP TABLE Users;--",
                City = "Cluj",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test12@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldBeCaseInsensitive_ForCountyAndCity()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "cluj",
                City = "CLUJ-NAPOCA",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test13@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCityDoesNotBelongToCounty()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Arad",
                City = "Constanța",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test14@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCountyNameIsOnlyNumbers()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "12345",
                City = "Slatina",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test16@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();


        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCityNameIsOnlyNumbers()
           => _registerService.Register(new RegisterRequest
           {
               LastName = "Ionescu",
               FirstName = "Maria",
               County = "Arad",
               City = "12345",
               BirthDate = new DateTime(1995, 05, 20),
               Email = "loc.test16@test.com",
               PhoneNumber = 744111222,
               Password = "StrongPass123!",
               ConfirmPassword = "StrongPass123!",
               SelectedCategories = new List<string> { "Sport" },
               ProfilePicture = "avatar.jpg"
           }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P1")]
        public void Register_ShouldFail_WhenCityContainsHtmlTags()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Galați",
                City = "<b>Galați</b>",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test18@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenCityNameIsShort()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Mureș",
                City = "Reghin",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test19@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCountyIsInvalidButCityIsCorrect()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Narnia",
                City = "Buzău",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test20@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenCityHasParentheses()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Ilfov",
                City = "Popești-Leordeni (Sud)",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test21@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P3")]
        public void Register_ShouldFail_WhenCityIsJustEmoji()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Vâlcea",
                City = "🏙️",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test22@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P2")]
        public void Register_ShouldSucceed_WhenCountyIsBucharest()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "București",
                City = "Sector 3",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test23@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "Negative")]
        [Trait("Priority", "P2")]
        public void Register_ShouldFail_WhenCountyAndCityAreSwapped()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Ploiești",
                City = "Prahova",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test24@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeFalse();

        [Fact]
        [Trait("Feature", "Location")]
        [Trait("Type", "HappyPath")]
        [Trait("Priority", "P3")]
        public void Register_ShouldSucceed_WhenCityNameIsVeryLongButValid()
            => _registerService.Register(new RegisterRequest
            {
                LastName = "Ionescu",
                FirstName = "Maria",
                County = "Constanța",
                City = "Mangalia Sat (Zona Industrială)",
                BirthDate = new DateTime(1995, 05, 20),
                Email = "loc.test25@test.com",
                PhoneNumber = 744111222,
                Password = "StrongPass123!",
                ConfirmPassword = "StrongPass123!",
                SelectedCategories = new List<string> { "Sport" },
                ProfilePicture = "avatar.jpg"
            }).Should().BeTrue();


    }
}