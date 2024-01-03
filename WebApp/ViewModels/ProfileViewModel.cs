using System.ComponentModel.DataAnnotations;
using WebApp.Extensions;

namespace WebApp.ViewModels;

public class ProfileViewModel
{
    public string Id { get; set; }

    [Display(Name = "نام")] 
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")] 
    public string LastName { get; set; }

    [Display(Name = "تاریخ تولد")] 
    public DateTime BirthDate { get; set; }

    [Display(Name = "ایمیل")] 
    public string Email { get; set; }

    [Display(Name = "رمز فعلی")] 
    public string CurrentPassword { get; set; }

    [StringLength(100, ErrorMessage = "مقدار {0} باید بین {1} تا {2} باشد", MinimumLength = 6)]
    [DataType(DataType.Password, ErrorMessage = "رمز عبور باید شامل اعداد ، حرف بزرگ و حرف کوچک باشد")]
    [Display(Name = "رمز عبور")]
    public string NewPassword { get; set; }

    [Display(Name = "تکرار رمز عبور")]
    [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور باید با رمز عبور برابر باشد")]
    public string ConfirmPassword { get; set; }

    public List<OrdersViewModel> Orders { get; set; }
}