using eCommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Repositories
{
    public interface ICartRepository
    {
        int GetCartCount(string userId);
        bool HasOpenCart(string userId);
        Order GetCurrentUserCart(string userId);
        bool HasThatProductOnCart(int cartId, int productId);
        OrderDetails GetProductOnCart(int cartId, int productId);
        void AddProductToCart(OrderDetails orderDetails);
        OrderDetails GetProductOnCart(int orderDetailsId);
        void EditProductOnCart(OrderDetails orderDetails);
        void DeleteProductOnCart(OrderDetails orderDetails);
        Order GetCart(int id);
        IEnumerable<Order> GetUserCarts(string userId);
        void CreateCart(Order order);
        void EditCart(Order order);
        void DeleteCart(Order order);
        void AddShipping(Shipping shipping);
        Shipping GetCartShippingAddress(int id);
        IEnumerable<Status> GetCartStatues();
        Status GetCartStatus(int id);
        void SaveChanges();
    }
}
