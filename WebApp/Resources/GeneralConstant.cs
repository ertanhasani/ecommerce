using WebApp.Data;

namespace WebApp.Resources;

public static class GeneralConstant
{
    public static class Roles
    {
        public const string Customer = "Customer";

        public const string Admin = "Admin";

        public const string All = "Admin, Customer";
    }

    public static class Routes
    {
        public const string ControllerDefault = "[controller]/[action]";

        public const string SlashProductItem = "/ProductItem";

        public const string IdParameter = "?Id=";
    }
}