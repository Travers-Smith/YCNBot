namespace YCNBot.Core.Entities;

public partial class CommunityPromptLike
{
    public int Id { get; set; }

    public int CommunityPromptId { get; set; }

    public Guid UserIdentifier { get; set; }

    public DateTime DateAdded { get; set; }

    public virtual CommunityPrompt CommunityPrompt { get; set; } = null!;
}
