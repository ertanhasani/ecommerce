using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ECommerceContext _context;

    public CategoryRepository(ECommerceContext context)
    {
        _context = context;
    }

    public void AddProductCategories(List<ProductCategory> categories)
    {
        _context.ProductCategory.AddRange(categories);
    }

    public void AddProductCategory(ProductCategory category)
    {
        _context.ProductCategory.Add(category);
    }

    public void DeleteProductCategories(List<ProductCategory> categories)
    {
        _context.ProductCategory.RemoveRange(categories);
    }

    public void UpdateCategories(List<Category> categories)
    {
        _context.Category.UpdateRange(categories);
    }

    public void DeleteProductCategory(ProductCategory category)
    {
        _context.ProductCategory.Remove(category);
    }

    public void AddCategory(Category category)
    {
        _context.Category.Add(category);
    }

    public IEnumerable<Category> GetCategories()
    {
        return _context.Category.ToList();
    }

    public Category GetCategory(int id)
    {
        return GetCategories().FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<ProductCategory> GetProductCategories(int productId)
    {
        return _context.ProductCategory.Where(p => p.ProductId == productId).Include(p => p.Category);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}