using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class LoginViewModel
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
    [EmailAddress(ErrorMessage = "لطفا ایمیل را به درستی وارد کنید")]
    public string Email { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
    [DataType(DataType.Password, ErrorMessage = "رمز عبور باید شامل اعداد ، حرف بزرگ و حرف کوچک باشد")]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [Display(Name = "به خاطر بسپار")] public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }
}