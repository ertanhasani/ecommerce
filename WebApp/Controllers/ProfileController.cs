using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Extensions;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Resources;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(Roles = GeneralConstant.Roles.All)]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICartRepository _cartRepository;

    public ProfileController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
    {
        _userManager = userManager;
        _cartRepository = cartRepository;
    }

    #region Info

    public async Task<IActionResult> Index(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.UserNotFind});

        var userProfile = await _userManager.FindByIdAsync(userId);

        if (userProfile == null)
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.UserNotFind});
        }

        var userCurrent = await _userManager.GetUserAsync(HttpContext.User);

        if (userCurrent.Id != userProfile.Id && !User.IsInRole(GeneralConstant.Roles.Admin))
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.UserNotFind});
        }

        return View(new ProfileViewModel
        {
            Id = userId,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            BirthDate = userProfile.BirthDate,
            Email = userProfile.Email,
            Orders = _cartRepository.GetUserCarts(userId).OrderByDescending(p => p.Id).Select(item => new OrdersViewModel()
            {
                Id = item.Id,
                OrderedDate = item.LastUpdatedOnDate?.ToString("yyyy.MM.dd"),
                Status = item.Status.Name,
                TotalPrice = item.TotalPrice ?? 0
            }).ToList()
        });
    }

    #endregion

    #region Edit

    [HttpGet]
    public async Task<IActionResult> Edit(ProfileViewModel model)
    {
        if (string.IsNullOrEmpty(model.Id))
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = "نام کاربری با این مشخصات موجود نیست"});


        var userProfile = await _userManager.FindByIdAsync(model.Id);

        if (userProfile == null)
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = "نام کاربری با این مشخصات موجود نیست"});
        }

        var userCurrent = await _userManager.GetUserAsync(HttpContext.User);

        if (userCurrent.Id != userProfile.Id && !User.IsInRole(GeneralConstant.Roles.Admin))
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = "نام کاربری با این مشخصات موجود نیست"});
        }

        var profile = new ProfileEditViewModel()
        {
            Id = model.Id,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            BirthDate = Convert.ToDateTime(DateHelper.MiladiToShamsi(userProfile.BirthDate)).Date,
            Email = userProfile.Email
        };

        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProfileEditViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);

        model.BirthDate = DateHelper.ShamsiToMiladi(model.BirthDateString);

        if (user == null)
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.UserNotFind});
        }

        var userCurrent = await _userManager.GetUserAsync(HttpContext.User);

        if (userCurrent.Id != user.Id && !User.IsInRole(GeneralConstant.Roles.Admin))
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.UserNotFind});
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.BirthDate = model.BirthDate;

        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                this.AddErrors(ErrorConstant.PasswordNotEqualToConfirmPassword);
                return View(model);
            }

            var currentPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);

            if (currentPassword)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    return RedirectToAction("Error", "Home", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.PasswordChange});
                }
            }
            else
            {
                this.AddErrors(ErrorConstant.CurrentPasswordIsNotCorrect);
                return View(model);
            }
        }

        await _userManager.UpdateAsync(user);

        return RedirectToAction("Index", new {userId = model.Id});
    }

    #endregion
}