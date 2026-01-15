using backend.Core.Accounts;

namespace backend.Core.Auth
{
    public interface IExternalAuthService
    {
        Task<UserDto?> HandleExternalLoginAsync(ExternalAuthDto externalAuthDto);
    }
}
