using eCommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Repositories
{
    public interface ICategoryRepository
    {
        void AddProductCategory(ProductCategory category);
        void AddProductCategories(List<ProductCategory> categories);
        void DeleteProductCategory(ProductCategory category);
        void DeleteProductCategories(List<ProductCategory> categories);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        IEnumerable<ProductCategory> GetProductCategories(int productId);
        void SaveChanges();
    }
}
