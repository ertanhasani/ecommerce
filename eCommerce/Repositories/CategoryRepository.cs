using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private eCommerceContext _context;

        public CategoryRepository(eCommerceContext context)
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

        public void DeleteProductCategory(ProductCategory category)
        {
            _context.ProductCategory.Remove(category);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Category;
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
}
