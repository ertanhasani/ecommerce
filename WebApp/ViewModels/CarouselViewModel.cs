using System.ComponentModel.DataAnnotations;
using WebApp.Data;

namespace WebApp.ViewModels;

public class CarouselViewModel
{
    public int Id { get; set; }

    [Display(Name = "ردیف")] 
    public string Path { get; set; }

    [Display(Name = "نام فایل")] 
    public string Name { get; set; }

    public int? ProductId { get; set; }

    public bool? IsThumbnail { get; set; }

    public bool IsView { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "توضیحات")] 
    public string Description { get; set; }

    [Display(Name = "توضیحات کوتاه")] 
    public string ShortDescription { get; set; }

    [Display(Name = "نام صفحه")] 
    public string PageName { get; set; }

    [Display(Name = "نام بخش")] 
    public string SectionName { get; set; }

    [Display(Name = "لینک")] 
    public string Link { get; set; }

    [Display(Name = "استایل")] 
    public string Style { get; set; }

    [Display(Name = "اولویت")] 
    public int Order { get; set; }

    public string CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public string LasUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedOnDate { get; set; }

    public bool? IsDeleted { get; set; }

    public Product Product { get; set; }
}