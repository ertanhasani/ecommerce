using System.ComponentModel.DataAnnotations;
using WebApp.Data;

namespace WebApp.ViewModels;

public class ProductListViewModel
{
    public int Id { get; set; }

    [Display(Name = "آدرس")] 
    public string Url { get; set; }

    [Display(Name = "آدرس عکس")] 
    public string ImagePath { get; set; }

    [Display(Name = "عنوان")] 
    public string Title { get; set; }

    [Display(Name = "قیمت")] 
    public decimal Price { get; set; }

    [Display(Name = "قیمت با تخفیف")] 
    public decimal SalePrice { get; set; }

    [Display(Name = "در تخفیف")] 
    public bool IsOnSale { get; set; }

    public IEnumerable<string> ProductCategory { get; set; }

    [Display(Name = "دسته بندی محصول")] 
    public string ProdoctCategories => ProductCategory?.Aggregate("", (current, s) => current + ',' + s).TrimStart(',');
}