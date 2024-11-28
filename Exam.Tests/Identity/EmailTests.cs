using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Exam.Areas.Identity.Pages.Account.Manage;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace MyShop.Tests.Identity
{
    public class EmailTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
        private readonly IdentityUser _testUser;

        public EmailTests()
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

            _mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                _mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<IdentityUser>>().Object
            );

            _testUser = new IdentityUser
            {
                Id = "testId",
                Email = "old@test.com",
                UserName = "old@test.com"
            };
        }

        [Fact]
        public async Task OnPostUpdateEmailAsync_ValidEmail_ReturnsRedirectResult() // Positive
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);
            _mockUserManager.Setup(um => um.GetEmailAsync(_testUser))
                .ReturnsAsync("old@test.com");
            _mockUserManager.Setup(um => um.SetEmailAsync(_testUser, "new@test.com"))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.SetUserNameAsync(_testUser, "new@test.com"))
                .ReturnsAsync(IdentityResult.Success);

            var model = new EmailModel(_mockUserManager.Object, _mockSignInManager.Object)
            {
                Input = new EmailModel.InputModel { NewEmail = "new@test.com" }
            };

            // Act
            var result = await model.OnPostUpdateEmailAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Your email has been updated successfully.", model.StatusMessage);
        }

        [Fact]
        public async Task OnPostUpdateEmailAsync_InvalidEmail_ReturnsPageResult() // Negative
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);
            _mockUserManager.Setup(um => um.SetEmailAsync(_testUser, "invalid"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid email format" }));

            var model = new EmailModel(_mockUserManager.Object, _mockSignInManager.Object)
            {
                Input = new EmailModel.InputModel { NewEmail = "invalid" }
            };
            model.ModelState.AddModelError("NewEmail", "Invalid email format");

            // Act
            var result = await model.OnPostUpdateEmailAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(model.ModelState.IsValid);
        }
    }
}
