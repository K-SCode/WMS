using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System.IdentityModel.Tokens.Jwt;
using WMS.Core.Common;
using WMS.Infrastructure.Auth;
using WMS.Infrastructure.Data.Identity;
using Xunit;

namespace WMS.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly TokenService _sut;


        private const int ExpiryMinutes = 240;
        private const string Issuer = "testIssuer";
        private const string Audience = "testAudience";
        public TokenServiceTests()
        {
            var settings = Options.Create(new JwtSettings
            {
                ExpiryMinutes = ExpiryMinutes,
                Issuer = Issuer,
                Audience = Audience,
                Key = "abcde-secret-test-key-at-least-32-chars-long"
            });

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(
                userStore.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            _sut = new TokenService(settings, _userManager.Object);
        }

        [Fact]
        public async Task CreateTokenAsync_ValidData_ReturnAuthToken()
        {
            //Arrange
            var timeNow = DateTime.UtcNow;
            int userId = 23;
            var user = new ApplicationUser() { Id = userId, Email = "test@test.com"};
            var role = Roles.Analyst;


            _userManager.Setup(
                m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _userManager.Setup(
                m => m.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync([role]);
           

            //act
            var result = await _sut.CreateTokenAsync(userId);

            //Assert
            result.ExpiresAt.ShouldBe(timeNow.AddMinutes(ExpiryMinutes), TimeSpan.FromSeconds(5));
            result.Token.ShouldNotBeNullOrWhiteSpace();

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.Token);
            jwt.Issuer.ShouldBe(Issuer);
            jwt.Audiences.ShouldContain(Audience);
            jwt.Claims.ShouldContain(c => c.Value == role);

            _userManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once());
            _userManager.Verify(m => m.GetRolesAsync(user), Times.Once());

        }

        [Fact]
        public async Task CreateTokenAsync_UserNotFound_ThrowException()
        {
            //arrange
            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync((ApplicationUser?)null);

            //Act, Assert
            await Should.ThrowAsync<Exception>(() => _sut.CreateTokenAsync(99));
        }
    }
}
