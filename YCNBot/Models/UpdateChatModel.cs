using System.ComponentModel.DataAnnotations;

namespace YCNBot.Models
{
    public class UpdateChatModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public Guid ChatIdentifier { get; set; }
    }
}
