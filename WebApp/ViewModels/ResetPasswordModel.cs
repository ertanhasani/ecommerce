using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WebApp.ViewModels;

public class ResetPasswordModel
{
    public string Code { get; set; }

    public string UserId { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
    [StringLength(100, ErrorMessage = "مقدار {0} باید بین {1} تا {2} باشد", MinimumLength = 6)]
    [DataType(DataType.Password, ErrorMessage = "رمز عبور باید شامل اعداد ، حرف بزرگ و حرف کوچک باشد")]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "تکرار رمز عبور")]
    [Compare("Password", ErrorMessage = "تکرار رمز عبور باید با رمز عبور برابر باشد")]
    public string ConfirmPassword { get; set; }
}