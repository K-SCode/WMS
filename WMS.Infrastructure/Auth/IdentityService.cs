using Microsoft.AspNetCore.Identity;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities.Auth;
using WMS.Infrastructure.Data.Identity;

namespace WMS.Infrastructure.Auth
{
    public class IdentityService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService) : IIdentityService
    {
        public async Task<Result<AuthToken>> LoginUserAsync(string email, string password, CancellationToken ct)
        {
           var user = await userManager.FindByEmailAsync(email);
            if (user is null || !await userManager.CheckPasswordAsync(user,password))
            {
                return Errors.Unauthorized;
            }
            AuthToken authToken = await tokenService.CreateTokenAsync(user.Id);
            return authToken;
        }

        public async Task<Result<bool>> RegisterUserAsync(
            string firstName,
            string lastName,
            string password,
            string email,
            string phone,
            CancellationToken ct)
        {
            if(await userManager.FindByEmailAsync(email) is not null)
            {
                return Errors<ApplicationUser>.ObjectWithIdExists(email);
            }

            var user = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone
            };

            var result = await userManager.CreateAsync(user);

            if(!result.Succeeded)
            {
                return Errors<ApplicationUser>.OperationFailed;
            }

            await userManager.AddToRoleAsync(user, Roles.User);
            return true;
        }
    }
}
