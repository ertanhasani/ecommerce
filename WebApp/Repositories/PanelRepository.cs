using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Repositories;

public class PanelRepository : IPanelRepository
{
    private eCommerceContext _context;
    private UserManager<ApplicationUser> _userManager;

    public PanelRepository(eCommerceContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IEnumerable<ApplicationUser> GetUsers()
    {
        return _userManager.GetUsersInRoleAsync("Admin").Result;
    }

    public IEnumerable<ApplicationUser> GetAllUsers()
    {
        return _context.AspNetUsers.Select(x => new ApplicationUser
        {
            Id = x.Id,
            UserName = x.UserName,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            BirthDate = (DateTime) x.BirthDate,
            IsDeleted = (bool) x.IsDeleted
        });
    }
    
    public bool HasOpenCart(string userId)
    {
        return _context.Order.Any(p => p.UserId == userId && p.Payed == false && p.IsDeleted == false && p.OrderDetails.Count > 0);
    }

    public int HowManyOrder(string userId)
    {
        return _context.Order.Count(p => p.UserId == userId && p.IsDeleted == false && p.Payed == true);
    }

    public IEnumerable<Order> GetOrders()
    {
        return _context.Order.Include(p => p.Status).Include(p => p.User).Where(p => p.Payed == true && p.IsDeleted == false && p.OrderDetails.Count > 0);
    }

    public IEnumerable<AspNetUsers> GetTopUsers()
    {
        return _context.AspNetUsers.Include(p => p.Order).ThenInclude(o => o.OrderDetails).Include(p => p.AspNetUserRoles).ThenInclude(o => o.Role).Where(p => p.Order.Where(o => o.IsDeleted == false && o.Payed == true && o.OrderDetails.Count > 0).Count() > 0 && p.AspNetUserRoles.Any(o => o.Role.Name.ToLower().Equals("admin"))).OrderByDescending(p => p.Order.Count());
    }

    public IEnumerable<Product> GetTopProducts()
    {
        return _context.Product.Include(p => p.OrderDetails).ThenInclude(p => p.Order).Where(p => p.OrderDetails.Any(o => o.Order.Payed == true && o.Order.IsDeleted == false)).OrderByDescending(p => p.OrderDetails.Select(o => o.Quantity).Sum());
    }

    public int GetProductSales(int productId)
    {
        return _context.OrderDetails.Select(p => (int) p.Quantity).Sum();
    }

    public IEnumerable<Order> GetTopCostOrders()
    {
        return GetOrders().OrderByDescending(p => p.TotalPrice);
    }

    public IEnumerable<Order> GetTopProductOrders()
    {
        return GetOrders().OrderByDescending(p => p.OrderDetails.Sum(o => o.Quantity));
    }
}