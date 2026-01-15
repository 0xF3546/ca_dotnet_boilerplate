namespace backend.Core.Auth
{
    public class ExternalAuthDto
    {
        public required string Provider { get; set; }
        public required string ProviderKey { get; set; }
        public required string Email { get; set; }
        public string? Name { get; set; }
    }
}
