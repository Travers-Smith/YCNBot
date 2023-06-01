using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IChatCompletionRepository AzureOpenAIChatCompletion { get; }

        IChatRepository Chat { get; }

        ICommunityPromptRepository CommunityPrompt { get; }

        ICommunityPromptCommentRepository CommunityPromptComment { get; }

        ICommunityPromptLikeRepository CommunityPromptLike { get; }

        IChatCompletionRepository OpenAIChatCompletion { get; }

        IMessageRepository Message { get; }

        INameRepository Name { get; }

        IStopWordRepository StopWord { get; }

        IUserRepository User { get; }

        IUserAgreedTermsRepository UserAgreedTerms { get; }

        IRepository<UserFeedback> UserFeedback { get; }

        void Commit();

        Task CommitAsync();

    }
}
