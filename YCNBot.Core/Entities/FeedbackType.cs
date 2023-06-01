namespace YCNBot.Core.Entities;

public partial class FeedbackType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserFeedback> UserFeedbacks { get; } = new List<UserFeedback>();
}
