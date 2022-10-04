using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Extensions;
using WebApp.Models;
using WebApp.Resources;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route(GeneralConstant.Routes.ControllerDefault)]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IEmailSender _emailSender;
    private readonly IUserStore<ApplicationUser> _userStore;
    public IList<AuthenticationScheme> ExternalLogins { get; set; }
    [TempData] public string ErrorMessage { get; set; }

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserStore<ApplicationUser> userStore,
        ILogger<AccountController> logger,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userStore = userStore;
        _logger = logger;
        _emailSender = emailSender;
    }

    #region LogOut

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation(LogConstant.LoggedOut);
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region Register

    [HttpGet]
    public Task<IActionResult> Register(string returnUrl)
    {
        return Task.FromResult<IActionResult>(View(new RegisterViewModel
        {
            ReturnUrl = returnUrl
        }));
    }

    [HttpPost]
    public async Task<IActionResult> RegisterForm(RegisterViewModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) != null || !EmailHandler.IsValidEmail(model.Email))
        {
            this.AddErrors(ErrorConstant.EmailIsDuplicate);
        }

        if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
        {
            this.AddErrors(ErrorConstant.FullNameIsNull);
        }

        model.BirthDate = DateHelper.ShamsiToMiladi(model.BirthDateString);

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                CreatedOnDate = DateTime.Now,
                IsDeleted = false
            };

            await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation(LogConstant.NewUserWithPass);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                await _userManager.AddToRoleAsync(user, GeneralConstant.Roles.Customer);

                if (model.ReturnUrl == null)
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(Url.GetLocalUrl(model.ReturnUrl));
            }

            this.AddErrors(result.Errors.Select(x => x.Description).ToList());
        }

        return View("Register", model);
    }

    [HttpGet]
    public async Task<ActionResult> RegisterForm(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return View("Error", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.EmailConfirm});
        }

        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            return View("ConfirmEmail");
        }

        return View("Error", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.EmailConfirm});
    }

    #endregion

    #region Login

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation(LogConstant.LoggedIn);

                if (model.ReturnUrl == null)
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(Url.GetLocalUrl(model.ReturnUrl));
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning(LogConstant.UserIsLocked);

                this.AddErrors(ErrorConstant.UserIsLocked);
                return View(model);
            }
        }

        this.AddErrors(ErrorConstant.PasswordIsNotCorrect);
        return View(model);
    }

    #endregion

    #region ForgotPassword

    [HttpGet]
    public Task<IActionResult> ForgotPassword(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return Task.FromResult<IActionResult>(View(new ForgotPasswordView()));
        }

        return Task.FromResult<IActionResult>(View("ResetPassword", new ResetPasswordModel {Code = code, UserId = userId}));
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordView model)
    {
        if (!EmailHandler.IsValidEmail(model.Email))
        {
            this.AddErrors(ErrorConstant.EmailIsNotCorrect);
        }

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                this.AddErrors(ErrorConstant.EmailIsNotConfirm);
                return View(model);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
            await _emailSender.SendResetPasswordAsync(model.Email, callbackUrl);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        return View("Error", new ErrorViewModel
        {
            Message = ErrorConstant.GeneralErrors.ForgotPassword
        });
    }

    [HttpGet]
    public Task<IActionResult> ForgotPasswordConfirmation()
    {
        return Task.FromResult<IActionResult>(View());
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        if (model.UserId == null || model.Code == null)
        {
            return View("Error", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.ForgotPassword});
        }

        var user = await _userManager.FindByIdAsync(model.UserId);

        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

        if (result.Succeeded)
        {
            return View("ResetPasswordConfirmation");
        }

        return View("Error", new ErrorViewModel {Message = ErrorConstant.GeneralErrors.EmailConfirm});
    }
}