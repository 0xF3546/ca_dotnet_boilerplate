namespace backend.Core.Accounts
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public string? UserName { get; set; }
    }
}
