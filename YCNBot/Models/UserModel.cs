namespace YCNBot.Models
{
    public class UserModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Department { get; set; }

        public string? JobTitle { get; set; }

        public bool AgreedToTerms { get; set; }

        public bool IsAdmin { get; set; }
    }
}
