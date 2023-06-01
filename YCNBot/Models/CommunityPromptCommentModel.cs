namespace YCNBot.Models
{
    public class CommunityPromptCommentModel
    {
        public string Comment { get; set; }

        public DateTime DateAdded { get; set; }

        public UserModel User { get; set; }
    }
}
