using System.ComponentModel.DataAnnotations;
using WebApp.Extensions;

namespace WebApp.ViewModels;

public class ProfileEditViewModel
{
    public string Id { get; set; }

    [Display(Name = "نام")] public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")] public string LastName { get; set; }

    [Display(Name = "تاریخ تولد")] public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "لطفا تاریخ تولد را وارد کنید")]
    [Display(Name = "تاریخ تولد")]
    public string BirthDateString { get; set; }

    [Display(Name = "تاریخ تولد")] public string ShamsiBirthDate => DateHelper.MiladiToShamsi(BirthDate);

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
    [EmailAddress(ErrorMessage = "لطفا ایمیل را به درستی وارد کنید")]
    public string Email { get; set; }

    [Display(Name = "رمز فعلی")] public string CurrentPassword { get; set; }

    [Display(Name = "رمز جدید")]
    [DataType(DataType.Password, ErrorMessage = "رمز عبور باید شامل اعداد ، حرف بزرگ و حرف کوچک باشد")]
    public string NewPassword { get; set; }

    [Display(Name = "تکرار رمز جدید")]
    [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور باید با رمز عبور برابر باشد")]
    public string ConfirmPassword { get; set; }
}