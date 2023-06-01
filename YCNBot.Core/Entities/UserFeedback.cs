namespace YCNBot.Core.Entities;

public partial class UserFeedback
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public Guid UserIdentifier { get; set; }

    public int FeedbackTypeId { get; set; }

    public virtual FeedbackType FeedbackType { get; set; } = null!;
}
