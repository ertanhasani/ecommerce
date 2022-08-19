using WebApp.Data;

namespace WebApp.Repositories;

public interface ICategoryRepository
{
    void AddProductCategory(ProductCategory category);

    void AddCategory(Category category);

    void AddProductCategories(List<ProductCategory> categories);

    void DeleteProductCategory(ProductCategory category);

    void DeleteProductCategories(List<ProductCategory> categories);
    void UpdateCategories(List<Category> categories);

    IEnumerable<Category> GetCategories();

    Category GetCategory(int id);

    IEnumerable<ProductCategory> GetProductCategories(int productId);

    void SaveChanges();
}