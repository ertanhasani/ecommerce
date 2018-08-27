using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Repositories;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages.Panel
{
    [Authorize(Roles = "Admin")]
    public class StatisticsModel : PageModel
    {
        private IPanelRepository _panelRepository;
        private IProductRepository _productRepository;

        public List<UsersViewModel> Users { get; set; }
        public List<ProductCartViewModel> Products { get; set; }
        public List<OrdersViewModel> CostOrders { get; set; }
        public List<OrdersViewModel> ProductOrders { get; set; }

        public StatisticsModel(IPanelRepository panelRepository, IProductRepository productRepository)
        {
            _panelRepository = panelRepository;
            _productRepository = productRepository;
            Users = new List<UsersViewModel>();
            Products = new List<ProductCartViewModel>();
            CostOrders = new List<OrdersViewModel>();
            ProductOrders = new List<OrdersViewModel>();
        }

        public void OnGet()
        {
            var users = _panelRepository.GetTopUsers().Take(10);
            foreach(var item in users)
            {
                Users.Add(new UsersViewModel()
                {
                    Id = item.Id,
                    Email = item.Email,
                    FullName = item.FirstName + " " + item.LastName,
                    Orders = _panelRepository.HowManyOrder(item.Id)
                });
            }

            var products = _panelRepository.GetTopProducts().Take(10);
            foreach(var item in products)
            {
                Products.Add(new ProductCartViewModel()
                {
                    Id = item.Id,
                    ProductName = item.Name,
                    Price = (decimal)_productRepository.GetCurrentPrice(item.Id).Price1,
                    Quantity = (int)item.Quantity,
                    Sold = _panelRepository.GetProductSales(item.Id)
                });
            }

            var costOrders = _panelRepository.GetTopCostOrders().Take(10);
            foreach(var item in costOrders)
            {
                CostOrders.Add(new OrdersViewModel()
                {
                    Id = item.Id,
                    OrderedDate = item.LastUpdatedOnDate.Value.ToString("dd.MM.yyyy"),
                    UsersFullName = item.User.FirstName + " " + item.User.LastName,
                    UserId = item.User.Id,
                    Status = item.Status.Name,
                    TotalPrice = (decimal)item.TotalPrice
                });
            }

            var productOrders = _panelRepository.GetTopProductOrders().Take(10);
            foreach(var item in productOrders)
            {
                ProductOrders.Add(new OrdersViewModel()
                {
                    Id = item.Id,
                    OrderedDate = item.LastUpdatedOnDate.Value.ToString("dd.MM.yyyy"),
                    UsersFullName = item.User.FirstName + " " + item.User.LastName,
                    UserId = item.User.Id,
                    Status = item.Status.Name,
                    TotalPrice = (decimal)item.TotalPrice
                });
            }
        }
    }
}