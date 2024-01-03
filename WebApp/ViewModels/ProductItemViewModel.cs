using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ProductItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "نام")] 
    public string Name { get; set; }

    [Display(Name = "توضیحات")] 
    public string Description { get; set; }

    [Display(Name = "قیمت")] 
    public decimal Price { get; set; }

    [Display(Name = "قیمت فروش")] 
    public decimal SalePrice { get; set; }

    [Display(Name = "تعداد")] 
    public int Quantity { get; set; }

    [Display(Name = "تعداد دست دوم")] 
    public int Stock { get; set; }

    [Display(Name = "در تخفیف")] 
    public bool IsOnSale { get; set; }

    [Display(Name = "فایل دیجیتال")] 
    public bool IsDigit { get; set; }

    public List<UploadViewModel> Uploads { get; set; }

    public List<ProductListViewModel> Products { get; set; }

    [Display(Name = "دسته بندی")] 
    public string Categories { get; set; }
}