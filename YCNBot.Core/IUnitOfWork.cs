using YCNBot.Core.Repositories;

namespace YCNBot.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IChatCompletionRepository AzureOpenAIChatCompletion { get; }

        IChatRepository Chat { get; }

        IChatCompletionRepository OpenAIChatCompletion { get; }

        IMessageRepository Message { get; }

        IUserAgreedTermsRepository UserAgreedTerms { get; }

        void Commit();

        Task CommitAsync();

    }
}
