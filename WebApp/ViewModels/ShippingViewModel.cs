using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ShippingViewModel
{
    [Display(Name = "نام کامل تحویل گیرنده")]
    [Required(ErrorMessage = "لطفا نام گیرنده را وارد کنید")]
    public string FullName { get; set; }

    [Display(Name = "استان")]
    [Required(ErrorMessage = "لطفا استان را وارد کنید")]
    public string State { get; set; }

    [Display(Name = "شهر")]
    [Required(ErrorMessage = "لطفا شهر را وارد کنید")]
    public string City { get; set; }

    [Required(ErrorMessage = "لطفا آدرس را وارد کنید")]
    [Display(Name = "آدرس")]
    public string Address { get; set; }

    [Display(Name = "کد پستی")]
    [Required(ErrorMessage = "لطفا کد پستی را وارد کنید")]
    public string PostalCode { get; set; }
}