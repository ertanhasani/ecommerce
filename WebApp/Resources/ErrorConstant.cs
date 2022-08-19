namespace WebApp.Resources;

public static class ErrorConstant
{
    public const string EmailIsNotConfirm = "ایمیل مورد نظر معتبر نمی باشد";

    public const string EmailIsNotCorrect = "لطفا ایمیل را به درستی وارد کنید";

    public const string EmailIsDuplicate = "لطفا ایمیل دیگری برای خود انتخاب کنید";

    public const string FullNameIsNull = "لطفا نام و نام خانوادگی را وارد کنید";

    public const string UserIsLocked = "کاربر مورد نظر مسدود است";

    public const string DeletePermission = "شما امکان حذف این داده را ندارید";

    public const string DeleteError = "خطا در حذف داده";

    public const string PasswordNotEqualToConfirmPassword = "رمز عبور و تکرار رمز برابر نمی باشد";

    public const string CurrentPasswordIsNotCorrect = "رمز عبور فعلی درست نیست";

    public static class GeneralErrors
    {
        public const string ForgotPassword = "بازنشانی رمز عبور با مشکل مواجه شده است";

        public const string Register = "خطایی در ثبت کاربر رخداد است لطفا با مدیر سامانه تماس بگیرید";

        public const string EmailConfirm = "خطایی در تایید ایمیل کاربر رخداد است لطفا با مدیر سامانه تماس بگیرید";

        public const string Login = "خطا در ورود به سامانه لطفا با مدیر سامانه تماس بگیرید";

        public const string GetCart = "شما دسترسی به این سبد خرید ندارید";

        public const string PasswordChange = "تغییر رمز عبور با مشکل رو به رو شده است";

        public const string UserNotFind = "نام کاربری با این مشخصات موجود نیست";

        public const string ProductNotFind = "کالایی با این مشخصات موجود نیست";

        public const string GetFile = "شما دسترسی به این فایل ندارید";
    }
}