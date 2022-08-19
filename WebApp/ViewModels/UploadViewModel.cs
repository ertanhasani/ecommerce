using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class UploadViewModel
{
    public int Id { get; set; }

    [Display(Name = "مسیر")] public string Path { get; set; }

    [Display(Name = "نام فایل")] public string FileName { get; set; }
}