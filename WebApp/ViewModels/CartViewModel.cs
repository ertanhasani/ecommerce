using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class CartViewModel
{
    [Display(Name = "محصولات")] public List<ProductCartViewModel> Products { get; set; }

    [Display(Name = "جمع کل")] public decimal Total { get; set; }

    [Display(Name = "وضعیت مرسوله")] public int StatusId { get; set; }

    [Display(Name = "سفارش")] public ShippingViewModel Shipping { get; set; }

    public int Id { get; set; }
}