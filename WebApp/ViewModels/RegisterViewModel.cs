using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels;

public class RegisterViewModel : IdentityUser
{
    [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
    [EmailAddress(ErrorMessage = "لطفا ایمیل را به صورت صحیح وارد کنید")]
    [Display(Name = "ایمیل")]
    public override string Email { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
    [StringLength(100, ErrorMessage = "مقدار {0} باید بین {1} تا {2} باشد", MinimumLength = 6)]
    [DataType(DataType.Password, ErrorMessage = "رمز عبور باید شامل اعداد ، حرف بزرگ و حرف کوچک باشد")]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "تکرار رمز عبور")]
    [Compare("Password", ErrorMessage = "تکرار رمز عبور باید با رمز عبور برابر باشد")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "لطفا نام را وارد کنید")]
    [Display(Name = "نام")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; }

    [Display(Name = "تاریخ تولد")] public DateTime BirthDate { get; set; }
    [Required(ErrorMessage = "لطفا تاریخ تولد را وارد کنید")]
    [Display(Name = "تاریخ تولد")]
    public string BirthDateString { get; set; }

    public string ReturnUrl { get; set; }
}