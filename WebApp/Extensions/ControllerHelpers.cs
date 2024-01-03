using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Extensions;

public static class ControllerHelpers
{
    public static IEnumerable<string> GetIdentityErrors(IdentityResult result)
    {
        var errors = new List<string>();

        foreach (var error in result.Errors)
        {
            var err = error.Description;

            if (error.Code == "DuplicateUserName")
            {
                errors.Add("قبلا با این نام کاربری ثبت نام شده است.");
            }

            if (err.StartsWith("Passwords must have at least one uppercase"))
            {
                errors.Add("کلمه عبور باید شامل حروف بزرگ باشد.");
            }

            if (err.StartsWith("Passwords must have at least one lowercase"))
            {
                errors.Add("کلمه عبور باید شامل حروف کوچک باشد.");
            }

            if (err.StartsWith("Incorrect password"))
            {
                errors.Add("نام کاربری یا کلمه عبور اشتباه است.");
            }

            if (err.StartsWith("Passwords must be at least 6 characters"))
            {
                errors.Add("کلمه عبور باید حداقل 6 حرف باشد.");
            }

            if (err.StartsWith("Passwords must have at least one digit"))
            {
                errors.Add("کلمه عبور باید شامل حداقل یک عدد باشد.");
            }

            if (err.StartsWith("Invalid token"))
            {
                errors.Add("کد اشتباه است.");
            }
        }

        return errors.ToArray();
    }

    public static void AddErrors(this Controller controller, IdentityResult result)
    {
        AddErrors(controller, GetIdentityErrors(result));
    }

    public static void AddErrors(this Controller controller, IEnumerable<string> errors)
    {
        foreach (var err in errors)
        {
            controller.ModelState.AddModelError(string.Empty, err);
        }
    }

    public static void AddErrors(this Controller controller, List<string> errors)
    {
        foreach (var err in errors)
        {
            controller.ModelState.AddModelError(string.Empty, err);
        }
    }

    public static void AddErrors(this Controller controller, string key, IEnumerable<string> errors)
    {
        foreach (var err in errors)
        {
            controller.ModelState.AddModelError(key, err);
        }
    }

    public static void AddErrors(this Controller controller, string err)
    {
        controller.ModelState.AddModelError(string.Empty, err);
    }

    public static void AddErrors(this Controller controller, string key, string err)
    {
        controller.ModelState.AddModelError(key, err);
    }

    public static string GetControllerName(string className)
    {
        var place = className.LastIndexOf("Controller", StringComparison.Ordinal);

        return place == -1 ? className : className.Remove(place, "Controller".Length);
    }
}