namespace YCNBot.Models
{
    public class CommunityPromptModel
    {
        public int Id { get; set; }

        public string Answer { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public string Question { get; set; }

        public UserModel User { get; set; }

        public bool UserLiked { get; set; }

        public Guid UniqueIdentifier { get; set; }
    }
}
