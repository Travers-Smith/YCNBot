namespace YCNBot.Models
{
    public class UpdateMessageRatingModel
    {
        public Guid MessageIdentifier { get; set; }

        public int Rating { get; set; }
    }
}
