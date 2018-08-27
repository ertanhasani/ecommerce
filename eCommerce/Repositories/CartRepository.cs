using eCommerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Repositories
{
    public class CartRepository : ICartRepository
    {
        private eCommerceContext _context;

        public CartRepository(eCommerceContext context)
        {
            _context = context;
        }

        public int GetCartCount(string userId)
        {
            return _context.OrderDetails.Where(p => p.Order.UserId == userId && p.IsDeleted == false && p.Order.IsDeleted == false && p.Order.Payed == false).Count();
        }

        public Order GetCurrentUserCart(string userId)
        {
            return _context.Order.Include(p => p.OrderDetails).ThenInclude(o => o.Product).FirstOrDefault(p => p.UserId == userId && p.Payed == false && p.IsDeleted == false);
        }

        public OrderDetails GetProductOnCart(int cartId, int productId)
        {
            return _context.OrderDetails.Include(p => p.Product).FirstOrDefault(p => p.OrderId == cartId && p.ProductId == productId && p.IsDeleted == false);
        }

        public bool HasOpenCart(string userId)
        {
            return _context.Order.Any(p => p.UserId == userId && p.Payed == false && p.IsDeleted == false);
        }

        public bool HasThatProductOnCart(int cartId, int productId)
        {
            return _context.OrderDetails.Any(p => p.OrderId == cartId && p.ProductId == productId && p.IsDeleted == false);
        }

        public void AddProductToCart(OrderDetails orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
        }

        public OrderDetails GetProductOnCart(int orderDetailsId)
        {
            return _context.OrderDetails.Include(p => p.Product).FirstOrDefault(p => p.Id == orderDetailsId && p.IsDeleted == false);
        }

        public void EditProductOnCart(OrderDetails orderDetails)
        {
            _context.Entry(orderDetails).State = EntityState.Modified;
        }

        public void DeleteProductOnCart(OrderDetails orderDetails)
        {
            _context.OrderDetails.Remove(orderDetails);
        }

        public void CreateCart(Order order)
        {
            _context.Order.Add(order);
        }

        public void EditCart(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }

        public void DeleteCart(Order order)
        {
            _context.Order.Remove(order);
        }

        public void AddShipping(Shipping shipping)
        {
            _context.Shipping.Add(shipping);
        }

        public Order GetCart(int id)
        {
            //return _context.OrderDetails.Include(p => p.Product).Include(p => p.Order).FirstOrDefault(p => p.OrderId == id).Order;
            return _context.Order.Include(p => p.OrderDetails).ThenInclude(p => p.Product).FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Order> GetUserCarts(string userId)
        {
            return _context.Order.Where(p => p.UserId == userId && p.IsDeleted == false && p.Payed == true).Include(p => p.Status);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Status> GetCartStatues()
        {
            return _context.Status;
        }

        public Status GetCartStatus(int id)
        {
            return _context.Order.Include(p => p.Status).FirstOrDefault(p => p.Id == id).Status;
        }

        public Shipping GetCartShippingAddress(int id)
        {
            return _context.Order.Include(p => p.Shipping).FirstOrDefault(p => p.Id == id).Shipping;
        }
    }
}
