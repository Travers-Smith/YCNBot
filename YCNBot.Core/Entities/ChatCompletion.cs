namespace YCNBot.Core.Entities
{
    public class ChatCompletion
    {
        public string Id { get; set; }

        public string Object { get; set; }

        public int Created { get; set; }

        public IEnumerable<ChatCompletionChoice> Choices { get; set; }

    }
}
