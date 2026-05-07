using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Services
{
    public static class LoginTestsData
    {
        public static IEnumerable<object[]> InvalidLoginData()
        {
            var invalidInputs = new[]
            {
                null,
                "",
                " "
            };

            foreach (var email in invalidInputs)
                foreach (var password in invalidInputs)

                {
                    yield return new object[] { email, password };
                }
        }

        public static IEnumerable<object[]> EmailAndPasswordContainsLeadingOrTrailingSpaces()
        {
            var emailInvalidInputs = new[]
            {
                " test@test.com",
                "test@test.com ",
                " test@test.com "
            };

            var passwordInvalidInputs = new[]
            {
                " Abc123456!",
                "Abc123456! ",
                " Abc123456! "
            };

            foreach (var email in emailInvalidInputs)
                foreach (var password in passwordInvalidInputs)

                {
                    yield return new object[] { email, password };
                }
        }


    }
}
