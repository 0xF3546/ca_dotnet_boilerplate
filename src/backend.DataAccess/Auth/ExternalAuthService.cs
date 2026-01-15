using backend.Core.Accounts;
using backend.Core.Auth;
using backend.DataAccess.Accounts;
using Microsoft.AspNetCore.Identity;

namespace backend.DataAccess.Auth
{
    public class ExternalAuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IExternalAuthService
    {

        public async Task<UserDto?> HandleExternalLoginAsync(ExternalAuthDto externalAuthDto)
        {
            if (string.IsNullOrEmpty(externalAuthDto.Email))
            {
                return null;
            }

            var existingUser = await userManager.FindByEmailAsync(externalAuthDto.Email);
            
            if (existingUser != null)
            {
                var existingLogin = await userManager.FindByLoginAsync(
                    externalAuthDto.Provider, 
                    externalAuthDto.ProviderKey
                );

                if (existingLogin == null)
                {
                    var existingUserLoginInfo = new UserLoginInfo(
                        externalAuthDto.Provider, 
                        externalAuthDto.ProviderKey, 
                        externalAuthDto.Provider
                    );
                    var existingAddLoginResult = await userManager.AddLoginAsync(existingUser, existingUserLoginInfo);
                    
                    if (!existingAddLoginResult.Succeeded)
                    {
                        return null;
                    }
                }

                await signInManager.SignInAsync(existingUser, isPersistent: true);
                return existingUser.GetDto();
            }

            var newUser = new AppUser
            {
                UserName = externalAuthDto.Email,
                Email = externalAuthDto.Email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                return null;
            }

            var loginInfo = new UserLoginInfo(
                externalAuthDto.Provider, 
                externalAuthDto.ProviderKey, 
                externalAuthDto.Provider
            );
            var addLoginResult = await userManager.AddLoginAsync(newUser, loginInfo);
            
            if (!addLoginResult.Succeeded)
            {
                await userManager.DeleteAsync(newUser);
                return null;
            }

            await signInManager.SignInAsync(newUser, isPersistent: true);
            return newUser.GetDto();
        }
    }
}
