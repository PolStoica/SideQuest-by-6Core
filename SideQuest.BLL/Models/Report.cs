using System;

namespace SideQuest.BLL.Models
{
    public class Report
    {
        public enum ReportReason
        {
            AggressiveBehavior,
            InappropriateBehavior,
            Harassment,
            Spam,
            FakeIdentity,
            HateSpeech,
            Threats,
            Other
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string ActivityId { get; set; }

        public string ReporterEmail { get; set; }

        public string TargetUserEmail { get; set; }

        public ReportReason Reason { get; set; }

        public string Description { get; set; }

        public bool IsResolved { get; set; }

        public bool IsFalseReport { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Report(string activityId, string reporterEmail, string targetUserEmail, ReportReason reason)
        {
            ActivityId = activityId;
            ReporterEmail = reporterEmail;
            TargetUserEmail = targetUserEmail;
            Reason = reason;
        }
    }
}