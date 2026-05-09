using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Services
{
    public static class RegisterTestsData
    {
        public static IEnumerable<object[]> InvalidFieldsData()
        {
            var invalidStrings = new[] { null, "", " " };

            foreach (var value in invalidStrings)
            {
                yield return new object[] { "LastName", value };
                yield return new object[] { "FirstName", value };
                yield return new object[] { "County", value };
                yield return new object[] { "City", value };
                yield return new object[] { "Email", value };
                yield return new object[] { "Password", value };
                yield return new object[] { "ConfirmPassword", value };
                yield return new object[] { "ProfilePicture", value };
            }

            yield return new object[] { "SelectedCategories", null }; 
            yield return new object[] { "BirthDate", default(DateTime) }; 
        }




        public static IEnumerable<object[]> InvalidNames()
        {
            var invalidNames = new[]
            {
            "John@",
            "Test#",
            "Ana$",
            "Ion&",
            "!!!Test",
            "@@@User",
            "Ion(",
            "Ion)",
            "Ion*",
            "Ion}",
            "{Pop",
            "Ion[",
            "Ion]",
            "Ion=",
            "Ion+",
            "Ion,",
            "Ion?",
            "Ion/",
            "Ion\\",
            "Ion|",
            "Ion`",
            "Ion~",
            "Ion>",
            "Ion<",
            "Ion:",
            "Ion;",
            "Ion\"",
            "Ion_",
            "___",
            "'",
            "'''''",
            "...."

            };

            foreach (var name in invalidNames)
                yield return new object[]{ name };

        }


        public static IEnumerable<object[]> InvalidSpaceBeforeOrAfterName()
        {
            var invalidNames = new[]
            {
            " Andrei",
            "Andrei ",
            "     Andrei",
            " Andrei "
            };

            foreach (var name in invalidNames)
                yield return new object[] { name };

        }


    }
    
}
