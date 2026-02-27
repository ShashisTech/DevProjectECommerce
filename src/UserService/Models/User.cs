namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? SecretQuestionHash { get; set; }
        public string? SecretAnswerHash { get; set; }
        public string Role { get; set; } // Admin, Buyer, Seller
        public bool IsSeller { get; set; }
        public bool IsActive { get; set; } = true;
        public bool RentPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int FailedLoginCount { get; set; }
    }
}
