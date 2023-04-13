namespace YCNBot.Core.Repositories
{
    public interface IUserRepository
    {
        Task<Stream?> GetUserInformation(string emailAddress);
    }
}