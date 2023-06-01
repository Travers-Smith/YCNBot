namespace YCNBot.Core.Entities
{
    public class ChatCompletionUsage
    {
        public int PromptToken { get; set; }

        public int CompletionTokens { get; set; }

        public int TotalTokens { get; set; }
    }
}
