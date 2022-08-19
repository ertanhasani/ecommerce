using System.ComponentModel.DataAnnotations;
using WebApp.Extensions;

namespace WebApp.ViewModels;

public class OrdersViewModel
{
    public int Id { get; set; }

    [Display(Name = "وضعیت")] public string Status { get; set; }

    [Display(Name = "تاریخ سفارش")] public string OrderedDate { get; set; }

    [Display(Name = "قیمت کل")] public decimal TotalPrice { get; set; }

    [Display(Name = "نام کاربر")] public string UsersFullName { get; set; }

    [Display(Name = "ایمیل")] public string UsersEmail { get; set; }

    [Display(Name = "شناسه کاربر")] public string UserId { get; set; }

    [Display(Name = "تاریخ سفارش")] public string ShamsiOrderedDate => DateHelper.MiladiToShamsi(DateTime.Parse(OrderedDate));
}