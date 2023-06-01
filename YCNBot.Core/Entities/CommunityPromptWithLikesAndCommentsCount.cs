namespace YCNBot.Core.Entities
{
    public class CommunityPromptWithLikesAndCommentsCount
    {
        public CommunityPrompt CommunityPrompt { get; set; } = null!;

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

    }
}
