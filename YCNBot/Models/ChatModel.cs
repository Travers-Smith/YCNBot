namespace YCNBot.Models
{
    public class ChatModel
    {
        public string Name { get; set; }

        public Guid UniqueIdentifier { get; set; }

        public IEnumerable<MessageModel> Messages { get; set; }
    }
}
