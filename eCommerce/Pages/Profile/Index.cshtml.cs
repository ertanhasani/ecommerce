using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Repositories;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private ICartRepository _cartRepository;

        public List<OrdersViewModel> Orders { get; set; }
        public string UserId { get; set; }
        public ProfileViewModel Profile { get; set; }

        public IndexModel(UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            Orders = new List<OrdersViewModel>();
        }

        public async Task<IActionResult> OnGetAsync(string userId, string toastr, bool error = false)
        {
            if (String.IsNullOrEmpty(userId))
                return RedirectToPage("/Index");
            
            var userProfile = await _userManager.FindByIdAsync(userId);
            if ((User.IsInRole("Admin") || (!User.IsInRole("Admin") && _userManager.GetUserId(User).Equals(userId))) && userProfile != null)
            {
                UserId = userId;
                Profile = new ProfileViewModel() {
                    Id = userId,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    BirthDate = userProfile.BirthDate,
                    Email = userProfile.Email
                };
                var orders = _cartRepository.GetUserCarts(userId).OrderByDescending(p => p.Id);
                foreach(var item in orders)
                {
                    Orders.Add(new OrdersViewModel()
                    {
                        Id = item.Id,
                        OrderedDate = item.LastUpdatedOnDate.Value.ToString("dd.MM.yyyy"),
                        Status = item.Status.Name,
                        TotalPrice = 120
                    });
                }

                if (!String.IsNullOrEmpty(toastr) && !error)
                    ViewData["Success"] = toastr;
                if (!String.IsNullOrEmpty(toastr) && error)
                    ViewData["Error"] = toastr;
                return Page();
            }
            else
                return RedirectToPage("/Index");

        }
    }
}