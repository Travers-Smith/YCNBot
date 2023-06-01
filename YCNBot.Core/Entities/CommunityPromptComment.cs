namespace YCNBot.Core.Entities;

public partial class CommunityPromptComment
{
    public int Id { get; set; }

    public string Comment { get; set; } = null!;

    public int CommunityPromptId { get; set; }

    public DateTime DateAdded { get; set; }

    public Guid UserIdentifier { get; set; }

    public virtual CommunityPrompt CommunityPrompt { get; set; } = null!;
}
