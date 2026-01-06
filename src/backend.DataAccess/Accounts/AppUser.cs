using backend.Core.Accounts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DataAccess.Accounts
{
    public class AppUser : IdentityUser
    {
        public UserDto GetDto()
        {
            return new();
        }
    }
}
