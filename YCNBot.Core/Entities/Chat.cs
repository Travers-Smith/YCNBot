namespace YCNBot.Core.Entities;

public partial class Chat
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid UniqueIdentifier { get; set; }

    public Guid UserIdentifier { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Message> Messages { get; } = new List<Message>();
}
