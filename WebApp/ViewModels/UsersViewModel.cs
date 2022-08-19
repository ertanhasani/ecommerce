using System.ComponentModel.DataAnnotations;
using WebApp.Extensions;

namespace WebApp.ViewModels;

public class UsersViewModel
{
    public string Id { get; set; }

    [Display(Name = "نام")] public string FullName { get; set; }

    [Display(Name = "نام کاربری")] public string UserName { get; set; }

    [Display(Name = "ایمیل")] public string Email { get; set; }

    [Display(Name = "تاریخ تولد")] public string BirthDate { get; set; }

    [Display(Name = "تاریخ تولد")] public string ShamsiBirthDate => DateHelper.MiladiToShamsi(DateTime.Parse(BirthDate));

    [Display(Name = "سفارشات")] public int Orders { get; set; }

    [Display(Name = "سبد خرید")] public bool CurrentCart { get; set; }
}