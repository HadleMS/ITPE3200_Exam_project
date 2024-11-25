// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

       public class InputModel
        {
            [Phone(ErrorMessage = "Please enter a valid phone number.")]
            [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Please enter a valid phone number (e.g., +47 123 45 678 or 123 45 678).")]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [StringLength(100, ErrorMessage = "The full name must be at least 2 and at most 100 characters.", MinimumLength = 2)]
            [Display(Name = "Full name")]
            public string FullName { get; set; }

            [StringLength(200, ErrorMessage = "The address must be at least 5 and at most 200 characters.", MinimumLength = 5)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "DOB")]
            public DateTime? DOB { get; set; }
        }


        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            // Hent FullName, Address, and DOB fra brukerens claims
            var claims = await _userManager.GetClaimsAsync(user);
            var fullName = claims.FirstOrDefault(c => c.Type == "FullName")?.Value ?? "";
            var address = claims.FirstOrDefault(c => c.Type == "Address")?.Value ?? "";
            var dobClaim = claims.FirstOrDefault(c => c.Type == "DOB")?.Value;
            var dob = dobClaim != null ? DateTime.Parse(dobClaim) : (DateTime?)null;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = fullName,
                Address = address,
                DOB = dob
            };
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

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Oppdater Full Name, Address og DOB som claims
            var claims = await _userManager.GetClaimsAsync(user);

            // Full Name
            var fullNameClaim = claims.FirstOrDefault(c => c.Type == "FullName");
            if (fullNameClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, fullNameClaim);
            }
            await _userManager.AddClaimAsync(user, new Claim("FullName", Input.FullName));

            // Address
            var addressClaim = claims.FirstOrDefault(c => c.Type == "Address");
            if (addressClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, addressClaim);
            }
            await _userManager.AddClaimAsync(user, new Claim("Address", Input.Address));

            // DOB
            var dobClaim = claims.FirstOrDefault(c => c.Type == "DOB");
            if (dobClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, dobClaim);
            }
            if (Input.DOB.HasValue)
            {
                await _userManager.AddClaimAsync(user, new Claim("DOB", Input.DOB.Value.ToString("yyyy-MM-dd")));
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
