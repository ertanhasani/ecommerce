using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Repositories;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages.Producti
{
    public class IndexModel : PageModel
    {
        private IProductRepository _productRepository;
        private IUploadRepository _uploadRepository;
        private ICategoryRepository _categoryRepository;
        private ICartRepository _cartRepository;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _env;

        [BindProperty]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [BindProperty]
        public decimal Price { get; set; }
        [BindProperty]
        public decimal SalePrice { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        public int Stock { get; set; }
        public bool IsOnSale { get; set; }
        public List<UploadViewModel> Uploads{ get; set; }
        public List<ProductListViewModel> Products { get; set; }
        public string Categories { get; set; }

        public IndexModel(IProductRepository productRepository, IUploadRepository uploadRepository, ICategoryRepository categoryRepository, SignInManager<ApplicationUser> signInManager, ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
        {
            _productRepository = productRepository;
            _uploadRepository = uploadRepository;
            _categoryRepository = categoryRepository;
            _cartRepository = cartRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _env = env;
            Uploads = new List<UploadViewModel>();
            Products = new List<ProductListViewModel>();
        }

        public IActionResult OnGet(int id, string toastr, bool error = false)
        {
            var model = _productRepository.GetProduct(id);

            if (model == null)
                return RedirectToPage("/Index");

            Id = model.Id;
            Name = model.Name;
            Stock = (int)model.Quantity;
            Description = model.Description;
            IsOnSale = (bool)model.IsOnSale;
            Price = (decimal)_productRepository.GetCurrentPrice(id).Price1;
            if(IsOnSale)
            {
                SalePrice = (decimal)_productRepository.GetSalePrice(id).Price1;
            }
            var categories = _categoryRepository.GetProductCategories(id);
            var i = 0;
            foreach(var item in categories)
            {
                if (i == 0)
                    Categories = item.Category.Name;
                else
                    Categories += ", " + item.Category.Name;
            }

            var uploads = _uploadRepository.GetProductUploads(id);
            foreach (var item in uploads)
            {
                Uploads.Add(new UploadViewModel() {
                    FileName = item.FileName,
                    Path = item.Path
                });
            }

            var product = _productRepository.GetProducts().Where(p => p.Id != id).Take(4);
            foreach(var item in product)
            {
                Products.Add(new ProductListViewModel()
                {
                    Id = item.Id,
                    ImagePath = Url.Content(_uploadRepository.GetProductThumbnail(item.Id).Path),
                    IsOnSale = (bool)item.IsOnSale,
                    SalePrice = _productRepository.GetSalePrice(item.Id) != null ? (decimal)_productRepository.GetSalePrice(item.Id).Price1 : 0.00M,
                    Price = (decimal)_productRepository.GetCurrentPrice(item.Id).Price1,
                    Title = item.Name,
                    Url = "/Producti/" + item.Id
                });
            }

            if (!String.IsNullOrEmpty(toastr) && !error)
                ViewData["Success"] = toastr;

            if(!String.IsNullOrEmpty(toastr) && error)
                ViewData["Error"] = toastr;

            return Page();
        }


        public IActionResult OnPost()
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToPage("/Account/Login");

            var userId = _userManager.GetUserId(User);
            var product = _productRepository.GetProduct(Id);

            if (Quantity > product.Quantity)
                return RedirectToPage("/Producti/Index", new { id = Id, toastr = "Quantity selected should be equal or less than stock.", error = true });

            var hasOpenCart = _cartRepository.HasOpenCart(userId);
            if(hasOpenCart)
            {
                var currentCart = _cartRepository.GetCurrentUserCart(userId);
                var hasCurrentProductOnCart = _cartRepository.HasThatProductOnCart(currentCart.Id, Id);
                if(hasCurrentProductOnCart)
                {
                    var currentProductOnCart = _cartRepository.GetProductOnCart(currentCart.Id, Id);
                    currentProductOnCart.Price = SalePrice > 0 ? SalePrice : Price;
                    if (currentProductOnCart.Quantity + Quantity > product.Quantity)
                        currentProductOnCart.Quantity = product.Quantity;
                    else
                        currentProductOnCart.Quantity += Quantity;
                    currentProductOnCart.Total = SalePrice > 0 ? SalePrice : Price * currentProductOnCart.Quantity;
                    currentProductOnCart.LastUpdatedOnDate = DateTime.Now;
                    currentProductOnCart.LasUpdatedByUserId = userId;
                    _cartRepository.EditProductOnCart(currentProductOnCart);
                }
                else{
                    var cartProd = new OrderDetails() {
                        OrderId = currentCart.Id,
                        ProductId = Id,
                        Price = SalePrice > 0 ? SalePrice : Price,
                        Quantity = Quantity,
                        Total = SalePrice > 0 ? SalePrice : Price * Quantity,
                        IsDeleted = false,
                        CreatedByUserId = userId,
                        CreatedOnDate = DateTime.Now
                    };
                    _cartRepository.AddProductToCart(cartProd);
                }
            }
            else
            {
                var newCart = new Order()
                {
                    UserId = userId,
                    IsDeleted = false,
                    Payed = false,
                    CreatedByUserId = userId,
                    CreatedOnDate = DateTime.Now
                };
                _cartRepository.CreateCart(newCart);
                _cartRepository.SaveChanges();

                var cartProd = new OrderDetails()
                {
                    OrderId = newCart.Id,
                    ProductId = Id,
                    Price = SalePrice > 0 ? SalePrice : Price,
                    Quantity = Quantity,
                    Total = SalePrice > 0 ? SalePrice : Price * Quantity,
                    IsDeleted = false,
                    CreatedByUserId = userId,
                    CreatedOnDate = DateTime.Now
                };
                _cartRepository.AddProductToCart(cartProd);
            }

            _cartRepository.SaveChanges();
            return RedirectToPage("Index", new { id = Id, toastr = "Product added to cart."});
        }

        public IActionResult OnPostDelete()
        {
            if(User.IsInRole("Admin"))
            {
                var product = _productRepository.GetProduct(Id);
                product.IsDeleted = true;
                var uploads = _uploadRepository.GetProductUploads(Id);
                foreach (var item in uploads)
                {
                    var path = System.IO.Path.Combine(_env.WebRootPath, "uploads", "products", Id.ToString(), item.FileName);
                    System.IO.File.Delete(path);
                    item.IsDeleted = true;
                    _uploadRepository.EditUpload(item);
                }

                var thumbnail = _uploadRepository.GetProductThumbnail(Id);
                var pathi = System.IO.Path.Combine(_env.WebRootPath, "uploads", "products", Id.ToString(), thumbnail.FileName);
                System.IO.File.Delete(pathi);
                thumbnail.IsDeleted = true;
                _uploadRepository.EditUpload(thumbnail);
                _uploadRepository.SaveChanges();
                return RedirectToPage("/Products/Index", new { toastr = "Product deleted successfully." });
            }

            return RedirectToPage("/Products/Index");
        }
    }
}