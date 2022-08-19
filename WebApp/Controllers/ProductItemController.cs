using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Resources;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Obsolete("Obsolete")]
public class ProductItemController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IUploadRepository _uploadRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICartRepository _cartRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHostingEnvironment _env;

    public ProductItemController(IProductRepository productRepository, IUploadRepository uploadRepository, ICategoryRepository categoryRepository, SignInManager<ApplicationUser> signInManager, ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
    {
        _productRepository = productRepository;
        _uploadRepository = uploadRepository;
        _categoryRepository = categoryRepository;
        _cartRepository = cartRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _env = env;
    }

    #region Info

    public IActionResult Index(int id)
    {
        var product = _productRepository.GetProduct(id);

        if (product == null)
            return View("Error", new ErrorViewModel
            {
                Message = ErrorConstant.GeneralErrors.ProductNotFind
            });

        var model = new ProductItemViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Stock = product.Quantity ?? 0,
            Quantity = product.Quantity ?? 0,
            Description = product.Description,
            IsOnSale = product.IsOnSale ?? false,
            IsDigit = product.IsDigit,
            Price = _productRepository.GetCurrentPrice(id).Price1 ?? 0
        };

        if (model.IsOnSale)
        {
            model.SalePrice = _productRepository.GetSalePrice(id).Price1 ?? 0;
        }

        var categories = _categoryRepository.GetProductCategories(id).ToList();

        model.Categories = string.Join("، ", categories.Select(x => x.Category.Name));

        var uploads = _uploadRepository.GetProductUploads(id);

        model.Uploads = new List<UploadViewModel>();

        foreach (var item in uploads)
        {
            model.Uploads.Add(new UploadViewModel()
            {
                Id = item.Id,
                FileName = item.FileName,
                Path = item.Path
            });
        }

        model.Products = new List<ProductListViewModel>();

        var products = _productRepository.GetProducts().Where(p => p.Id != id && Equals(p.ProductCategory, product.ProductCategory)).ToList();

        foreach (var item in products)
        {
            model.Products.Add(new ProductListViewModel()
            {
                Id = item.Id,
                ImagePath = Url.Content(_uploadRepository.GetProductThumbnail(item.Id)?.Path),
                IsOnSale = item.IsOnSale ?? false,
                SalePrice = _productRepository.GetSalePrice(item.Id) != null ? _productRepository.GetSalePrice(item.Id).Price1 ?? 0 : 0,
                ProductCategory = _categoryRepository.GetProductCategories(item.Id).Select(x => x.Category.Name).ToList(),
                Price = _productRepository.GetCurrentPrice(item.Id).Price1 ?? 0,
                Title = item.Name,
                Url = "/ProductItem?id=" + item.Id
            });
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(ProductItemViewModel model)
    {
        if (!_signInManager.IsSignedIn(User))
            return RedirectToAction("Login", "Account");

        var userId = _userManager.GetUserId(User);
        var product = _productRepository.GetProduct(model.Id);

        if (model.Quantity > product.Quantity && !product.IsDigit)
            return RedirectToAction("Index", "ProductItem", new {id = model.Id, error = true});

        var hasOpenCart = _cartRepository.HasOpenCart(userId);
        if (hasOpenCart)
        {
            var currentCart = _cartRepository.GetCurrentUserCart(userId);
            var hasCurrentProductOnCart = _cartRepository.HasThatProductOnCart(currentCart.Id, model.Id);
            if (hasCurrentProductOnCart)
            {
                if (product.IsDigit)
                {
                    return RedirectToAction("Index", new {id = model.Id});
                }

                var currentProductOnCart = _cartRepository.GetProductOnCart(currentCart.Id, model.Id);
                currentProductOnCart.Price = model.SalePrice > 0 ? model.SalePrice : model.Price;
                if (currentProductOnCart.Quantity + model.Quantity > product.Quantity)
                    currentProductOnCart.Quantity = product.Quantity;
                else
                    currentProductOnCart.Quantity += model.Quantity;
                currentProductOnCart.Total = model.SalePrice > 0 ? model.SalePrice : model.Price * currentProductOnCart.Quantity;
                currentProductOnCart.LastUpdatedOnDate = DateTime.Now;
                currentProductOnCart.LasUpdatedByUserId = userId;
                _cartRepository.EditProductOnCart(currentProductOnCart);
            }
            else
            {
                var cartProd = new OrderDetails()
                {
                    OrderId = currentCart.Id,
                    ProductId = model.Id,
                    Price = model.SalePrice > 0 ? model.SalePrice : model.Price,
                    Quantity = model.Quantity,
                    Total = model.SalePrice > 0 ? model.SalePrice : model.Price * model.Quantity,
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
                ProductId = model.Id,
                Price = model.SalePrice > 0 ? model.SalePrice : model.Price,
                Quantity = model.Quantity,
                Total = model.SalePrice > 0 ? model.SalePrice : model.Price * model.Quantity,
                IsDeleted = false,
                CreatedByUserId = userId,
                CreatedOnDate = DateTime.Now
            };
            _cartRepository.AddProductToCart(cartProd);
        }

        _cartRepository.SaveChanges();
        return RedirectToAction("Index", new {id = model.Id});
    }

    #endregion

    #region Delete

    [HttpPost]
    [Authorize(Roles = GeneralConstant.Roles.Admin)]
    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetProduct(id);
        product.IsDeleted = true;
        var uploads = _uploadRepository.GetProductUploads(id);
        foreach (var item in uploads)
        {
            var path = Path.Combine(_env.WebRootPath, "uploads", "products", id.ToString(), item.FileName);
            System.IO.File.Delete(path);
            item.IsDeleted = true;
            _uploadRepository.EditUpload(item);
        }

        var thumbnail = _uploadRepository.GetProductThumbnail(id);
        var pathItem = Path.Combine(_env.WebRootPath, "uploads", "products", id.ToString(), thumbnail.FileName);
        System.IO.File.Delete(pathItem);
        thumbnail.IsDeleted = true;
        _uploadRepository.EditUpload(thumbnail);
        _uploadRepository.SaveChanges();
        return RedirectToAction("Index", "Products");
    }

    #endregion

    #region PriceList

    [HttpGet]
    [Authorize(Roles = GeneralConstant.Roles.Admin)]
    public Task<IActionResult> PriceList()
    {
        var model = _productRepository.GetProducts().Select(x => new PriceListViewModel
        {
            ProductId = x.Id,
            Name = x.Name,
            IsOnSale = x.IsOnSale ?? false,
            IsDigit = x.IsDigit,
            Price = _productRepository.GetCurrentPrice(x.Id),
            OffPrice = _productRepository.GetSalePrice(x.Id) != null ? _productRepository.GetSalePrice(x.Id).Price1 ?? 0 : 0,
        }).ToList();

        return Task.FromResult<IActionResult>(View(model));
    }

    [HttpPost]
    public async Task<ActionResult> PriceList([Bind(Prefix = "item")] PriceListViewModel model)
    {
        try
        {
            var product = _productRepository.GetProduct(model.ProductId);
            product.IsOnSale = model.IsOnSale;
            product.IsDigit = model.IsDigit;
            product.LasUpdatedByUserId = _userManager.GetUserId(User);
            product.LastUpdatedOnDate = DateTime.Now;

            _productRepository.EditProduct(product);

            var price = _productRepository.GetCurrentPrice(product.Id);
            price.IsActive = true;
            price.IsSale = false;
            price.Price1 = model.Price.Price1;

            var oldPrice = _productRepository.GetSalePrice(product.Id);
            if (model.IsOnSale)
            {
                if (oldPrice != null)
                {
                    oldPrice.IsActive = true;
                    oldPrice.IsSale = true;
                    oldPrice.Price1 = model.OffPrice;
                    _productRepository.EditProductPrice(oldPrice);
                }
                else
                {
                    var salePrice = new Price();
                    price.IsSale = true;
                    price.Price1 = model.OffPrice;
                    price.ProductId = product.Id;
                    price.IsActive = true;
                    _productRepository.AddPriceToProduct(salePrice);
                }
            }
            else if (oldPrice != null)
            {
                oldPrice.IsActive = false;
                _productRepository.EditProductPrice(oldPrice);
            }

            _productRepository.SaveChanges();
            return Ok("ثبت انجام شد");
        }
        catch (Exception e)
        {
            return Ok("ثبت با خطا مواجع شد");
        }
    }

    #endregion
}