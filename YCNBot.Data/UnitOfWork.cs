using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using YCNBot.Core;
using YCNBot.Core.Repositories;
using YCNBot.Data.Repositories;

namespace YCNBot.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IChatCompletionRepository? _azureOpenAIChatCompletion;
        private IChatRepository? _chat;
        private ICommunityPromptRepository? _communityPrompt;
        private ICommunityPromptCommentRepository? _communityPromptComment;
        private ICommunityPromptLikeRepository? _communityPromptLike;
        private IChatCompletionRepository? _openAIChatCompletion;
        private IConfiguration _configuration;
        private IMessageRepository? _message;
        private INameRepository? _name;
        private IStopWordRepository? _stopWord;
        private IUserRepository? _user;
        private IUserAgreedTermsRepository? _userAgreedTerms;
        private IRepository<Core.Entities.UserFeedback>? _userFeedback;

        private readonly YCNBotContext _context;
        private readonly HttpClient _openAIClient;
        private readonly HttpClient _azureOpenAIClient;
        private readonly GraphServiceClient _graphServiceClient;

        public UnitOfWork(IConfiguration configuration, YCNBotContext context, GraphServiceClient graphServiceClient, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _azureOpenAIClient = httpClientFactory.CreateClient("AzureOpenAIClient");
            _graphServiceClient = graphServiceClient;
            _openAIClient = httpClientFactory.CreateClient("OpenAIClient");
        }

        public IChatCompletionRepository AzureOpenAIChatCompletion => _azureOpenAIChatCompletion ??= new AzureChatCompletionRepository(_azureOpenAIClient, _configuration);

        public IChatRepository Chat => _chat ??= new ChatRepository(_context);

        public ICommunityPromptRepository CommunityPrompt => _communityPrompt ??= new CommunityPromptRepository(_context);

        public ICommunityPromptCommentRepository CommunityPromptComment => _communityPromptComment ??= new CommunityPromptCommentRepository(_context);

        public ICommunityPromptLikeRepository CommunityPromptLike => _communityPromptLike ??= new CommunityPromptLikeRepository(_context);

        public IChatCompletionRepository OpenAIChatCompletion => _openAIChatCompletion ??= new OpenAIChatCompletionRepository(_openAIClient);

        public IMessageRepository Message => _message ??= new MessageRepository(_context);

        public INameRepository Name => _name ??= new NameRepository(_configuration);

        public IStopWordRepository StopWord => _stopWord ??= new StopWordRepository(_configuration);

        public IUserRepository User => _user ??= new UserRepository(_graphServiceClient);

        public IUserAgreedTermsRepository UserAgreedTerms => _userAgreedTerms ??= new UserAgreedTermsRepository(_context);

        public IRepository<Core.Entities.UserFeedback> UserFeedback => _userFeedback ??= new Repository<Core.Entities.UserFeedback>(_context);


        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
