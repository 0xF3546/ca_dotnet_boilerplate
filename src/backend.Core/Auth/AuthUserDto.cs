using backend.Core.Accounts;
using backend.Core.Roles;

namespace backend.Core.Auth
{
    public class AuthUserDto
    {
        public required UserDto User { get; set; }
        public List<RoleDto> Roles { get; set; } = [];
    }
}
