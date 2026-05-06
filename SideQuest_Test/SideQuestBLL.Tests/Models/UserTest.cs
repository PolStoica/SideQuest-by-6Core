using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions; 
using SideQuest.BLL.Enums;
using SideQuest.BLL.Models;

namespace SideQuest_Test.SideQuestBLL.Tests.Models
{
    public class UserTest
    {
        [Fact]
        [Trait("Feature", "UserProfile")]
        [Trait("Type", "UnitTest")]
        [Trait("Priority", "P1")]
        public void SaveProfile_GivenValidZoneManastur_ShouldAssignZoneCorrectly()
        {
            var user = new User();
            var expectedZone = Zone.Manastur;

            user.UserZone = expectedZone;

            user.UserZone.Should().Be(expectedZone, "because the user profile must reflect the selected Cluj district");
        }


        [Fact]
        [Trait("Feature", "UserProfile")]
        [Trait("Type", "UnitTest")]
        [Trait("Priority", "P1")]
        public void UpdateProfile_ChangingZoneFromZorilorToGheorgheni_ShouldUpdateSuccessfully()
        {
            var user = new User { Name = "Andrei", UserZone = Zone.Zorilor };
            var newZone = Zone.Gheorgheni;

            user.UserZone = newZone;

            user.UserZone.Should().Be(newZone, "because a user should be able to update their residence district if they move");
        }
    }

}
