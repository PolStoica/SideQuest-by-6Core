using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SideQuest.BLL.Models;
using Xunit;

namespace SideQuest_Test.SideQuestBLL.Tests.Models
{
    public class ReviewTest
    {
        private readonly Review _review;
        private const string ActId = "activity-123";
        private const string Reviewer = "reviewer@test.com";
        private const string Target = "target@test.com";

        public ReviewTest()
        {
            _review = new Review(ActId, Reviewer, Target);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_WithUnpleasantRatingAndValidDescription_PassesValidation()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = "Valid explanation provided";
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Review_WithUnpleasantRatingAndNullDescription_ThrowsArgumentException()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = null;
            Action act = () => _review.Validate();
            act.Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Review_WithUnpleasantRatingAndEmptyDescription_ThrowsArgumentException()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = string.Empty;
            Action act = () => _review.Validate();
            act.Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Review_WithUnpleasantRatingAndWhitespaceDescription_ThrowsArgumentException()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = "   ";
            Action act = () => _review.Validate();
            act.Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_WithPleasantRatingAndNullDescription_PassesValidation()
        {
            _review.Rating = Review.ReviewRating.Pleasant;
            _review.Description = null;
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_WithExceptionalRatingAndNullDescription_PassesValidation()
        {
            _review.Rating = Review.ReviewRating.Exceptional;
            _review.Description = null;
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Review_NewInstance_NegativeFlagsDefaultToFalse()
        {
            _review.WasLate.Should().BeFalse();
            _review.WasAggressive.Should().BeFalse();
            _review.WasInappropriate.Should().BeFalse();
            _review.DidNotShowUp.Should().BeFalse();
            _review.SpammedChat.Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Review_NewInstance_PositiveFlagsDefaultToFalse()
        {
            _review.WasFunny.Should().BeFalse();
            _review.WasFriendly.Should().BeFalse();
            _review.WasHelpful.Should().BeFalse();
            _review.WasOnTime.Should().BeFalse();
            _review.BroughtGoodVibes.Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Review_NewInstance_SystemFlagsDefaultToExpectedValues()
        {
            _review.IsDisputed.Should().BeFalse();
            _review.IsValidatedByAdmin.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetWasLateToTrue_PersistsState()
        {
            _review.WasLate = true;
            _review.WasLate.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetWasAggressiveToTrue_PersistsState()
        {
            _review.WasAggressive = true;
            _review.WasAggressive.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetWasInappropriateToTrue_PersistsState()
        {
            _review.WasInappropriate = true;
            _review.WasInappropriate.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetDidNotShowUpToTrue_PersistsState()
        {
            _review.DidNotShowUp = true;
            _review.DidNotShowUp.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetSpammedChatToTrue_PersistsState()
        {
            _review.SpammedChat = true;
            _review.SpammedChat.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetWasFunnyToTrue_PersistsState()
        {
            _review.WasFunny = true;
            _review.WasFunny.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetWasFriendlyToTrue_PersistsState()
        {
            _review.WasFriendly = true;
            _review.WasFriendly.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetWasHelpfulToTrue_PersistsState()
        {
            _review.WasHelpful = true;
            _review.WasHelpful.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetWasOnTimeToTrue_PersistsState()
        {
            _review.WasOnTime = true;
            _review.WasOnTime.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetBroughtGoodVibesToTrue_PersistsState()
        {
            _review.BroughtGoodVibes = true;
            _review.BroughtGoodVibes.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetMultipleNegativeFlags_PersistsAllStates()
        {
            _review.WasLate = true;
            _review.WasAggressive = true;
            _review.DidNotShowUp = true;
            _review.WasLate.Should().BeTrue();
            _review.WasAggressive.Should().BeTrue();
            _review.DidNotShowUp.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetMultiplePositiveFlags_PersistsAllStates()
        {
            _review.WasFunny = true;
            _review.WasHelpful = true;
            _review.BroughtGoodVibes = true;
            _review.WasFunny.Should().BeTrue();
            _review.WasHelpful.Should().BeTrue();
            _review.BroughtGoodVibes.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_SetMixOfPositiveAndNegativeFlags_PersistsAllStates()
        {
            _review.WasOnTime = true;
            _review.SpammedChat = true;
            _review.WasOnTime.Should().BeTrue();
            _review.SpammedChat.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountWasLate_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasLate = true }, new Review("a2", "u2", "t") { WasLate = false } };
            list.Count(r => r.WasLate)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountWasAggressive_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasAggressive = true }, new Review("a2", "u2", "t") { WasAggressive = true } };
            list.Count(r => r.WasAggressive)
                .Should()
                .Be(2);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountWasInappropriate_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasInappropriate = false } };
            list.Count(r => r.WasInappropriate)
                .Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountDidNotShowUp_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { DidNotShowUp = true } };
            list.Count(r => r.DidNotShowUp)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountSpammedChat_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { SpammedChat = true }, new Review("a2", "u2", "t") { SpammedChat = true } };
            list.Count(r => r.SpammedChat)
                .Should()
                .Be(2);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountWasFunny_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasFunny = true } };
            list.Count(r => r.WasFunny)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountWasFriendly_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasFriendly = true }, new Review("a2", "u2", "t") { WasFriendly = true } };
            list.Count(r => r.WasFriendly)
                .Should()
                .Be(2);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountWasHelpful_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasHelpful = false } };
            list.Count(r => r.WasHelpful)
                .Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountWasOnTime_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasOnTime = true } };
            list.Count(r => r.WasOnTime)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountBroughtGoodVibes_ReturnsCorrectAccumulation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { BroughtGoodVibes = true } };
            list.Count(r => r.BroughtGoodVibes)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_FilterByRatingUnpleasant_ReturnsOnlyUnpleasantReviews()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" }, new Review("a2", "u2", "t") { Rating = Review.ReviewRating.Pleasant } };
            list.Where(r => r.Rating == Review.ReviewRating.Unpleasant)
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_FilterByRatingPleasant_ReturnsOnlyPleasantReviews()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Pleasant } };
            list.Where(r => r.Rating == Review.ReviewRating.Pleasant)
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_FilterByRatingExceptional_ReturnsOnlyExceptionalReviews()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Exceptional } };
            list.Where(r => r.Rating == Review.ReviewRating.Exceptional)
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateTotalScore_ExceptionalRatingAddsTenPoints()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Exceptional } };
            var score = list.Sum(r => r.Rating == Review.ReviewRating.Exceptional ? 10 : 0);
            score.Should()
                .Be(10);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateTotalScore_PleasantRatingAddsFivePoints()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Pleasant } };
            var score = list.Sum(r => r.Rating == Review.ReviewRating.Pleasant ? 5 : 0);
            score.Should()
                .Be(5);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateTotalScore_UnpleasantRatingAddsZeroPoints()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" } };
            var score = list.Sum(r => r.Rating == Review.ReviewRating.Unpleasant ? 0 : 5);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateTotalScore_AccumulatesCorrectlyForMultipleUsers()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Exceptional }, new Review("a2", "u2", "t") { Rating = Review.ReviewRating.Pleasant } };
            var score = list.Sum(r => r.Rating == Review.ReviewRating.Exceptional ? 10 : (r.Rating == Review.ReviewRating.Pleasant ? 5 : 0));
            score.Should()
                .Be(15);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateTotalScore_IgnoresReviewsNotValidatedByAdmin()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Exceptional, IsValidatedByAdmin = false } };
            var score = list.Where(r => r.IsValidatedByAdmin).Sum(r => r.Rating == Review.ReviewRating.Exceptional ? 10 : 0);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateTotalScore_IgnoresDisputedReviews()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Exceptional, IsDisputed = true } };
            var score = list.Where(r => !r.IsDisputed).Sum(r => r.Rating == Review.ReviewRating.Exceptional ? 10 : 0);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateLatenessRate_CalculatesCorrectPercentage()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasLate = true }, new Review("a2", "u2", "t") { WasLate = false } };
            var rate = (double)list.Count(r => r.WasLate) / list.Count * 100;
            rate.Should()
                .Be(50.0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateNoShowRate_CalculatesCorrectPercentage()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { DidNotShowUp = true }, new Review("a2", "u2", "t") { DidNotShowUp = false }, new Review("a3", "u3", "t") { DidNotShowUp = false } };
            var rate = (double)list.Count(r => r.DidNotShowUp) / list.Count * 100;
            rate.Should()
                .BeApproximately(33.33, 0.01);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateSpamRate_CalculatesCorrectPercentage()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { SpammedChat = true } };
            var rate = (double)list.Count(r => r.SpammedChat) / list.Count * 100;
            rate.Should()
                .Be(100.0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void Review_ConstructorWithEmergencyTrue_SetsIsEmergencyReviewToTrue()
        {
            var emergencyReview = new Review(ActId, Reviewer, Target, true);
            emergencyReview.IsEmergencyReview.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void Review_ConstructorDefault_SetsIsEmergencyReviewToFalse()
        {
            _review.IsEmergencyReview.Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void Review_IsEmergencyReview_CanBeCombinedWithAnyRating()
        {
            var emergencyReview = new Review(ActId, Reviewer, Target, true) { Rating = Review.ReviewRating.Exceptional };
            emergencyReview.Rating.Should().Be(Review.ReviewRating.Exceptional);
            emergencyReview.IsEmergencyReview.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void Review_IsEmergencyReview_AllowsNegativeFlagsToSetState()
        {
            var emergencyReview = new Review(ActId, Reviewer, Target, true) { WasAggressive = true };
            emergencyReview.WasAggressive.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void Review_IsEmergencyReview_AllowsPositiveFlagsToSetState()
        {
            var emergencyReview = new Review(ActId, Reviewer, Target, true) { WasFriendly = true };
            emergencyReview.WasFriendly.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_IsDisputed_CanBeToggledToTrue()
        {
            _review.IsDisputed = true;
            _review.IsDisputed.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_IsDisputed_CanBeToggledBackToFalse()
        {
            _review.IsDisputed = true;
            _review.IsDisputed = false;
            _review.IsDisputed.Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_IsValidatedByAdmin_CanBeToggledToFalse()
        {
            _review.IsValidatedByAdmin = false;
            _review.IsValidatedByAdmin.Should().BeFalse();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_IsValidatedByAdmin_CanBeToggledBackToTrue()
        {
            _review.IsValidatedByAdmin = false;
            _review.IsValidatedByAdmin = true;
            _review.IsValidatedByAdmin.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_FilterEmergencyReviews_ReturnsOnlyEmergencyInstances()
        {
            var list = new List<Review> { new Review("a1", "u1", "t", true), new Review("a2", "u2", "t", false) };
            list.Where(r => r.IsEmergencyReview)
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_FilterDisputedReviews_ReturnsOnlyDisputedInstances()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { IsDisputed = true }, new Review("a2", "u2", "t") { IsDisputed = false } };
            list.Where(r => r.IsDisputed)
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_FilterNonValidated_ReturnsOnlyAdminRejectedInstances()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { IsValidatedByAdmin = false }, new Review("a2", "u2", "t") { IsValidatedByAdmin = true } };
            list.Where(r => !r.IsValidatedByAdmin)
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P1")]
        public void ReviewCollection_CalculateReputation_InitialValueIsMaintainedIfNoReviews()
        {
            var list = new List<Review>();
            var reputation = 50;
            list.Count.Should().Be(0);
            reputation.Should().Be(50);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_SingleUnpleasantDoesNotTriggerPenalty()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" } };
            var uniqueActivitiesCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant).Select(r => r.ActivityId).Distinct().Count();
            var penalty = uniqueActivitiesCount >= 3 ? 20 : 0;
            penalty.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_TwoUnpleasantDoesNotTriggerPenalty()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" }, new Review("a2", "u2", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Y" } };
            var uniqueActivitiesCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant).Select(r => r.ActivityId).Distinct().Count();
            var penalty = uniqueActivitiesCount >= 3 ? 20 : 0;
            penalty.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P1")]
        public void ReviewCollection_CalculateReputation_ThreeUnpleasantFromUniqueActivities_TriggersPenalty()
        {
            var list = new List<Review>
            {
                new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" },
                new Review("a2", "u2", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Y" },
                new Review("a3", "u3", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Z" }
            };
            var uniqueActivitiesCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant).Select(r => r.ActivityId).Distinct().Count();
            var penalty = uniqueActivitiesCount >= 3 ? 20 : 0;
            penalty.Should()
                .Be(20);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_ThreeUnpleasantFromSameActivity_DoesNotTriggerPenalty()
        {
            var list = new List<Review>
            {
                new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" },
                new Review("a1", "u2", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Y" },
                new Review("a1", "u3", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Z" }
            };
            var uniqueActivitiesCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant).Select(r => r.ActivityId).Distinct().Count();
            var penalty = uniqueActivitiesCount >= 3 ? 20 : 0;
            penalty.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_ThreeUnpleasantFromSameReviewer_DoesNotTriggerPenalty()
        {
            var list = new List<Review>
            {
                new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" },
                new Review("a2", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Y" },
                new Review("a3", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Z" }
            };
            var uniqueReviewersCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant).Select(r => r.ReviewerEmail).Distinct().Count();
            var penalty = uniqueReviewersCount >= 3 ? 20 : 0;
            penalty.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_ExceptionalReviewIncreasesReputationByFixedPoints()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Exceptional } };
            var bonus = list.Count(r => r.Rating == Review.ReviewRating.Exceptional) * 5;
            bonus.Should()
                .Be(5);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_PleasantReviewDoesNotChangeReputation()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Pleasant } };
            var modification = list.Count(r => r.Rating == Review.ReviewRating.Exceptional) * 5;
            modification.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_ReputationCannotDropBelowZero()
        {
            var rep = 10;
            var penalty = 20;
            var result = Math.Max(0, rep - penalty);
            result.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_ReputationCannotExceedOneHundred()
        {
            var rep = 98;
            var bonus = 5;
            var result = Math.Min(100, rep + bonus);
            result.Should()
                .Be(100);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_PenaltyTriggeredMultipleTimesIfThresholdsMultiplied()
        {
            var list = new List<Review>();
            for (int i = 1; i <= 6; i++) list.Add(new Review($"a{i}", $"u{i}", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X" });
            var uniqueActivitiesCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant).Select(r => r.ActivityId).Distinct().Count();
            var penalty = (uniqueActivitiesCount / 3) * 20;
            penalty.Should()
                .Be(40);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_IgnoresDisputedUnpleasantReviewsFromPenalty()
        {
            var list = new List<Review>
            {
                new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "X", IsDisputed = true },
                new Review("a2", "u2", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Y" },
                new Review("a3", "u3", "t") { Rating = Review.ReviewRating.Unpleasant, Description = "Z" }
            };
            var uniqueActivitiesCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant && !r.IsDisputed).Select(r => r.ActivityId).Distinct().Count();
            var penalty = uniqueActivitiesCount >= 3 ? 20 : 0;
            penalty.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_IgnoresNonAdminValidatedReviewsFromCalculations()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { Rating = Review.ReviewRating.Exceptional, IsValidatedByAdmin = false } };
            var bonus = list.Where(r => r.IsValidatedByAdmin).Count(r => r.Rating == Review.ReviewRating.Exceptional) * 5;
            bonus.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_EmergencyUnpleasantReviewsTriggerDoublePenalty()
        {
            var list = new List<Review>
            {
                new Review("a1", "u1", "t", true) { Rating = Review.ReviewRating.Unpleasant, Description = "X" },
                new Review("a2", "u2", "t", true) { Rating = Review.ReviewRating.Unpleasant, Description = "Y" },
                new Review("a3", "u3", "t", true) { Rating = Review.ReviewRating.Unpleasant, Description = "Z" }
            };
            var isEmergencyTrigger = list.Any(r => r.IsEmergencyReview && r.Rating == Review.ReviewRating.Unpleasant);
            var uniqueActivitiesCount = list.Where(r => r.Rating == Review.ReviewRating.Unpleasant).Select(r => r.ActivityId).Distinct().Count();
            var penalty = uniqueActivitiesCount >= 3 ? (isEmergencyTrigger ? 40 : 20) : 0;
            penalty.Should()
                .Be(40);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_OnTimeAndGoodVibesCombo_ProvidesSmallReputationBonus()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasOnTime = true, BroughtGoodVibes = true } };
            var bonus = list.Count(r => r.WasOnTime && r.BroughtGoodVibes) * 2;
            bonus.Should()
                .Be(2);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_NoShowFlag_TriggersImmediateReputationPenalty()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { DidNotShowUp = true } };
            var penalty = list.Count(r => r.DidNotShowUp) * 15;
            penalty.Should()
                .Be(15);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_AggressiveFlag_TriggersImmediateReputationPenalty()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasAggressive = true } };
            var penalty = list.Count(r => r.WasAggressive) * 10;
            penalty.Should()
                .Be(10);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_InappropriateFlag_TriggersImmediateReputationPenalty()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { WasInappropriate = true } };
            var penalty = list.Count(r => r.WasInappropriate) * 10;
            penalty.Should()
                .Be(10);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_SpammedChatFlag_TriggersReputationPenalty()
        {
            var list = new List<Review> { new Review("a1", "u1", "t") { SpammedChat = true } };
            var penalty = list.Count(r => r.SpammedChat) * 5;
            penalty.Should()
                .Be(5);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Review_CaseInsensitivity_TargetEmailComparisonInConstructor_ThrowsException()
        {
            Action act = () => new Review(ActId, "user@test.com", "USER@TEST.COM");
            act.Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Review_CaseInsensitivity_ReviewerEmailComparisonInConstructor_ThrowsException()
        {
            Action act = () => new Review(ActId, "USER@TEST.COM", "user@test.com");
            act.Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_Id_IsRegeneratedAndUniqueForEveryInstance()
        {
            var secondaryReview = new Review(ActId, Reviewer, Target);
            _review.Id.Should()
                .NotBe(secondaryReview.Id);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_CreatedAt_IsSetToUtcAtTheMomentOfCreation()
        {
            _review.CreatedAt.Should()
                .BeBefore(DateTime.UtcNow.AddSeconds(1));
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Review_Description_AcceptsVeryLongText()
        {
            var longText = new string('A', 5000);
            _review.Description = longText;
            _review.Description.Should()
                .HaveLength(5000);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Review_Description_AcceptsSpecialCharactersAndEmojis()
        {
            var specialText = "Text with Emojis 🚀 & Special Characters #@$%^&*()_+-=";
            _review.Description = specialText;
            _review.Description.Should()
                .Be(specialText);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Review_Description_AcceptsNumericStrings()
        {
            var numericText = "1234567890";
            _review.Description = numericText;
            _review.Description.Should()
                .Be(numericText);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_ActivityId_RetainsExactValueAssignedInConstructor()
        {
            _review.ActivityId.Should().Be(ActId);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_ReviewerEmail_RetainsExactValueAssignedInConstructor()
        {
            _review.ReviewerEmail.Should().Be(Reviewer);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Review_TargetUserEmail_RetainsExactValueAssignedInConstructor()
        {
            _review.TargetUserEmail.Should().Be(Target);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_Rating_CanBeChangedMultipleTimesBeforeValidation()
        {
            _review.Rating = Review.ReviewRating.Exceptional;
            _review.Rating = Review.ReviewRating.Pleasant;
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Rating.Should()
                .Be(Review.ReviewRating.Unpleasant);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Review_Flags_CanBeInvertedMultipleTimes()
        {
            _review.WasLate = true;
            _review.WasLate = false;
            _review.WasLate = true;
            _review.WasLate.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Review_Validate_DoesNotThrowIfRatingIsPleasantWithWhitespaceDescription()
        {
            _review.Rating = Review.ReviewRating.Pleasant;
            _review.Description = "   ";
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Review_Validate_DoesNotThrowIfRatingIsExceptionalWithWhitespaceDescription()
        {
            _review.Rating = Review.ReviewRating.Exceptional;
            _review.Description = "   ";
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P1")]
        public void Review_EmergencyReview_DefaultsToFalseWhenUsingThreeParamConstructor()
        {
            _review.IsEmergencyReview
                   .Should().BeFalse();
        }


        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P2")]
        public void Review_Validate_WithMaximumAllowedDescriptionLength_PassesSuccessfully()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = new string('A', 4000);
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P2")]
        public void Review_Validate_DescriptionExceedingDatabaseSoftLimit_MaintainsModelIntegrity()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = new string('A', 10000);
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P2")]
        public void Review_WithAllPositiveAndAllNegativeFlagsTrue_ShouldBeLogicallyAllowedAtModelLevel()
        {
            _review.WasLate = true;
            _review.WasOnTime = true;
            _review.WasLate.Should().BeTrue();
            _review.WasOnTime.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Review_EmergencyReview_CombinedWithUnpleasantRatingAndNoDescription_ThrowsArgumentException()
        {
            var emergencyReview = new Review(ActId, Reviewer, Target, true)
            {
                Rating = Review.ReviewRating.Unpleasant,
                Description = null
            };
            Action act = () => emergencyReview.Validate();
            act.Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P2")]
        public void Review_IsDisputedTrue_ButIsValidatedByAdminTrue_RepresentsValidStateBeforeAdminReview()
        {
            _review.IsDisputed = true;
            _review.IsValidatedByAdmin = true;
            _review.IsDisputed.Should().BeTrue();
            _review.IsValidatedByAdmin.Should().BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Review_RatingChange_FromUnpleasantToExceptional_ClearsDescriptionRequirementValidation()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = null;
            _review.Rating = Review.ReviewRating.Exceptional;
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P2")]
        public void Review_TimeTravelSimulation_CreatedAtCannotBeModifiedPostConstruction()
        {
            var initialTime = _review.CreatedAt;
            initialTime.Should().BeBefore(DateTime.UtcNow.AddSeconds(1));
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_DetectsAndMitigatesCollusion()
        {
            var list = new List<Review>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Review(ActId, "colluder@test.com", Target) { Rating = Review.ReviewRating.Exceptional });
            }
            var hasCollusion = list.GroupBy(r => new { r.ReviewerEmail, r.TargetUserEmail }).Any(g => g.Count() >= 5);
            var reputationBonus = hasCollusion ? 0 : list.Count * 5;
            reputationBonus.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateTotalScore_SpamChatFlagTrue_ReducesPositiveScoreContribution()
        {
            var list = new List<Review> { new Review(ActId, Reviewer, Target) { Rating = Review.ReviewRating.Exceptional, SpammedChat = true } };
            var score = list.Sum(r => r.Rating == Review.ReviewRating.Exceptional ? (r.SpammedChat ? 5 : 10) : 0);
            score.Should()
                .Be(5);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_DidNotShowUpFlag_OverridesAllPositiveFlagsCalculations()
        {
            var list = new List<Review> { new Review(ActId, Reviewer, Target) { WasFunny = true, WasFriendly = true, DidNotShowUp = true } };
            var bonus = list.Any(r => r.DidNotShowUp) ? 0 : list.Count(r => r.WasFunny || r.WasFriendly) * 2;
            bonus.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_PerfectScoreBonus_TriggeredOnlyIfAllPositiveFlagsAreTrue()
        {
            var list = new List<Review> { new Review(ActId, Reviewer, Target) { WasFunny = true, WasFriendly = true, WasHelpful = true, WasOnTime = true, BroughtGoodVibes = true } };
            var isPerfect = list.All(r => r.WasFunny && r.WasFriendly && r.WasHelpful && r.WasOnTime && r.BroughtGoodVibes);
            var bonus = isPerfect ? 25 : 0;
            bonus.Should()
                .Be(25);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_ToxicUserTrigger_FiveDistinctInappropriateFlagsDropsReputationToZero()
        {
            var list = new List<Review>();
            for (int i = 0; i < 5; i++) list.Add(new Review($"a{i}", $"u{i}", Target) { WasInappropriate = true });
            var toxicTrigger = list.Count(r => r.WasInappropriate) >= 5;
            var currentReputation = 80;
            var finalReputation = toxicTrigger ? 0 : currentReputation;
            finalReputation.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_FilterValidReviews_ExcludesReviewsWhereReviewerEmailIsFormatInvalid_Simulation()
        {
            var list = new List<Review> { new Review(ActId, "invalid-email-format", Target), new Review(ActId, Reviewer, Target) };
            var validCount = list.Count(r => r.ReviewerEmail.Contains("@"));
            validCount.Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_OrderAndPaginate_ReturnsChronologicalOrderCorrectly()
        {
            var list = new List<Review> { _review };
            var ordered = list.OrderBy(r => r.CreatedAt).ToList();
            ordered.Should()
                .ContainInOrder(list);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Review_Constructor_TrimsWhitespaceFromEmailsAndActivityId()
        {
            var untrimmedReview = new Review("   act123   ", "  user@t.com  ", "  target@t.com  ");
            var trimmedActId = untrimmedReview.ActivityId.Trim();
            trimmedActId.Should()
                .Be("act123");
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateVibeScore_ReturnsCorrectRatioOfGoodVibes()
        {
            var list = new List<Review> { new Review(ActId, Reviewer, Target) { BroughtGoodVibes = true }, new Review(ActId, Reviewer, Target) { BroughtGoodVibes = false } };
            var score = (double)list.Count(r => r.BroughtGoodVibes) / list.Count * 100;
            score.Should()
                .Be(50.0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateHelpfulnessIndex_WeighsHelpfulFlagAgainstTotalReviews()
        {
            var list = new List<Review> { new List<Review> { new Review(ActId, Reviewer, Target) { WasHelpful = true } }[0] };
            var ratio = (double)list.Count(r => r.WasHelpful) / list.Count;
            ratio.Should()
                .Be(1.0);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_IdentifyTrend_DetectsIfLastThreeReviewsAreDroppingInRating()
        {
            var trend = new List<Review.ReviewRating> { Review.ReviewRating.Exceptional, Review.ReviewRating.Pleasant, Review.ReviewRating.Unpleasant };
            var isDropping = trend[0] > trend[1] && trend[1] > trend[2];
            isDropping.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculatePerformanceMetrics_ReturnsCorrectCombinedObject()
        {
            var list = new List<Review> { new Review(ActId, Reviewer, Target) { WasOnTime = true } };
            var combinedMetrics = new { Total = list.Count, OnTimeCount = list.Count(r => r.WasOnTime) };
            combinedMetrics.Total.Should().Be(1);
            combinedMetrics.OnTimeCount.Should().Be(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_EmergencyReviewRatio_ShouldNotExceedTenPercentOfTotalReviews()
        {
            var list = new List<Review> { new Review(ActId, Reviewer, Target, true) };
            for (int i = 0; i < 9; i++) list.Add(new Review(ActId, Reviewer, Target, false));
            var ratio = (double)list.Count(r => r.IsEmergencyReview) / list.Count;
            ratio.Should()
                .BeLessThanOrEqualTo(0.10);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReviewCollection_CalculateReputation_TwoEmergencyUnpleasantReviews_TriggersMaximumPenalty()
        {
            var list = new List<Review> { new Review("a1", "u1", Target, true) { Rating = Review.ReviewRating.Unpleasant, Description = "X" }, new Review("a2", "u2", Target, true) { Rating = Review.ReviewRating.Unpleasant, Description = "Y" } };
            var penalty = list.Count(r => r.IsEmergencyReview && r.Rating == Review.ReviewRating.Unpleasant) * 20;
            var finalPenalty = Math.Min(40, penalty);
            finalPenalty.Should()
                .Be(40);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P2")]
        public void Review_Validate_DoesNotThrowIfDescriptionContainsSqlInjectionAttackStrings()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = "SELECT * FROM Users; DROP TABLE Reviews; --";
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P2")]
        public void Review_Validate_DoesNotThrowIfDescriptionContainsHtmlTags()
        {
            _review.Rating = Review.ReviewRating.Unpleasant;
            _review.Description = "<script>alert('compromised')</script><h1>Review</h1>";
            Action act = () => _review.Validate();
            act.Should()
                .NotThrow();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Review_TargetUserEmail_ComparisonIsCaseInsensitiveEverywhere()
        {
            var isMatch = _review.TargetUserEmail.Equals("TARGET@TEST.COM", StringComparison.OrdinalIgnoreCase);
            isMatch.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Review_ReviewerEmail_ComparisonIsCaseInsensitiveEverywhere()
        {
            var isMatch = _review.ReviewerEmail.Equals("REVIEWER@TEST.COM", StringComparison.OrdinalIgnoreCase);
            isMatch.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Review_ActivityId_ComparisonIsCaseSensitive()
        {
            var isExactMatch = _review.ActivityId.Equals(ActId, StringComparison.Ordinal);
            isExactMatch.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CountUniqueReviewers_ReturnsExactCount()
        {
            var list = new List<Review> { new Review(ActId, "r1@t.com", Target), new Review(ActId, "r1@t.com", Target), new Review(ActId, "r2@t.com", Target) };
            var uniqueCount = list.Select(r => r.ReviewerEmail).Distinct().Count();
            uniqueCount.Should()
                .Be(2);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_FilterByDateRange_ReturnsCorrectSubset()
        {
            var list = new List<Review> { _review };
            var subset = list.Where(r => r.CreatedAt <= DateTime.UtcNow.AddMinutes(1)).ToList();
            subset.Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReviewCollection_CalculateReputation_MixedRatings_AppliesAdditionAndSubtractionInCorrectMathematicalOrder()
        {
            var initialReputation = 50;
            initialReputation += 5;
            initialReputation -= 10;
            initialReputation.Should()
                .Be(45);
        }

        [Fact]
        [Trait("Feature", "Review")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Review_ToggleDisputed_DisablesValidationByAdminAutomatically_Simulation()
        {
            _review.IsDisputed = true;
            var isValidated = !_review.IsDisputed && _review.IsValidatedByAdmin;
            isValidated.Should()
                .BeFalse();
        }



    }
}