using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ECommerceContext _context;

    public ProductRepository(ECommerceContext context)
    {
        _context = context;
    }

    public void AddPriceToProduct(Price price)
    {
        _context.Price.Add(price);
    }

    public void AddProduct(Product product)
    {
        _context.Product.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        product.IsDeleted = true;
        EditProduct(product);
    }

    public void EditProduct(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
    }

    public void EditProductPrice(Price price)
    {
        _context.Entry(price).State = EntityState.Modified;
    }

    public Price GetCurrentPrice(int productId)
    {
        return GetPricesOfProduct(productId).FirstOrDefault(p => p.IsSale == false && p.IsActive == true);
    }

    public IEnumerable<Price> GetPricesOfProduct(int productId)
    {
        return _context.Price.Where(p => p.IsActive == true && p.ProductId == productId);
    }

    public Product GetProduct(int productId)
    {
        return GetProducts().FirstOrDefault(p => p.Id == productId);
    }

    public IEnumerable<Product> GetProducts()
    {
        return _context.Product.Include(p => p.ProductCategory).Include(pp => pp.Price).Where(p => p.IsDeleted == false).OrderByDescending(p => p.Id);
    }

    public Price GetSalePrice(int productId)
    {
        return GetPricesOfProduct(productId).FirstOrDefault(p => p.IsSale == true && p.IsActive == true);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}