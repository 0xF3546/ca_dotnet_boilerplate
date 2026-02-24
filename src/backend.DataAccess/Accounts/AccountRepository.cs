using backend.Core.Accounts;
using Microsoft.AspNetCore.Identity;

namespace backend.DataAccess.Accounts
{
    public class AuthRepository(UserManager<AppUser> userManager) : IAccountCrud
    {
        /// <inheritdoc />
        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            if (createUserDto == null)
                throw new ArgumentNullException(nameof(createUserDto));

            if (string.IsNullOrWhiteSpace(createUserDto.Email) || string.IsNullOrWhiteSpace(createUserDto.UserName))
                throw new ArgumentException("Email and username are required.");

            var existingUser = await userManager.FindByEmailAsync(createUserDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var user = new AppUser
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName,
            };

            var createResult = await userManager.CreateAsync(user, createUserDto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            var createdUser = await userManager.FindByEmailAsync(user.Email);
            return createdUser?.GetDto() ?? throw new InvalidOperationException("User created but could not be retrieved.");
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var deleteResult = await userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to delete user: {errors}");
            }
        }

        /// <inheritdoc/>
        public Task<UserDto?> GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            return userManager.FindByEmailAsync(email)
                .ContinueWith(task => task.Result?.GetDto());
        }

        /// <inheritdoc/>
        public Task<UserDto?> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(id));

            return userManager.FindByIdAsync(id.ToString())
                .ContinueWith(task => task.Result?.GetDto());
        }
    }
}
