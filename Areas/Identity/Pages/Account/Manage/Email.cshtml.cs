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

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$", ErrorMessage = "The email address format is invalid.")]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

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

        public async Task<IActionResult> OnPostUpdateEmailAsync()
        {
            // Hent brukeren basert på pålogget identitet
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Valider modellen (sjekker at NewEmail er fylt inn og gyldig)
            if (!ModelState.IsValid)
            {
                await LoadAsync(user); // Laster eksisterende data hvis noe feiler
                return Page();
            }

            // Sjekk om e-posten faktisk skal endres
            var currentEmail = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail == currentEmail)
            {
                StatusMessage = "Your new email is the same as your current email.";
                return RedirectToPage(); // Tilbake til samme side
            }

            // Oppdater e-posten direkte
            var setEmailResult = await _userManager.SetEmailAsync(user, Input.NewEmail);
            if (!setEmailResult.Succeeded)
            {
                foreach (var error in setEmailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user); // Last inn data igjen i tilfelle feil
                return Page();
            }

            // Oppdater brukernavn hvis det er satt til e-posten
            var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.NewEmail);
            if (!setUserNameResult.Succeeded)
            {
                foreach (var error in setUserNameResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user); // Last inn data igjen i tilfelle feil
                return Page();
            }

            // Re-autentiser brukeren etter endringen
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your email has been updated successfully.";
            return RedirectToPage();
        }
    }
}
