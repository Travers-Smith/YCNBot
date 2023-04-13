namespace YCNBot.Models
{
    public class MessageModel
    {
        public string Text { get; set; }

        public bool IsSystem { get; set; }

        public int? Rating { get; set; }

        public ChatModel Chat { get; set; }

        public Guid UniqueIdentifier { get; set; }
    }
}
