using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages.Profile
{
    [Authorize]
    public class EditModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public ProfileViewModel Profile { get; set; }

        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Index");

            var userProfile = await _userManager.FindByIdAsync(userId);
            if ((User.IsInRole("Admin") || (!User.IsInRole("Admin") && _userManager.GetUserId(User).Equals(userId))) && userProfile != null)
            {
                Profile = new ProfileViewModel()
                {
                    Id = userId,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    BirthDate = userProfile.BirthDate,
                    Email = userProfile.Email
                };

                return Page();
            }
            else
                return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPost()
        {
            if(!String.IsNullOrEmpty(Profile.ConfirmPassword))
            {
                var user = await _userManager.FindByIdAsync(Profile.Id);
                user.FirstName = Profile.FirstName;
                user.LastName = Profile.LastName;
                user.BirthDate = Profile.BirthDate;
                await _userManager.UpdateAsync(user);

                if (!Profile.NewPassword.Equals(Profile.ConfirmPassword))
                {
                    ViewData["Error"] = "Your new password does not match your confirm password";
                    return Page();
                }

                var currentPassword = await _userManager.CheckPasswordAsync(user, Profile.CurrentPassword);

                if (currentPassword)
                {
                    var result = await _userManager.ChangePasswordAsync(user, Profile.CurrentPassword, Profile.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToPage("/Profile/Index", new { userId = Profile.Id, toastr = "Profile updated successfully." });
                    }
                }
                else
                {
                    ViewData["Error"] = "Current password is invalid.";
                    return Page();
                }
            }
            else
            {
                var user = await _userManager.FindByIdAsync(Profile.Id);
                user.FirstName = Profile.FirstName;
                user.LastName = Profile.LastName;
                user.BirthDate = Profile.BirthDate;
                await _userManager.UpdateAsync(user);
                return RedirectToPage("/Profile/Index", new { userId = Profile.Id, toastr = "Profile updated successfully." });
            }

            return RedirectToPage("/Profile/Index", new { userId = Profile.Id, toastr = "Something happened, please try again later.", error = true });
        }
    }
}