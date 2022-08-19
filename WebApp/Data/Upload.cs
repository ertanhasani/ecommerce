using System.ComponentModel.DataAnnotations;

namespace WebApp.Data;

public class Upload
{
    public int Id { get; set; }

    public string Path { get; set; }

    [Display(Name = "نام فایل")] public string FileName { get; set; }

    public int? ProductId { get; set; }

    public int? CategoryId { get; set; }

    public bool? IsThumbnail { get; set; }

    public bool IsView { get; set; }

    [Display(Name = "عنوان")] public string Title { get; set; }

    [Display(Name = "توضیحات")] public string Description { get; set; }

    [Display(Name = "توضیحات کوتاه")] public string ShortDescription { get; set; }

    [Display(Name = "نام صفحه")] public string PageName { get; set; }

    [Display(Name = "نام بخش")] public string SectionName { get; set; }

    [Display(Name = "لینک")] public string Link { get; set; }

    [Display(Name = "استایل")] public string Style { get; set; }

    [Display(Name = "اولویت")] public int Order { get; set; }

    public string CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public string LasUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedOnDate { get; set; }

    public bool? IsDeleted { get; set; }

    public Category Category { get; set; }

    public Product Product { get; set; }
}