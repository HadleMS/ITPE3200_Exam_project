using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Principal;
using Exam.Areas.Identity.Pages.Account.Manage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Exam.Tests.Areas.Identity.Pages.Account.Manage
{
    public class IndexModelTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManager;
        private readonly Mock<SignInManager<IdentityUser>> _signInManager;
        private readonly IdentityUser _testUser;

        public IndexModelTests()
        {
            // Setup test user
            _testUser = new IdentityUser
            {
                Id = "testUserId",
                UserName = "testuser@example.com",
                Email = "testuser@example.com"
            };

            // Setup UserManager mock
            var userStore = new Mock<IUserStore<IdentityUser>>();
            _userManager = new Mock<UserManager<IdentityUser>>(
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

            // Setup SignInManager mock
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _signInManager = new Mock<SignInManager<IdentityUser>>(
                _userManager.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<IdentityUser>>().Object
            );

            SetupBasicMocks();
        }

        private void SetupBasicMocks()
        {
            // Setup basic user manager responses
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);
            _userManager.Setup(x => x.GetUserIdAsync(_testUser))
                .ReturnsAsync(_testUser.Id);
            _userManager.Setup(x => x.GetUserNameAsync(_testUser))
                .ReturnsAsync(_testUser.UserName);
            _userManager.Setup(x => x.GetPhoneNumberAsync(_testUser))
                .ReturnsAsync("123456789");

            // Setup claims
            var claims = new List<Claim>
            {
                new Claim("FullName", "Test User"),
                new Claim("Address", "Test Address"),
                new Claim("DOB", DateTime.Now.AddYears(-20).ToString("yyyy-MM-dd"))
            };
            _userManager.Setup(x => x.GetClaimsAsync(_testUser))
                .ReturnsAsync(claims);

            // Setup successful results
            _userManager.Setup(x => x.SetPhoneNumberAsync(_testUser, It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.RemoveClaimAsync(_testUser, It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddClaimAsync(_testUser, It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);
        }

        [Fact]
        public async Task OnGetAsync_ValidUser_ReturnsPageResult()
        {
            // Arrange
            var indexModel = new IndexModel(_userManager.Object, _signInManager.Object);
            SetupUserContext(indexModel);

            // Act
            var result = await indexModel.OnGetAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.NotNull(indexModel.Input);
            Assert.Equal("123456789", indexModel.Input.PhoneNumber);
            Assert.Equal("Test User", indexModel.Input.FullName);
            Assert.Equal("Test Address", indexModel.Input.Address);
        }

        [Fact]
        public async Task OnGetAsync_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((IdentityUser?)null);

            var indexModel = new IndexModel(_userManager.Object, _signInManager.Object);
            SetupUserContext(indexModel);

            // Act
            var result = await indexModel.OnGetAsync();

            // Assert
            if (result is NotFoundObjectResult notFoundResult && notFoundResult.Value != null)
            {
                Assert.Contains("Unable to load user", notFoundResult.Value.ToString());
            }
        }

        [Fact]
        public async Task OnPostAsync_ValidModel_UpdatesUserAndRedirects()
        {
            // Arrange
            var indexModel = new IndexModel(_userManager.Object, _signInManager.Object)
            {
                Input = new IndexModel.InputModel
                {
                    PhoneNumber = "987654321",
                    FullName = "Updated Name",
                    Address = "Updated Address",
                    DOB = DateTime.Now.AddYears(-25)
                }
            };
            SetupUserContext(indexModel);

            // Act
            var result = await indexModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Your profile has been updated", indexModel.StatusMessage);

            // Verify calls
            _userManager.Verify(x => x.SetPhoneNumberAsync(_testUser, "987654321"), Times.Once);
            _userManager.Verify(x => x.AddClaimAsync(_testUser, It.Is<Claim>(c => c.Type == "FullName")), Times.Once);
            _userManager.Verify(x => x.AddClaimAsync(_testUser, It.Is<Claim>(c => c.Type == "Address")), Times.Once);
            _userManager.Verify(x => x.AddClaimAsync(_testUser, It.Is<Claim>(c => c.Type == "DOB")), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_InvalidModel_ReturnsPageWithErrors()
        {
            // Arrange
            var indexModel = new IndexModel(_userManager.Object, _signInManager.Object)
            {
                Input = new IndexModel.InputModel
                {
                    PhoneNumber = "invalid-phone", // Invalid phone number
                    FullName = "U", // Too short
                    Address = "A" // Too short
                }
            };
            SetupUserContext(indexModel);
            indexModel.ModelState.AddModelError("", "Test error");

            // Act
            var result = await indexModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(indexModel.ModelState.IsValid);
        }

        [Fact]
        public async Task OnPostAsync_PhoneUpdateFails_ReturnsErrorMessage()
        {
            // Arrange
            _userManager.Setup(x => x.SetPhoneNumberAsync(_testUser, It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Phone update failed" }));

            var indexModel = new IndexModel(_userManager.Object, _signInManager.Object)
            {
                Input = new IndexModel.InputModel
                {
                    PhoneNumber = "987654321",
                    FullName = "Test User",
                    Address = "Test Address"
                }
            };
            SetupUserContext(indexModel);

            // Act
            var result = await indexModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Unexpected error when trying to set phone number.", indexModel.StatusMessage);
        }

        private void SetupUserContext(PageModel model)
        {
            var context = new DefaultHttpContext();
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            context.User = mockPrincipal.Object;
            model.PageContext = new PageContext
            {
                HttpContext = context
            };
            model.TempData = new TempDataDictionary(
                context,
                Mock.Of<ITempDataProvider>()
            );
        }
    }
}
