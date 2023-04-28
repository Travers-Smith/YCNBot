using YCNBot.Data.Repositories;
using YCNBot.Core;
using YCNBot.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace YCNBot.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IChatRepository? _chat;
        private IChatCompletionRepository? _openAIChatCompletion;
        private IConfiguration _configuration;
        private IMessageRepository? _message;
        private INameRepository? _name;
        private IStopWordRepository? _stopWord;
        private IUserRepository? _user;
        private IUserAgreedTermsRepository? _userAgreedTerms;
        private IRepository<Core.Entities.UserFeedback>? _userFeedback;

        private readonly YCNBotContext _context;
        private readonly GraphServiceClient _graphServiceClient;
        private readonly HttpClient _openAIClient;
        private readonly HttpClient _azureOpenAIClient;

        public UnitOfWork(IConfiguration configuration, YCNBotContext context, GraphServiceClient graphServiceClient, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _azureOpenAIClient = httpClientFactory.CreateClient("AzureOpenAIClient");
            _graphServiceClient = graphServiceClient;
            _openAIClient = httpClientFactory.CreateClient("OpenAIClient");
        }

        public IChatCompletionRepository AzureOpenAIChatCompletion => _openAIChatCompletion ??= new AzureChatCompletionRepository(_azureOpenAIClient, _configuration);

        public IChatRepository Chat => _chat ??= new ChatRepository(_context);

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
