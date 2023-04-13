using System.Net.Http.Headers;

namespace YCNBot.MessageHandlers
{
    public class OpenAIClientHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;

        public OpenAIClientHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["OpenAIKey"]);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
