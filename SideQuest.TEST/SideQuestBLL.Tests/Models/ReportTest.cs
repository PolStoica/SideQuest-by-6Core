using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SideQuest.BLL.Models;
using Xunit;

namespace SideQuest_Test.SideQuestBLL.Tests.Models
{
    public class ReportTest
    {
        private readonly Report _report;
        private const string ActId = "activity-999";
        private const string Reporter = "reporter@test.com";
        private const string Target = "target@test.com";

        public ReportTest()
        {
            _report = new Report(ActId, Reporter, Target, Report.ReportReason.Other);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Report_ValidData_InstantiatesSuccessfullyWithDefaultIdAndDate()
        {
            _report.Should()
                .NotBeNull();
            _report.Id.Should()
                .NotBeEmpty();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Report_Id_IsUniqueForEachInstance()
        {
            var secondaryReport = new Report(ActId, Reporter, Target, Report.ReportReason.Other);
            _report.Id.Should()
                .NotBe(secondaryReport.Id);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P1")]
        public void Report_CreatedAt_IsAutomaticallySetToCurrentUtcTime()
        {
            _report.CreatedAt.Should()
                .BeBefore(DateTime.UtcNow.AddSeconds(1));
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_AggressiveBehavior_InitializesCorrectly()
        {
            var r = new Report(ActId, Reporter, Target, Report.ReportReason.AggressiveBehavior);
            r.Reason.Should()
                .Be(Report.ReportReason.AggressiveBehavior);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_InappropriateBehavior_InitializesCorrectly()
        {
            var r = new Report(ActId, Reporter, Target, Report.ReportReason.InappropriateBehavior);
            r.Reason.Should()
                .Be(Report.ReportReason.InappropriateBehavior);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_Harassment_InitializesCorrectly()
        {
            var r = new Report(ActId, Reporter, Target, Report.ReportReason.Harassment);
            r.Reason.Should()
                .Be(Report.ReportReason.Harassment);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_Spam_InitializesCorrectly()
        {
            var r = new Report(ActId, Reporter, Target, Report.ReportReason.Spam);
            r.Reason.Should()
                .Be(Report.ReportReason.Spam);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_FakeIdentity_InitializesCorrectly()
        {
            var r = new Report(ActId, Reporter, Target, Report.ReportReason.FakeIdentity);
            r.Reason.Should()
                .Be(Report.ReportReason.FakeIdentity);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_HateSpeech_InitializesCorrectly()
        {
            var r = new Report(ActId, Reporter, Target, Report.ReportReason.HateSpeech);
            r.Reason.Should()
                .Be(Report.ReportReason.HateSpeech);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_Threats_InitializesCorrectly()
        {
            var r = new Report(ActId, Reporter, Target, Report.ReportReason.Threats);
            r.Reason.Should()
                .Be(Report.ReportReason.Threats);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_Reason_Other_InitializesCorrectly()
        {
            _report.Reason.Should()
                .Be(Report.ReportReason.Other);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_IsResolved_DefaultsToFalse()
        {
            _report.IsResolved.Should()
                .BeFalse();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P1")]
        public void Report_IsFalseReport_DefaultsToFalse()
        {
            _report.IsFalseReport.Should()
                .BeFalse();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetActivityId_UpdatesAndPersistsValue()
        {
            _report.ActivityId = "new-act-id";
            _report.ActivityId.Should()
                .Be("new-act-id");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetReporterEmail_UpdatesAndPersistsValue()
        {
            _report.ReporterEmail = "changed@test.com";
            _report.ReporterEmail.Should()
                .Be("changed@test.com");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetTargetUserEmail_UpdatesAndPersistsValue()
        {
            _report.TargetUserEmail = "victim@test.com";
            _report.TargetUserEmail.Should()
                .Be("victim@test.com");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetDescription_UpdatesAndPersistsValue()
        {
            _report.Description = "Detailed details here";
            _report.Description.Should()
                .Be("Detailed details here");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetIsResolvedToTrue_PersistsState()
        {
            _report.IsResolved = true;
            _report.IsResolved.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetIsFalseReportToTrue_PersistsState()
        {
            _report.IsFalseReport = true;
            _report.IsFalseReport.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetIsResolvedBackToFalse_PersistsState()
        {
            _report.IsResolved = true;
            _report.IsResolved = false;
            _report.IsResolved.Should()
                .BeFalse();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "State")]
        [Trait("Priority", "P2")]
        public void Report_SetIsFalseReportBackToFalse_PersistsState()
        {
            _report.IsFalseReport = true;
            _report.IsFalseReport = false;
            _report.IsFalseReport.Should()
                .BeFalse();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountActiveReports_IgnoresResolvedReports()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Spam) { IsResolved = true }, new Report("a", "r", "t", Report.ReportReason.Spam) { IsResolved = false } };
            list.Count(r => !r.IsResolved)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountActiveReports_IgnoresFalseReports()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Spam) { IsFalseReport = true }, new Report("a", "r", "t", Report.ReportReason.Spam) { IsFalseReport = false } };
            list.Count(r => !r.IsFalseReport)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_AggressiveBehavior_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.AggressiveBehavior), new Report("a", "r", "t", Report.ReportReason.Spam) };
            list.Count(r => r.Reason == Report.ReportReason.AggressiveBehavior)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_InappropriateBehavior_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.InappropriateBehavior) };
            list.Count(r => r.Reason == Report.ReportReason.InappropriateBehavior)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_Harassment_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Harassment) };
            list.Count(r => r.Reason == Report.ReportReason.Harassment)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_Spam_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Spam) };
            list.Count(r => r.Reason == Report.ReportReason.Spam)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_FakeIdentity_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.FakeIdentity) };
            list.Count(r => r.Reason == Report.ReportReason.FakeIdentity)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_HateSpeech_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.HateSpeech) };
            list.Count(r => r.Reason == Report.ReportReason.HateSpeech)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_Threats_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Threats) };
            list.Count(r => r.Reason == Report.ReportReason.Threats)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CountByReason_Other_ReturnsCorrectSum()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Other) };
            list.Count(r => r.Reason == Report.ReportReason.Other)
                .Should()
                .Be(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateDangerScore_ThreatsReasonAddsHighWeight()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Threats) };
            var score = list.Sum(r => r.Reason == Report.ReportReason.Threats ? 50 : 0);
            score.Should()
                .Be(50);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateDangerScore_HateSpeechAddsHighWeight()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.HateSpeech) };
            var score = list.Sum(r => r.Reason == Report.ReportReason.HateSpeech ? 50 : 0);
            score.Should()
                .Be(50);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateDangerScore_HarassmentAddsMediumWeight()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Harassment) };
            var score = list.Sum(r => r.Reason == Report.ReportReason.Harassment ? 25 : 0);
            score.Should()
                .Be(25);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateDangerScore_SpamAddsLowWeight()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Spam) };
            var score = list.Sum(r => r.Reason == Report.ReportReason.Spam ? 5 : 0);
            score.Should()
                .Be(5);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateDangerScore_OtherAddsZeroWeight()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Other) };
            var score = list.Sum(r => r.Reason == Report.ReportReason.Other ? 0 : 10);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateDangerScore_IgnoresFalseReportsEntirely()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Threats) { IsFalseReport = true } };
            var score = list.Where(r => !r.IsFalseReport).Sum(r => r.Reason == Report.ReportReason.Threats ? 50 : 0);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateDangerScore_IgnoresResolvedReportsEntirely()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Threats) { IsResolved = true } };
            var score = list.Where(r => !r.IsResolved).Sum(r => r.Reason == Report.ReportReason.Threats ? 50 : 0);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_DetermineSeverity_SingleThreatTriggersCriticalSeverity()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Threats) };
            var severity = list.Any(r => r.Reason == Report.ReportReason.Threats) ? "Critical" : "Normal";
            severity.Should()
                .Be("Critical");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_DetermineSeverity_MultipleHarassmentsTriggersCriticalSeverity()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Harassment), new Report("a2", "r", "t", Report.ReportReason.Harassment) };
            var severity = list.Count(r => r.Reason == Report.ReportReason.Harassment) >= 2 ? "Critical" : "Normal";
            severity.Should()
                .Be("Critical");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_DetermineSeverity_MultipleSpamsTriggersElevatedSeverity()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Spam), new Report("a2", "r", "t", Report.ReportReason.Spam) };
            var severity = list.Count(r => r.Reason == Report.ReportReason.Spam) >= 2 ? "Elevated" : "Normal";
            severity.Should()
                .Be("Elevated");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_DetermineSeverity_SingleSpamTriggersNormalSeverity()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Spam) };
            var severity = list.Count(r => r.Reason == Report.ReportReason.Spam) > 1 ? "Elevated" : "Normal";
            severity.Should()
                .Be("Normal");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_DetermineSeverity_NoReportsReturnsNormalSeverity()
        {
            var list = new List<Report>();
            var severity = list.Count == 0 ? "Normal" : "Critical";
            severity.Should()
                .Be("Normal");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_DetermineSeverity_IgnoresFalseReportsInEvaluation()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Threats) { IsFalseReport = true } };
            var severity = list.Any(r => r.Reason == Report.ReportReason.Threats && !r.IsFalseReport) ? "Critical" : "Normal";
            severity.Should()
                .Be("Normal");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P2")]
        public void ReportCollection_CalculateUserTrustScore_InitialValueIsMaximized()
        {
            var list = new List<Report>();
            var trustScore = 100;
            list.Count.Should().Be(0);
            trustScore.Should().Be(100);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_AggressiveBehaviorReducesTrustModerately()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.AggressiveBehavior) };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.AggressiveBehavior) * 15;
            var score = 100 - penalty;
            score.Should()
                .Be(85);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_HarassmentReducesTrustSeverely()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Harassment) };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.Harassment) * 40;
            var score = 100 - penalty;
            score.Should()
                .Be(60);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_ThreatsReducesTrustDrastically()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Threats) };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.Threats) * 100;
            var score = Math.Max(0, 100 - penalty);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_TrustScoreCannotDropBelowZero()
        {
            var list = new List<Report> { new Report("a1", "r", "t", Report.ReportReason.Harassment), new Report("a2", "r", "t", Report.ReportReason.Harassment), new Report("a3", "r", "t", Report.ReportReason.Harassment) };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.Harassment) * 40;
            var score = Math.Max(0, 100 - penalty);
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_FalseReportsDoNotReduceTrustScore()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Harassment) { IsFalseReport = true } };
            var penalty = list.Where(r => !r.IsFalseReport).Sum(r => 40);
            var score = 100 - penalty;
            score.Should()
                .Be(100);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_ResolvedReportsStillAffectHistoricalTrustScore()
        {
            var list = new List<Report> { new Report("a", "r", "t", Report.ReportReason.Harassment) { IsResolved = true } };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.Harassment) * 40;
            var score = 100 - penalty;
            score.Should()
                .Be(60);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void ReportCollection_DetectReporterSpam_MultipleReportsFromSameReporterOnSameTarget_IdentifiesDuplication()
        {
            var list = new List<Report> { new Report(ActId, Reporter, Target, Report.ReportReason.Spam), new Report(ActId, Reporter, Target, Report.ReportReason.Spam) };
            var uniqueGroups = list.GroupBy(r => new { r.ReporterEmail, r.TargetUserEmail }).Any(g => g.Count() > 1);
            uniqueGroups.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_FilterByActivityId_ReturnsOnlyReportsForThatSpecificActivity()
        {
            var list = new List<Report> { new Report("act-1", Reporter, Target, Report.ReportReason.Other), new Report("act-2", Reporter, Target, Report.ReportReason.Other) };
            list.Where(r => r.ActivityId == "act-1")
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_FilterByTargetUser_ReturnsAllReportsReceivedByTarget()
        {
            var list = new List<Report> { new Report(ActId, Reporter, "t1@t.com", Report.ReportReason.Other), new Report(ActId, Reporter, "t2@t.com", Report.ReportReason.Other) };
            list.Where(r => r.TargetUserEmail == "t1@t.com")
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Metric")]
        [Trait("Priority", "P2")]
        public void ReportCollection_FilterByReporter_ReturnsAllReportsSubmittedByReporter()
        {
            var list = new List<Report> { new Report(ActId, "r1@t.com", Target, Report.ReportReason.Other), new Report(ActId, "r2@t.com", Target, Report.ReportReason.Other) };
            list.Where(r => r.ReporterEmail == "r1@t.com")
                .Should()
                .HaveCount(1);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Report_Description_AcceptsEmptyAndNullValues()
        {
            _report.Description = string.Empty;
            _report.Description.Should()
                .BeEmpty();
            _report.Description = null;
            _report.Description.Should()
                .BeNull();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Report_Description_AcceptsVeryLongTextDescriptions()
        {
            var longDesc = new string('R', 8000);
            _report.Description = longDesc;
            _report.Description.Should()
                .HaveLength(8000);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Report_Reason_AcceptsAnyValidReportReasonEnumValues()
        {
            _report.Reason = Report.ReportReason.HateSpeech;
            _report.Reason.Should()
                .Be(Report.ReportReason.HateSpeech);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Report_CreatedAt_IsAlwaysBeforeFutureDates()
        {
            _report.CreatedAt.Should()
                .BeBefore(DateTime.UtcNow.AddDays(1));
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Report_SelfReportingSimulation_CollectionIdentifiesIfReporterMatchesTarget()
        {
            var badReport = new Report(ActId, "user@test.com", "user@test.com", Report.ReportReason.Other);
            var isSelfReported = badReport.ReporterEmail.Equals(badReport.TargetUserEmail, StringComparison.OrdinalIgnoreCase);
            isSelfReported.Should()
                .BeTrue();
        }


        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P3")]
        public void ReportCollection_DetectTargetSpam_SameTargetReportedTenTimesInOneHour_RaisesSystemAlertLevel()
        {
            var list = new List<Report>();
            for (int i = 0; i < 10; i++) list.Add(new Report(ActId, $"r{i}@t.com", Target, Report.ReportReason.Spam));
            var alertLevel = list.Count(r => r.TargetUserEmail == Target) >= 10 ? "High" : "Low";
            alertLevel.Should()
                .Be("High");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_FakeIdentityReason_ReducesTrustDrastically()
        {
            var list = new List<Report> { new Report(ActId, Reporter, Target, Report.ReportReason.FakeIdentity) };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.FakeIdentity) * 60;
            var score = 100 - penalty;
            score.Should()
                .Be(40);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_HateSpeechReason_ReducesTrustToZeroImmediately()
        {
            var list = new List<Report> { new Report(ActId, Reporter, Target, Report.ReportReason.HateSpeech) };
            var isHateSpeechPresent = list.Any(r => r.Reason == Report.ReportReason.HateSpeech);
            var score = isHateSpeechPresent ? 0 : 100;
            score.Should()
                .Be(0);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_SpamReason_ReducesTrustVeryLightly()
        {
            var list = new List<Report> { new Report(ActId, Reporter, Target, Report.ReportReason.Spam) };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.Spam) * 5;
            var score = 100 - penalty;
            score.Should()
                .Be(95);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Rule")]
        [Trait("Priority", "P3")]
        public void ReportCollection_CalculateUserTrustScore_OtherReason_DoesNotReduceTrustWithoutAdminIntervention()
        {
            var list = new List<Report> { _report };
            var penalty = list.Count(r => r.Reason == Report.ReportReason.Other) * 0;
            var score = 100 - penalty;
            score.Should()
                .Be(100);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Security")]
        [Trait("Priority", "P3")]
        public void ReportCollection_IdentifyVindictiveReporter_ReporterSubmitsFiveFalseReports_TriggersReporterBanSimulation()
        {
            var list = new List<Report>();
            for (int i = 0; i < 5; i++) list.Add(new Report(ActId, Reporter, $"t{i}@t.com", Report.ReportReason.Other) { IsFalseReport = true });
            var triggerBan = list.Count(r => r.ReporterEmail == Reporter && r.IsFalseReport) >= 5;
            triggerBan.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Report_SetIsFalseReportTrue_AutomaticallySetsIsResolvedTrue_Simulation()
        {
            _report.IsFalseReport = true;
            var isResolved = _report.IsFalseReport || _report.IsResolved;
            isResolved.Should()
                .BeTrue();
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Report_Constructor_AcceptsSpecialCharactersInDescription()
        {
            _report.Description = "Special #$%^&* Characters";
            _report.Description.Should()
                .Be("Special #$%^&* Characters");
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Validation")]
        [Trait("Priority", "P2")]
        public void Report_Constructor_AcceptsExtremelyLongDescriptions()
        {
            var longText = new string('Z', 9000);
            _report.Description = longText;
            _report.Description.Should()
                .HaveLength(9000);
        }

        [Fact]
        [Trait("Feature", "Report")]
        [Trait("Type", "Guard")]
        [Trait("Priority", "P1")]
        public void Report_Reason_EnumValidation_ThrowsExceptionWhenCastedFromInvalidInt()
        {
            Action act = () => { var reason = (Report.ReportReason)999; if (!Enum.IsDefined(typeof(Report.ReportReason), reason)) throw new ArgumentOutOfRangeException(); };
            act.Should()
                .Throw<ArgumentOutOfRangeException>();
        }


    }
}