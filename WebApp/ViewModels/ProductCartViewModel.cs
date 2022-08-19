using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ProductCartViewModel
{
    public int Id { get; set; }

    [Display(Name = "نام محصول")] public string ProductName { get; set; }

    [Display(Name = "تعداد")] public int Quantity { get; set; }

    [Display(Name = "قیمت")] public decimal Price { get; set; }

    [Display(Name = "جمع کل")] public decimal Total { get; set; }

    [Display(Name = "مسیر عکس")] public string ImagePath { get; set; }

    [Display(Name = "فروخت شده")] public int Sold { get; set; }

    [Display(Name = "فایل دیجیتال")] public bool IsDigit { get; set; }
}