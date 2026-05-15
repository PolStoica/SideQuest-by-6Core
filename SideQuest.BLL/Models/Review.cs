namespace SideQuest.BLL.Models
{
    public class Review
    {
        public enum ReviewRating
        {
            Unpleasant,
            Pleasant,
            Exceptional
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string ActivityId { get; private set; }
        public string ReviewerEmail { get; private set; }
        public string TargetUserEmail { get; private set; }
        public ReviewRating Rating { get; set; }
        public string? Description { get; set; }

        public bool WasLate { get; set; }
        public bool WasAggressive { get; set; }
        public bool WasInappropriate { get; set; }
        public bool DidNotShowUp { get; set; }
        public bool SpammedChat { get; set; }

        public bool WasFunny { get; set; }
        public bool WasFriendly { get; set; }
        public bool WasHelpful { get; set; }
        public bool WasOnTime { get; set; }
        public bool BroughtGoodVibes { get; set; }

        public bool IsEmergencyReview { get; set; }
        public bool IsDisputed { get; set; }
        public bool IsValidatedByAdmin { get; set; } = true;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Review(string activityId, string reviewerEmail, string targetUserEmail, bool isEmergency = false)
        {
            if (string.IsNullOrWhiteSpace(activityId))
                throw new ArgumentException("ActivityId invalid");

            if (string.IsNullOrWhiteSpace(reviewerEmail))
                throw new ArgumentException("ReviewerEmail invalid");

            if (string.IsNullOrWhiteSpace(targetUserEmail))
                throw new ArgumentException("TargetUserEmail invalid");

            if (reviewerEmail == targetUserEmail)
                throw new InvalidOperationException("Reviewer cannot review himself");

            ActivityId = activityId;
            ReviewerEmail = reviewerEmail;
            TargetUserEmail = targetUserEmail;
            IsEmergencyReview = isEmergency;
        }

        public void Validate()
        {
            if (Rating == ReviewRating.Unpleasant && string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("Descrierea este obligatorie pentru un rating neplacut.");
        }
    }
}