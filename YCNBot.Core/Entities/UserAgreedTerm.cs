namespace YCNBot.Core.Entities;

public partial class UserAgreedTerm
{
    public int Id { get; set; }

    public Guid UserIdentifier { get; set; }

    public bool Agreed { get; set; }
}
