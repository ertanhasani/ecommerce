namespace WebApp.Services;

public interface IGeneralServices
{
    void AddCategories(int[] categories, int productId, bool isEditing);
}