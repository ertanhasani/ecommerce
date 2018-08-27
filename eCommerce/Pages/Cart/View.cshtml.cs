using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Repositories;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerce.Pages.Cart
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private ICartRepository _cartRepository;
        private IUploadRepository _uploadRepository;
        [BindProperty]
        public int StatusId { get; set; }
        [BindProperty]
        public int Id { get; set; }

        public List<ProductCartViewModel> Products { get; set; }
        public ShippingViewModel Shipping { get; set; }
        public decimal Total { get; set; }

        public ViewModel(ICartRepository cartRepository, IUploadRepository uploadRepository)
        {
            _cartRepository = cartRepository;
            _uploadRepository = uploadRepository;
            Total = 0M;
            Products = new List<ProductCartViewModel>();
            Shipping = new ShippingViewModel();
        }

        public IActionResult OnGet(int id, string toastr)
        {
            var cart = _cartRepository.GetCart(id);

            if (id == 0 || cart == null)
                return RedirectToPage("/Index");
            
            foreach(var item in cart.OrderDetails)
            {
                Products.Add(new ProductCartViewModel()
                {
                    Id = (int)item.ProductId,
                    ProductName = item.Product.Name,
                    ImagePath = Url.Content(_uploadRepository.GetProductThumbnail((int)item.ProductId).Path),
                    Price = (decimal)item.Price,
                    Quantity = (int)item.Quantity,
                    Total = (decimal)item.Total
                });
                Total += (decimal)item.Total;
            }

            if (User.IsInRole("Admin"))
            {
                Id = id;
                var statuses = _cartRepository.GetCartStatues();
                var currentStatus = _cartRepository.GetCartStatus(id);
                StatusId = currentStatus.Id;
                ViewData["Statuses"] = new SelectList(statuses, "Id", "Name", currentStatus);
            }

            var shipping = _cartRepository.GetCartShippingAddress(id);
            Shipping.FullName = shipping.FullName;
            Shipping.Address1 = shipping.Address1;
            Shipping.Address2 = shipping.Address2;
            Shipping.City = shipping.City;
            Shipping.State = shipping.State;
            Shipping.ZipCode = shipping.PostalCode;

            if (!String.IsNullOrEmpty(toastr))
                ViewData["Success"] = toastr;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (User.IsInRole("Admin"))
            {
                var cart = _cartRepository.GetCart(Id);
                cart.StatusId = StatusId;
                _cartRepository.EditCart(cart);
                _cartRepository.SaveChanges();
                return RedirectToPage("/Cart/View", new { id = Id, toastr = "Cart updated successfully." });
            }
            else
                return RedirectToPage("/Index");
        }
    }
}