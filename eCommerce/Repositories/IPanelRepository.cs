using eCommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Repositories
{
    public interface IPanelRepository
    {
        IEnumerable<ApplicationUser> GetUsers();
        bool HasOpenCart(string userId);
        int HowManyOrder(string userId);
        IEnumerable<Order> GetOrders();
        IEnumerable<AspNetUsers> GetTopUsers();
        IEnumerable<Product> GetTopProducts();
        int GetProductSales(int productId);
        IEnumerable<Order> GetTopCostOrders();
        IEnumerable<Order> GetTopProductOrders();
    }
}
