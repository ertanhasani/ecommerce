using WebApp.Data;
using WebApp.Repositories;

namespace WebApp.Services;

public class GeneralServices : IGeneralServices
{
    private readonly ICategoryRepository _categoryRepository;

    public GeneralServices(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public void AddCategories(IEnumerable<int> categories, int productId, bool isEditing)
    {
        if (isEditing)
        {
            var oldCategories = _categoryRepository.GetProductCategories(productId).ToList();
            _categoryRepository.DeleteProductCategories(oldCategories);
            _categoryRepository.SaveChanges();
        }

        var newCategories = categories.Select(item => new ProductCategory {CategoryId = item, ProductId = productId}).ToList();

        _categoryRepository.AddProductCategories(newCategories);
        _categoryRepository.SaveChanges();
    }
}