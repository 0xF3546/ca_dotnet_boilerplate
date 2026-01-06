using backend.Core.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Core.Accounts
{
    public interface IAccountCrud
    {
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task DeleteAsync(Guid id);
        Task<UserDto?> GetById(Guid id);
        Task<UserDto?> GetByEmail(string email);

    }
}
