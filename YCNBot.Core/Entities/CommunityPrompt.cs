namespace YCNBot.Core.Entities;

public partial class CommunityPrompt
{
    public int Id { get; set; }

    public string Question { get; set; } = null!;

    public Guid UserIdentifier { get; set; }

    public Guid UniqueIdentifier { get; set; }

    public DateTime DateAdded { get; set; }

    public string Answer { get; set; } = null!;

    public virtual ICollection<CommunityPromptComment> CommunityPromptComments { get; } = new List<CommunityPromptComment>();

    public virtual ICollection<CommunityPromptLike> CommunityPromptLikes { get; } = new List<CommunityPromptLike>();
}
