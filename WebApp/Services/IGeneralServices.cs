namespace WebApp.Services;

public interface IGeneralServices
{
    void AddCategories(IEnumerable<int> categories, int productId, bool isEditing);
}