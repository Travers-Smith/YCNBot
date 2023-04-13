namespace YCNBot.Core.Entities
{
    public class ChatCompletionChoice
    {
        public int Index { get; set; }

        public string FinishReason { get; set; }

        public ChatCompletionChoiceMessage Message { get; set; }
    }
}