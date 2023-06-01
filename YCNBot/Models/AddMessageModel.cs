namespace YCNBot.Models
{
    public class AddMessageModel
    {
        public bool AllowPersonalInformation { get; set; }

        public Guid? ChatIdentifier { get; set; }

        public string Message { get; set; }
    }
}
