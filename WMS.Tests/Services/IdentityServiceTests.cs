using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities.Auth;
using WMS.Infrastructure.Auth;
using WMS.Infrastructure.Data.Identity;
using Xunit;

namespace WMS.Tests.Services
{
    public class IdentityServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<RoleManager<IdentityRole<int>>> _roleManager;
        private readonly Mock<ITokenService> _tokenService;
        private readonly IdentityService _sut;
        public IdentityServiceTests()
        {
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

            var roleStore = new Mock<IRoleStore<IdentityRole<int>>>();
            _roleManager = new Mock<RoleManager<IdentityRole<int>>>(
                roleStore.Object,
                null!,
                null!,
                null!,
                null!);
            _tokenService = new Mock<ITokenService>();

            _sut = new IdentityService(_userManager.Object, _roleManager.Object,_tokenService.Object);
        }

        [Fact]
        public async Task AssignRole_AssignNewRoleAndRefreshStamp_ReturnBoolValue()
        {
            //arrange
            string email = "test@test.com";
            var user = new ApplicationUser() { Email = email };
            var role = Roles.Manager;
            var oldRoles = new string[] { Roles.Analyst};

            _roleManager.Setup(
                m => m.RoleExistsAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManager.Setup(
                m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            
            _userManager.Setup(
                m => m.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(oldRoles);

            _userManager.Setup(
                m => m.RemoveFromRolesAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(
                m => m.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(
                m => m.UpdateSecurityStampAsync(It.IsAny<ApplicationUser>()));

            //act
            var result = await _sut.AssignRole(email, role);

            //assert
            result.IsSuccess.ShouldBeTrue();
            _roleManager.Verify(
                m => m.RoleExistsAsync(
                    role),
                Times.Once);

            _userManager.Verify(
               m => m.FindByEmailAsync(email),
               Times.Once);

            _userManager.Verify(
                m => m.GetRolesAsync(user),
                Times.Once);

            _userManager.Verify(
                m => m.RemoveFromRolesAsync(
                    user,
                    oldRoles),
                Times.Once);

            _userManager.Verify(
                m => m.AddToRoleAsync(
                    user,
                    role),
                Times.Once);

            _userManager.Verify(
                m => m.UpdateSecurityStampAsync(
                    user),
                Times.Once);
        }
        [Fact]
        public async Task AssignRole_UserNotExists_RetrunNotFoundError()
        {

            //arrange
            string email = "test@test.com";
            ApplicationUser user = null!;
            var role = Roles.Manager;

            _userManager.Setup(
                m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            //act
            var result = await _sut.AssignRole(email, role);

            //assert

            result.IsSuccess.ShouldBeFalse();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
            result.Error.ShouldBeOfType(typeof(Error));

            _userManager.Verify(
               m => m.FindByEmailAsync(email),
               Times.Once);
        }
        [Fact]
        public async Task AssingRole_RoleNotExists_ReturnNotExistsError()
        {
            //arrange
            string email = "test@test.com";
            ApplicationUser user = new() { Email = email };
            var role = Roles.Manager;

            _userManager.Setup(
                m => m.FindByEmailAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(user);

            _roleManager.Setup(
                m => m.RoleExistsAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(false);
                

            //act
            var result = await _sut.AssignRole(email, role);

            //assert

            result.IsSuccess.ShouldBeFalse();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
            result.Error.ShouldBeOfType(typeof(Error));

            _userManager.Verify(
               m => m.FindByEmailAsync(email),
               Times.Once);
            _roleManager.Verify(
                m => m.RoleExistsAsync(role),
                Times.Once);
        }

        [Fact]
        public async Task LoginUserAsync_ValidData_ReturnAuthToken()
        {
            //Arrange
            var email = "test@test.com";
            var password = "XYZ";
            var user = new ApplicationUser() { Email = email, Id = 12 };
            var time = DateTime.UtcNow;
            var authToken = new AuthToken("Token",time);

            _userManager.Setup(
                m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _userManager.Setup(
                m => m.CheckPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            _tokenService.Setup(
                t => t.CreateTokenAsync(It.IsAny<int>()))
                .ReturnsAsync(authToken);
            //Act
            var result = await _sut.LoginUserAsync(email, password);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();

            _userManager.Verify(
                m => m.FindByEmailAsync(email),
                Times.Once());
                

            _userManager.Verify(
                m => m.CheckPasswordAsync(
                    user,
                    password),
                Times.Once());

            _tokenService.Verify(
                t => t.CreateTokenAsync(user.Id),
                Times.Once());

        }
        [Fact]
        public async Task LoginUserAsync_InvalidPassword_ReturnErrorUnauthorized()
        {
            //Arrange
            var email = "test@test.com";
            var password = "XYZ";
            var user = new ApplicationUser() { Email = email, Id = 12 };

            _userManager.Setup(
                m => m.FindByEmailAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(user);

            _userManager.Setup(
                m => m.CheckPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(false);
            //Act
            var result = await _sut.LoginUserAsync(email, password);

            //Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.Error.ShouldBeOfType(typeof(Error));
            result.Error.Type.ShouldBe(ErrorType.AccessUnAuthorized);

            _userManager.Verify(
                m => m.FindByEmailAsync(email),
                Times.Once());


            _userManager.Verify(
                m => m.CheckPasswordAsync(
                    user,
                    password),
                Times.Once());
        }

        [Fact]
        public async Task RegisterUserAsync_DataIsCorrect_UserCreatedWithUserRole()
        {
            //arrange
            string firstName = "John";
            string lastName = "SMith";
            string password = "XYZ";
            string email = "test@test.com";
            string phone = "123123123";

            var user = new ApplicationUser()
            {
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = password,
                Email = email,
                PhoneNumber = phone,
            };

            var identityResult = IdentityResult.Success;

            _userManager.Setup(
                m => m.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            _userManager.Setup(
                m => m.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()));

            //Act
            var result = await _sut.RegisterUserAsync(
                firstName,
                lastName,
                password,
                email,
                phone);

            //Assert
            result.IsSuccess.ShouldBeTrue();

            _userManager.Verify(
                m => m.CreateAsync(
                    It.Is<ApplicationUser>(u =>
                        u.Email == email &&
                        u.FirstName == firstName &&
                        u.LastName == lastName &&
                        u.PhoneNumber == phone),
                    password),
                Times.Once());

            _userManager.Verify(
                m => m.AddToRoleAsync(
                    It.Is<ApplicationUser>(u => u.Email == email),
                    Roles.User),
                Times.Once());
        }
    }
}
