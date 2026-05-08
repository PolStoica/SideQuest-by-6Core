using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Services
{
    public static class RegisterTestsData
    {
        public static IEnumerable<object[]> InvalidRegisterData()
        {
            var invalidInputs = new[]
            {
            null,
            "",
            " "
        };

            foreach (var email in invalidInputs)
                foreach (var password in invalidInputs)
                    foreach (var username in invalidInputs)
                        foreach (var birthdate in invalidInputs)
                            foreach (var confirmPassword in invalidInputs)
                                foreach (var phoneNumber in invalidInputs)
                                    foreach (var profilePicture in invalidInputs)
                                        foreach (var selectedCategories in invalidInputs)
                                        {
                                            yield return new object[] { email, password, username, birthdate, confirmPassword, phoneNumber, profilePicture, selectedCategories };
                                    }
        }



        public static IEnumerable<object[]> InvalidFullNames()
        {
            var invalidNames = new[]
            {
            "John @ Doe",
            "Test # User",
            "Ana $ Maria",
            "Ion & Pop",
            "!!! Test !!!",
            "@@@ User @@@",
            "Ion ( Pop",
            "Ion ) Pop",
            "Ion - Pop",
            "Ion * Pop",
            "Ion} Pop",
            "Ion {Pop",
            "Ion[ Pop",
            "Ion] Pop",
            "Ion= Pop",
            "Ion+ Pop",
            "Ion, Pop",
            "Ion. Pop",
            "Ion? Pop",
            "Ion/ Pop",
            "Ion\\ Pop",
            "Ion| Pop",
            "Ion` Pop",
            "Ion~ Pop",
            "Ion> Pop",
            "Ion< Pop",
            "Ion: Pop",
            "Ion; Pop",
            "Ion\" Pop"

        };

            foreach (var fullName in invalidNames)
                yield return new object[]
                {
                "test@test.com",
                "Abc123456!",
                fullName
                };
        }

        public static IEnumerable<object[]> InvalidFullNameOnlySpecialCharacters()
        {
            var invalidNames = new[]
            {
            "'",
            "_",
            "'''''",
            "_____"
        };

            foreach (var fullName in invalidNames)
                yield return new object[]
                {
                "test@test.com",
                "Abc123456!",
                fullName
                };
        }


    }
    
}
