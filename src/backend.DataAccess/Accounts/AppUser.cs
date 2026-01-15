using backend.Core.Accounts;
using Microsoft.AspNetCore.Identity;

namespace backend.DataAccess.Accounts
{
    public class AppUser : IdentityUser
    {
        public UserDto GetDto()
        {
            return new UserDto
            {
                Id = Id,
                Email = Email ?? string.Empty,
                UserName = UserName
            };
        }
    }
}
