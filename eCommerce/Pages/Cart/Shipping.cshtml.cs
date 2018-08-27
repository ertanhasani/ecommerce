using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages.Cart
{
    [Authorize]
    public class ShippingModel : PageModel
    {
        private ICartRepository _cartRepository;
        private UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string Address1 { get; set; }
        [BindProperty]
        public string Address2 { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string State { get; set; }
        [BindProperty]
        public string ZipCode { get; set; }
        [BindProperty]
        public string NameOnCard { get; set; }
        [BindProperty]
        public string CardNumber { get; set; }
        [BindProperty]
        public string ExpireDate { get; set; }
        [BindProperty]
        public int? CVV { get; set; }

        public ShippingModel(ICartRepository cartRepository, UserManager<ApplicationUser> userManager)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        public IActionResult OnGet(string toastr)
        {
            var userId = _userManager.GetUserId(User);
            var hasACart = _cartRepository.HasOpenCart(userId);
            if (hasACart)
            {
                var currentCart = _cartRepository.GetCurrentUserCart(userId);
                if(currentCart.OrderDetails.Count == 0)
                    return RedirectToPage("/Index");

                if (!String.IsNullOrEmpty(toastr))
                    ViewData["Success"] = toastr;

                return Page();
            }
            else
                return RedirectToPage("/Index");
        }

        public IActionResult OnPost()
        {
            var userId = _userManager.GetUserId(User);

            var shipping = new Shipping();
            shipping.Address1 = Address1;
            shipping.Address2 = Address2;
            shipping.City = City;
            shipping.State = State;
            shipping.PostalCode = ZipCode;
            shipping.FullName = FullName;
            shipping.CreatedByUserId = userId;
            shipping.CreatedOnDate = DateTime.Now;
            _cartRepository.AddShipping(shipping);
            _cartRepository.SaveChanges();


            var currentCart = _cartRepository.GetCurrentUserCart(userId);

            var total = 0M;
            var orderDetails = currentCart.OrderDetails.Select(p => p.Total);
            foreach (var item in orderDetails)
                total += (decimal)item;

            currentCart.TotalPrice = total;
            currentCart.ShippingId = shipping.Id;
            currentCart.Payed = true;
            currentCart.StatusId = 1;
            currentCart.LastUpdatedOnDate = DateTime.Now;
            currentCart.LasUpdatedByUserId = userId;
            _cartRepository.EditCart(currentCart);
            _cartRepository.SaveChanges();
            
            return RedirectToPage("/Cart/View", new { id = currentCart.Id, toastr = "Your cart has been ordered." });
        }
    }
}