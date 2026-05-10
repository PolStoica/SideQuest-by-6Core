using System.Collections.Generic;

namespace SideQuest.BLL.Services
{
    public static class LoginTestsData
    {
        public static IEnumerable<object[]> InvalidLoginData()
        {
            yield return new object[] { null, "Abc123456!" };
            yield return new object[] { "test@test.com", null };
            yield return new object[] { "", "" };
            yield return new object[] { " ", " " };
        }

        public static IEnumerable<object[]> EmailAndPasswordContainsLeadingOrTrailingSpaces()
        {
            yield return new object[] { " test@test.com", "Abc123456!" };
            yield return new object[] { "test@test.com ", "Abc123456!" };
            yield return new object[] { "test@test.com", " Abc123456!" };
            yield return new object[] { "test@test.com", "Abc123456! " };
        }
    }
}