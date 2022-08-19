using WebApp.Data;

namespace WebApp.ViewModels;

public class AllProductViewModel
{
    public int? CategoryId { get; set; }

    public List<ProductListViewModel> ProductList { get; set; }

    public Category Category { get; set; }

    public string SearchQuery { get; set; }
}