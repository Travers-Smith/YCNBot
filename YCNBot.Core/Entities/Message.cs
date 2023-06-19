namespace YCNBot.Core.Entities;

public partial class Message
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsSystem { get; set; }

    public int ChatId { get; set; }

    public Guid UniqueIdentifier { get; set; }

    public int? Rating { get; set; }

    public DateTime? DateAdded { get; set; }

    public bool? ContainsCaseLaw { get; set; }

    public virtual Chat Chat { get; set; } = null!;
}
