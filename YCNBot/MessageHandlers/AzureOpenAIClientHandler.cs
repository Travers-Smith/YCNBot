using System.Diagnostics.CodeAnalysis;

namespace YCNBot.MessageHandlers
{
    [ExcludeFromCodeCoverage]
    public class AzureOpenAIClientHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;

        public AzureOpenAIClientHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("api-key", _configuration["AzureOpenAIKey"]);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
