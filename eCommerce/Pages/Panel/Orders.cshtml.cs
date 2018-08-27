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
    public class OrdersModel : PageModel
    {
        private IPanelRepository _panelRepository;

        public OrdersModel(IPanelRepository panelRepository)
        {
            _panelRepository = panelRepository;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetOrders(string search, int pageNr)
        {
            var model = new List<OrdersViewModel>();
            var orders = _panelRepository.GetOrders().Where(p => p.StatusId == 1);

            decimal pagesInDecimal = (decimal)orders.Count() / 20;
            var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int)pagesInDecimal + 1;

            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                orders = orders.Where(p => p.Id.ToString().Contains(search) || p.Status.Name.ToLower().Contains(search) || (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).Contains(search) || p.User.Email.ToLower().Contains(search));
            }

            orders = orders.OrderByDescending(p => p.LastUpdatedOnDate).Skip((pageNr - 1) * 20).Take(20);
            foreach (var item in orders)
            {
                model.Add(new OrdersViewModel()
                {
                    Id = item.Id,
                    OrderedDate = item.LastUpdatedOnDate.Value.ToString("dd.MM.yyyy"),
                    Status = item.Status.Name,
                    TotalPrice = (decimal)item.TotalPrice,
                    UsersFullName = item.User.FirstName + " " + item.User.LastName,
                    UsersEmail = item.User.Email
                });
            }

            return new JsonResult(new
            {
                totalPages = pages,
                orders = model
            });
        }

        public IActionResult OnGetProcessingOrders(string search, int pageNr)
        {
            var model = new List<OrdersViewModel>();
            var orders = _panelRepository.GetOrders().Where(p => p.StatusId == 2);

            decimal pagesInDecimal = (decimal)orders.Count() / 20;
            var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int)pagesInDecimal + 1;

            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                orders = orders.Where(p => p.Id.ToString().Contains(search) || p.Status.Name.ToLower().Contains(search) || (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).Contains(search) || p.User.Email.ToLower().Contains(search));
            }

            orders = orders.OrderByDescending(p => p.LastUpdatedOnDate).Skip((pageNr - 1) * 20).Take(20);
            foreach (var item in orders)
            {
                model.Add(new OrdersViewModel()
                {
                    Id = item.Id,
                    OrderedDate = item.LastUpdatedOnDate.Value.ToString("dd.MM.yyyy"),
                    Status = item.Status.Name,
                    TotalPrice = (decimal)item.TotalPrice,
                    UsersFullName = item.User.FirstName + " " + item.User.LastName,
                    UsersEmail = item.User.Email
                });
            }

            return new JsonResult(new
            {
                totalPages = pages,
                orders = model
            });
        }

        public IActionResult OnGetShippedOrders(string search, int pageNr)
        {
            var model = new List<OrdersViewModel>();
            var orders = _panelRepository.GetOrders().Where(p => p.StatusId == 3);

            decimal pagesInDecimal = (decimal)orders.Count() / 20;
            var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int)pagesInDecimal + 1;

            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                orders = orders.Where(p => p.Id.ToString().Contains(search) || p.Status.Name.ToLower().Contains(search) || (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).Contains(search) || p.User.Email.ToLower().Contains(search));
            }

            orders = orders.OrderByDescending(p => p.LastUpdatedOnDate).Skip((pageNr - 1) * 20).Take(20);
            foreach (var item in orders)
            {
                model.Add(new OrdersViewModel()
                {
                    Id = item.Id,
                    OrderedDate = item.LastUpdatedOnDate.Value.ToString("dd.MM.yyyy"),
                    Status = item.Status.Name,
                    TotalPrice = (decimal)item.TotalPrice,
                    UsersFullName = item.User.FirstName + " " + item.User.LastName,
                    UsersEmail = item.User.Email
                });
            }

            return new JsonResult(new
            {
                totalPages = pages,
                orders = model
            });
        }
    }
}