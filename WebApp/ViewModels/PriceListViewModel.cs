using System.ComponentModel.DataAnnotations;
using WebApp.Data;

namespace WebApp.ViewModels;

public class PriceListViewModel
{
    [Display(Name = "شماره محصول")] public int ProductId { get; set; }

    [Display(Name = "نام محصول")] public string Name { get; set; }

    [Display(Name = "در تخفیف")] public bool IsOnSale { get; set; }

    [Display(Name = "قیمت با تخفیف")] public decimal OffPrice { get; set; }

    [Display(Name = "فایل دیجیتال")] public bool IsDigit { get; set; }

    [Display(Name = "قیمت محصول")] public Price Price { get; set; }
}