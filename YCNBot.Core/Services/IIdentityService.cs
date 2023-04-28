namespace YCNBot.Core.Services
{
    public interface IIdentityService
    {
        string? GetEmail();

        string? GetName();

        Guid? GetUserIdentifier();

        bool IsAdmin();

        bool IsAuthenticated();

    }
}