using WebApp.Data;

namespace WebApp.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> GetProducts();

    Product GetProduct(int productId);

    void AddProduct(Product product);

    void EditProduct(Product product);

    void DeleteProduct(Product product);

    IEnumerable<Price> GetPricesOfProduct(int productId);

    void EditProductPrice(Price price);

    Price GetCurrentPrice(int productId);

    Price GetSalePrice(int productId);

    void AddPriceToProduct(Price price);

    void SaveChanges();
}