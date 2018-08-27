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

namespace eCommerce.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private ICartRepository _cartRepository;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUploadRepository _uploadRepository;
        private IProductRepository _productRepository;

        public List<ProductCartViewModel> Products { get; set; }
        public decimal Total { get; set; }

        public IndexModel(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUploadRepository uploadRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _uploadRepository = uploadRepository;
            _productRepository = productRepository;
            Products = new List<ProductCartViewModel>();
            Total = 0M;
        }

        public IActionResult OnGet(string id)
        {
            var userId = String.IsNullOrEmpty(id) ? _userManager.GetUserId(User) : id;

            var cart = _cartRepository.GetCurrentUserCart(userId);
            if (cart == null || cart.OrderDetails.Count == 0 || cart.Payed == true || cart.IsDeleted == true)
                return Page();

            foreach (var item in cart.OrderDetails)
            {
                var price = _productRepository.GetSalePrice((int)item.ProductId) == null ? _productRepository.GetCurrentPrice((int)item.ProductId).Price1 : _productRepository.GetSalePrice((int)item.ProductId).Price1;
                Products.Add(new ProductCartViewModel()
                {
                    Id = item.Id,
                    Price = (decimal)price,
                    ProductName = item.Product.Name,
                    Quantity = (int)item.Quantity,
                    Total = (decimal)price * (int)item.Quantity,
                    ImagePath = Url.Content(_uploadRepository.GetProductThumbnail((int)item.ProductId).Path)
                });
                Total += (decimal)price * (int)item.Quantity;
            }

            return Page();
        }

        public IActionResult OnGetCartCount()
        {
            try
            {
                if(_signInManager.IsSignedIn(User))
                {
                    var userId = _userManager.GetUserId(User);

                    return new JsonResult(_cartRepository.GetCartCount(userId));
                }
                
                return new JsonResult(0);
            }
            catch(Exception)
            {
                return new JsonResult(0);
            }
        }

        public IActionResult OnPostDelete(int deleteId)
        {
            var cartProduct = _cartRepository.GetProductOnCart(deleteId);
            _cartRepository.DeleteProductOnCart(cartProduct);
            _cartRepository.SaveChanges();
            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPostUpdate(int updateId, int updatedQuantity)
        {
            var cartProduct = _cartRepository.GetProductOnCart(updateId);

            var product = _productRepository.GetProduct((int)cartProduct.ProductId);
            if (updatedQuantity > product.Quantity)
                cartProduct.Quantity = product.Quantity;
            else
                cartProduct.Quantity = updatedQuantity;
            cartProduct.Total = updatedQuantity * cartProduct.Price;
            _cartRepository.EditProductOnCart(cartProduct);
            _cartRepository.SaveChanges();
            return RedirectToPage("/Cart/Index");
        }
    }
}