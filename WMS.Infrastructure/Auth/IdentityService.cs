using Microsoft.AspNetCore.Identity;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities.Auth;
using WMS.Infrastructure.Data.Identity;

namespace WMS.Infrastructure.Auth
{
    public class IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        ITokenService tokenService) : IIdentityService
    {
        public async Task<Result<bool>> AssignRole(string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Errors<ApplicationUser>.NotFound;
            }
            if (!await RoleExistsAsync(role))
            {
                return Errors.NotExists;
            }

            var removeResult = await RemoveCurrentRoleAsync(role, user);
            if (!removeResult.IsSuccess)
            {
                return removeResult.Error!;
            }

            var addResult = await userManager.AddToRoleAsync(user, role);
            if (!addResult.Succeeded)
            {
                return Errors<ApplicationUser>.OperationFailed;
            }
            await userManager.AddToRoleAsync(user, role);
            await userManager.UpdateSecurityStampAsync(user);
            return true;
        }


        public async Task<Result<AuthToken>> LoginUserAsync(string email, string password, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (!await VerifyUser(password, user))
            {
                return Errors.Unauthorized;
            }
            AuthToken authToken = await tokenService.CreateTokenAsync(user!.Id);
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
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone,
                IsActive = true,
                CratedAt = DateTimeOffset.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user,password);

            if(!result.Succeeded)
            {
                return Errors<ApplicationUser>.OperationFailed;
            }

            await userManager.AddToRoleAsync(user, Roles.User);
            return true;
        }
        private async Task<Result<bool>> RemoveCurrentRoleAsync(string role, ApplicationUser user)
        {
            var currentRole = await userManager.GetRolesAsync(user);
            if (currentRole.Count == 1 && currentRole.Contains(role))
            {
                return true;
            }

            if (currentRole.Count > 0)
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, currentRole);
                if (!removeResult.Succeeded)
                {
                    return Errors<ApplicationUser>.OperationFailed;
                }
            }

            return true;
        }

        private async Task<bool> RoleExistsAsync(string role)
        {
            return await roleManager.RoleExistsAsync(role);
        }
        private async Task<bool> VerifyUser(string password, ApplicationUser? user)
        {
            return user is null || !await userManager.CheckPasswordAsync(user, password);
        }
    }
}
