using backend.Core.Roles;
using Microsoft.AspNetCore.Identity;

namespace backend.DataAccess.Accounts
{
    public class AppRole : IdentityRole
    {
        public RoleDto GetDto()
        {
            return new RoleDto
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
