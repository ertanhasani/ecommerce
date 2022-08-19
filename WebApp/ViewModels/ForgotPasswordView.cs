using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

//POST
public class ForgotPasswordView
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
    [EmailAddress(ErrorMessage = "لطفا ایمیل را به درستی وارد کنید")]
    public string Email { get; set; }
}