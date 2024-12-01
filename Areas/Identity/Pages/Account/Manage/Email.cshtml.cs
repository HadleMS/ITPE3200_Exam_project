// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Exam.Areas.Identity.Pages.Account.Manage
{
    // PageModel for managing email changes
    public class EmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public EmailModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        // Input model for handling new email input
        public class InputModel
        {

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "The email address format is invalid.")]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        // Loads the user data for displaying the email and confirmation status
        private async Task LoadAsync(IdentityUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        // Handles GET requests to load the email management page
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        // Handles POST requests for updating the user's email
        public async Task<IActionResult> OnPostUpdateEmailAsync()
        {
            // Retrieve the user based on the logged-in identity
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Validate the model (checks if NewEmail is provided and valid)
            if (!ModelState.IsValid)
            {
                await LoadAsync(user); // Load existing data if validation fails
                return Page();
            }

            // Check if the email is actually being changed
            var currentEmail = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail == currentEmail)
            {
                StatusMessage = "Your new email is the same as your current email.";
                return RedirectToPage(); // Return to the same page
            }

            // Update the email directly
            var setEmailResult = await _userManager.SetEmailAsync(user, Input.NewEmail);
            if (!setEmailResult.Succeeded)
            {
                foreach (var error in setEmailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user); // Reload data in case of errors
                return Page();
            }

            // Update the username if it is set to the email
            var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.NewEmail);
            if (!setUserNameResult.Succeeded)
            {
                foreach (var error in setUserNameResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user); // Reload data in case of errors
                return Page();
            }

            // Re-authenticate the user after the change
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your email has been updated successfully.";
            return RedirectToPage();
        }
    }
}
