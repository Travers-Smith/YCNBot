﻿namespace YCNBot.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Department { get; set; }

        public string? JobTitle { get; set; }

    }
}
