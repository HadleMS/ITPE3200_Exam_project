using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authentication;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Exam.Areas.Identity.Pages.Account;  // For RegisterModel

namespace Exam.Tests.Areas.Identity.Pages.Account
{
    public class RegisterModelTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManager;
        private readonly Mock<IUserStore<IdentityUser>> _userStore;
        private readonly Mock<IUserEmailStore<IdentityUser>> _emailStore;
        private readonly Mock<SignInManager<IdentityUser>> _signInManager;
        private readonly Mock<ILogger<RegisterModel>> _logger;
        private readonly Mock<IEmailSender> _emailSender;

        public RegisterModelTests()
        {
            // Setup basic mocks
            _emailStore = new Mock<IUserEmailStore<IdentityUser>>();
            _userStore = _emailStore.As<IUserStore<IdentityUser>>();
            _logger = new Mock<ILogger<RegisterModel>>();
            _emailSender = new Mock<IEmailSender>();

            // Setup UserManager
            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(o => o.Value).Returns(new IdentityOptions());

            _userManager = new Mock<UserManager<IdentityUser>>(
                _userStore.Object,
                options.Object,
                new PasswordHasher<IdentityUser>(),
                Array.Empty<IUserValidator<IdentityUser>>(),
                Array.Empty<IPasswordValidator<IdentityUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<IdentityUser>>>().Object
            );

            // Setup SignInManager
            _signInManager = new Mock<SignInManager<IdentityUser>>(
                _userManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                options.Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<IdentityUser>>().Object
            );

            SetupMocks();
        }

        private void SetupMocks()
        {
            // UserManager setups
            _userManager.Setup(x => x.SupportsUserEmail).Returns(true);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.GetUserIdAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync("testUserId");
            _userManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync("testToken");

            // EmailStore setups
            _emailStore.Setup(x => x.SetEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), default))
                .Returns(Task.CompletedTask);
            _emailStore.Setup(x => x.SetUserNameAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), default))
                .Returns(Task.CompletedTask);

            // UserStore setup
            _userStore.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), default))
                .ReturnsAsync(IdentityResult.Success);

            // SignInManager setup
            _signInManager.Setup(x => x.SignInAsync(It.IsAny<IdentityUser>(), It.IsAny<bool>(), null))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task OnPostAsync_InvalidModel_ReturnsPageResult()
        {
            // Arrange
            var registerModel = new RegisterModel(
                _userManager.Object,
                _userStore.Object,
                _signInManager.Object,
                _logger.Object,
                _emailSender.Object)
            {
                PageContext = new PageContext(new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new PageActionDescriptor(),
                    new ModelStateDictionary()
                )),
                TempData = new TempDataDictionary(
                    new DefaultHttpContext(),
                    Mock.Of<ITempDataProvider>()
                )
            };

            // Setup URL helper
            registerModel.Url = new Mock<IUrlHelper>().Object;
            registerModel.ModelState.AddModelError("", "Test error");

            // Act
            var result = await registerModel.OnPostAsync(null);

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}