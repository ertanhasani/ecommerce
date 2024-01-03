using WebApp.Data;

namespace WebApp.Repositories;

public interface IPanelRepository
{
    IEnumerable<ApplicationUser> GetUsers();
    
    IEnumerable<ApplicationUser> GetAllUsers();

    bool HasOpenCart(string userId);

    int HowManyOrder(string userId);

    IEnumerable<Order> GetOrders();

    IEnumerable<AspNetUsers> GetTopUsers();

    IEnumerable<Product> GetTopProducts();

    int GetProductSales(int productId);

    IEnumerable<Order> GetTopCostOrders();

    IEnumerable<Order> GetTopProductOrders();
}