using YCNBot.Data.Repositories;
using YCNBot.Core;
using YCNBot.Core.Repositories;

namespace YCNBot.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IChatRepository? _chat;
        private IChatCompletionRepository _azureOpenAIChatCompletion;
        private IChatCompletionRepository _openAIChatCompletion;
        private IMessageRepository? _message;
        private IUserAgreedTermsRepository? _userAgreedTerms; 

        private readonly YCNBotContext _context;
        private readonly HttpClient _openAIClient;
        private readonly HttpClient _azureOpenAIClient;

        public UnitOfWork(YCNBotContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _azureOpenAIClient = httpClientFactory.CreateClient("AzureOpenAIClient");
            _openAIClient = httpClientFactory.CreateClient("OpenAIClient");
        }

        public IChatCompletionRepository AzureOpenAIChatCompletion => _azureOpenAIChatCompletion ??= new AzureChatCompletionRepository(_azureOpenAIClient);

        public IChatRepository Chat => _chat ??= new ChatRepository(_context);

        public IChatCompletionRepository OpenAIChatCompletion => _openAIChatCompletion ??= new OpenAIChatCompletionRepository(_openAIClient);

        public IMessageRepository Message => _message ??= new MessageRepository(_context);

        public IUserAgreedTermsRepository UserAgreedTerms => _userAgreedTerms ??= new UserAgreedTermsRepository(_context);

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
