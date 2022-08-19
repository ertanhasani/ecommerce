using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ProductsViewModel
{
    public int Id { get; set; }

    [Display(Name = "نام")] public string Name { get; set; }

    [Display(Name = "توضیحات")] public string Description { get; set; }

    [Display(Name = "قیمت")] public decimal? Price { get; set; }

    [Display(Name = "قیمت با تخفیف")] public decimal? SalePrice { get; set; }

    [Display(Name = "در تخفیف")] public bool IsOnSale { get; set; }

    [Display(Name = "دارای فایل دیجیتال")] public bool IsDigit { get; set; }

    [Display(Name = "نام عکس")] public string ThumbnailString { get; set; }

    [Display(Name = "تعداد")] public int Quantity { get; set; }

    [Display(Name = "دسته بندی")] public int[] Categories { get; set; }

    [Display(Name = "آدرس عکس")] public string ThumbnailUrl { get; set; }
}