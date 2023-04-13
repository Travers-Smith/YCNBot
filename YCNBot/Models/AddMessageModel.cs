namespace YCNBot.Models
{
    public class AddMessageModel
    {
        public Guid? ChatIdentifier { get; set; }

        public string Message { get; set; }
    }
}
