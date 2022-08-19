namespace WebApp.ViewModels;

public class StatisticsViewModel
{
    public List<UsersViewModel> Users { get; set; }
    
    public List<ProductCartViewModel> Products { get; set; }
    
    public List<OrdersViewModel> CostOrders { get; set; }
    
    public List<OrdersViewModel> ProductOrders { get; set; }
}