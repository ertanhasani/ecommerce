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
    public class UsersModel : PageModel
    {
        private IPanelRepository _panelRepository;

        public UsersModel(IPanelRepository panelRepository)
        {
            _panelRepository = panelRepository;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetUsers(string search, int pageNr)
        {
            var list = new List<UsersViewModel>();
            var users = _panelRepository.GetUsers();

            decimal pagesInDecimal = (decimal)users.Count() / 20;
            var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int)pagesInDecimal + 1;

            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                users = users.Where(p => p.Id.ToLower().Contains(search) || p.Email.ToLower().Contains(search) || (p.FirstName + " " + p.LastName).ToLower().Contains(search));
            }

            users = users.OrderByDescending(p => p.CreatedOnDate).Skip((pageNr - 1) * 20).Take(20);
            foreach(var item in users)
            {
                list.Add(new UsersViewModel()
                {
                    Id = item.Id,
                    FullName = item.FirstName + " " + item.LastName,
                    Email = item.Email,
                    CurrentCart = _panelRepository.HasOpenCart(item.Id),
                    Orders = _panelRepository.HowManyOrder(item.Id),
                    BirthDate = item.BirthDate.ToString("dd.MM.yyyy")
                });
            }

            return new JsonResult(new {
                totalPages = pages,
                users = list
            });
        }
    }
}