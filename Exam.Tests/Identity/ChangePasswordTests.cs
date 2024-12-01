using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Moq;
using Xunit;
using Exam.Areas.Identity.Pages.Account.Manage;
using System.Security.Claims;

namespace MyShop.Tests.Identity
{
    public class ChangePasswordTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
        private readonly Mock<ILogger<ChangePasswordModel>> _mockLogger;

        public ChangePasswordTests()
        {
            var userStore = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<IdentityUser>>().Object,
                new IUserValidator<IdentityUser>[0],
                new IPasswordValidator<IdentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<IdentityUser>>>().Object
            );

            _mockLogger = new Mock<ILogger<ChangePasswordModel>>();

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            _mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                _mockUserManager.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<IdentityUser>>().Object
            );
        }

        [Fact]
        public async Task OnPostAsync_ValidPassword_ReturnsRedirectResult()
        {
            // Arrange
            var user = new IdentityUser { Id = "testId", UserName = "test@test.com" };
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }));

            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(um => um.ChangePasswordAsync(user, "OldPass123!", "NewPass123!"))
                .ReturnsAsync(IdentityResult.Success);

            var model = new ChangePasswordModel(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockLogger.Object)
            {
                Input = new ChangePasswordModel.InputModel
                {
                    OldPassword = "OldPass123!",
                    NewPassword = "NewPass123!",
                    ConfirmPassword = "NewPass123!"
                }
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Your password has been changed.", model.StatusMessage);
        }

        [Fact]
        public async Task OnPostAsync_InvalidOldPassword_ReturnsPageResult()
        {
            // Arrange
            var user = new IdentityUser { Id = "testId", UserName = "test@test.com" };

            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(um => um.ChangePasswordAsync(user, "WrongPass123!", "NewPass123!"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Incorrect password." }));

            var model = new ChangePasswordModel(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockLogger.Object)
            {
                Input = new ChangePasswordModel.InputModel
                {
                    OldPassword = "WrongPass123!",
                    NewPassword = "NewPass123!",
                    ConfirmPassword = "NewPass123!"
                }
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(model.ModelState.IsValid);
        }
    }
}