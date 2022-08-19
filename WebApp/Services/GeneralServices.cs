using WebApp.Data;
using WebApp.Repositories;

namespace WebApp.Services;

public class GeneralServices : IGeneralServices
{
    private ICategoryRepository _categoryRepository;

    public GeneralServices(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public void AddCategories(int[] categories, int productId, bool isEditing)
    {
        if (isEditing)
        {
            var oldCategories = _categoryRepository.GetProductCategories(productId).ToList();
            _categoryRepository.DeleteProductCategories(oldCategories);
            _categoryRepository.SaveChanges();
        }

        var newCategories = new List<ProductCategory>();

        foreach (var item in categories)
        {
            newCategories.Add(new ProductCategory
            {
                CategoryId = item,
                ProductId = productId
            });
        }

        _categoryRepository.AddProductCategories(newCategories);
        _categoryRepository.SaveChanges();
    }
}