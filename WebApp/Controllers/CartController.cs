using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.Extensions;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Resources;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route(GeneralConstant.Routes.ControllerDefault)]
[Authorize(Roles = GeneralConstant.Roles.All)]
public class CartController : Controller
{
    private readonly ICartRepository _cartRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUploadRepository _uploadRepository;
    private readonly IProductRepository _productRepository;

    public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUploadRepository uploadRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _uploadRepository = uploadRepository;
        _productRepository = productRepository;
    }

    #region Info

    [HttpGet]
    public Task<IActionResult> Index(string id)
    {
        string userId;

        if (User.IsInRole(GeneralConstant.Roles.Admin))
        {
            userId = string.IsNullOrEmpty(id) ? _userManager.GetUserId(User) : id;
        }
        else
        {
            if (!string.IsNullOrEmpty(id) && _userManager.GetUserId(User) != id)
            {
                return Task.FromResult<IActionResult>(RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = ErrorConstant.GeneralErrors.GetCart
                }));
            }

            userId = _userManager.GetUserId(User);
        }

        var cart = _cartRepository.GetCurrentUserCart(userId);

        var model = new CartViewModel
        {
            Total = 0M,
            Products = new List<ProductCartViewModel>()
        };

        if (cart == null || cart.OrderDetails.Count == 0 || cart.Payed == true || cart.IsDeleted == true)
            return Task.FromResult<IActionResult>(View(model));

        model.Products = new List<ProductCartViewModel>();

        foreach (var item in cart.OrderDetails)
        {
            var price = _productRepository.GetSalePrice((int) item.ProductId!) == null ? _productRepository.GetCurrentPrice((int) item.ProductId).Price1 : _productRepository.GetSalePrice((int) item.ProductId).Price1;

            model.Products.Add(new ProductCartViewModel
            {
                Id = item.Id,
                Price = price ?? 0,
                ProductName = item.Product.Name,
                Quantity = item.Quantity ?? 0,
                IsDigit = item.Product.IsDigit,
                Total = price ?? 0 * item.Quantity ?? 0,
                ImagePath = Url.Content(_uploadRepository.GetProductThumbnail((int) item.ProductId).Path)
            });

            model.Total += (price ?? 0) * (item.Quantity ?? 0);
        }

        return Task.FromResult<IActionResult>(View(model));
    }

    [HttpGet]
    public IActionResult Detail(int id)
    {
        var cart = _cartRepository.GetCart(id);

        if (id == 0 || cart == null)
            return View("Index");

        if (cart.UserId != _userManager.GetUserId(User) && !User.IsInRole(GeneralConstant.Roles.Admin))
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel
            {
                Message = ErrorConstant.GeneralErrors.GetCart
            });
        }

        var model = new CartViewModel
        {
            Total = 0M,
            Products = new List<ProductCartViewModel>()
        };

        foreach (var item in cart.OrderDetails)
        {
            model.Products.Add(new ProductCartViewModel()
            {
                Id = (int) item.ProductId!,
                ProductName = item.Product.Name,
                ImagePath = Url.Content(_uploadRepository.GetProductThumbnail((int) item.ProductId).Path),
                Price = item.Price ?? 0,
                Quantity = item.Quantity ?? 0,
                Total = item.Total ?? 0
            });

            model.Total += item.Total ?? 0;
        }

        if (User.IsInRole(GeneralConstant.Roles.Admin))
        {
            model.Id = id;

            var statuses = _cartRepository.GetCartStatues();

            var currentStatus = _cartRepository.GetCartStatus(id);

            model.StatusId = currentStatus.Id;

            ViewData["Statuses"] = new SelectList(statuses, "Id", "Name", currentStatus);
        }

        var shipping = _cartRepository.GetCartShippingAddress(id);

        if (shipping != null)
        {
            model.Shipping = new ShippingViewModel
            {
                FullName = shipping.FullName,
                Address = shipping.Address,
                City = shipping.City,
                State = shipping.State,
                PostalCode = shipping.PostalCode
            };
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult ChangeStatus(CartViewModel model)
    {
        if (!User.IsInRole("Admin"))
            return RedirectToAction("Detail", new {id = model.Id});

        var cart = _cartRepository.GetCart(model.Id);
        cart.StatusId = model.StatusId;
        _cartRepository.EditCart(cart);
        _cartRepository.SaveChanges();

        return RedirectToAction("Detail", new {id = model.Id});
    }

    #endregion

    #region Count

    [AllowAnonymous]
    [HttpGet]
    public IActionResult CartCount()
    {
        try
        {
            if (!_signInManager.IsSignedIn(User)) return new JsonResult(0);

            var userId = _userManager.GetUserId(User);

            return new JsonResult(_cartRepository.GetCartCount(userId));
        }
        catch (Exception)
        {
            return new JsonResult(0);
        }
    }

    #endregion

    #region Crud

    [HttpPost]
    public IActionResult Delete(int deleteId)
    {
        var cartProduct = _cartRepository.GetProductOnCart(deleteId);

        if (cartProduct.Order.UserId != _userManager.GetUserId(User) && !User.IsInRole(GeneralConstant.Roles.Admin))
        {
            this.AddErrors(ErrorConstant.DeletePermission);
            return RedirectToAction("Index");
        }

        _cartRepository.DeleteProductOnCart(cartProduct);

        _cartRepository.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(int updateId, int updatedQuantity)
    {
        var cartProduct = _cartRepository.GetProductOnCart(updateId);

        var product = _productRepository.GetProduct((int) cartProduct.ProductId!);

        cartProduct.Quantity = updatedQuantity > product.Quantity ? product.Quantity : updatedQuantity;

        cartProduct.Total = cartProduct.Quantity * cartProduct.Price;

        _cartRepository.EditProductOnCart(cartProduct);

        _cartRepository.SaveChanges();

        return RedirectToAction("Index", new {id = cartProduct.Order.UserId});
    }

    #endregion

    #region Shipping

    [HttpGet]
    public IActionResult Shipping()
    {
        var userId = _userManager.GetUserId(User);
        var hasACart = _cartRepository.HasOpenCart(userId);

        if (hasACart)
        {
            var currentCart = _cartRepository.GetCurrentUserCart(userId);

            if (currentCart.OrderDetails.Count == 0)
                return RedirectToAction("Index");

            return View(new ShippingViewModel());
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Shipping(ShippingViewModel model)
    {
        var userId = _userManager.GetUserId(User);

        var shipping = new Shipping();
        shipping.Address = model.Address;
        shipping.City = model.City;
        shipping.State = model.State;
        shipping.PostalCode = model.PostalCode;
        shipping.FullName = model.FullName;
        shipping.CreatedByUserId = userId;
        shipping.CreatedOnDate = DateTime.Now;
        _cartRepository.AddShipping(shipping);
        _cartRepository.SaveChanges();


        var currentCart = _cartRepository.GetCurrentUserCart(userId);

        var total = 0M;
        var orderDetails = currentCart.OrderDetails.Select(p => p.Total);
        foreach (var item in orderDetails)
            total += (decimal) item;

        currentCart.TotalPrice = total;
        currentCart.ShippingId = shipping.Id;
        currentCart.Payed = true;
        currentCart.StatusId = 1;
        currentCart.IsDeleted = false;
        currentCart.LastUpdatedOnDate = DateTime.Now;
        currentCart.LasUpdatedByUserId = userId;
        _cartRepository.EditCart(currentCart);
        _cartRepository.SaveChanges();

        return RedirectToAction("Detail", currentCart.Id);
    }

    #endregion

    #region DigitFile

    [HttpGet]
    public Task<IActionResult> DigitFileList()
    {
        var user = _userManager.GetUserId(User);

        if (user == null)
        {
            return Task.FromResult<IActionResult>(RedirectToAction("Error", "Home", new ErrorViewModel
            {
                Message = ErrorConstant.GeneralErrors.GetCart
            }));
        }

        var cartList = _cartRepository.GetDigitFileList(user);

        var model = new CartViewModel
        {
            Total = 0M,
            Products = new List<ProductCartViewModel>()
        };

        model.Products = new List<ProductCartViewModel>();

        foreach (var cart in cartList.Where(x => x.Product.IsDigit))
        {
            model.Products.Add(new ProductCartViewModel
                {
                    Id = cart.Id,
                    ProductName = cart.Product.Name,
                    ImagePath = Url.Content(_uploadRepository.GetProductThumbnail(cart.ProductId ?? 0).Path)
                });
        }

        return Task.FromResult<IActionResult>(View(model));
    }

    [HttpPost]
    public IActionResult DownloadFile(int id)
    {
        var user = _userManager.GetUserId(User);

        if (user == null)
        {
            return RedirectToAction("Error", "Home", new ErrorViewModel
            {
                Message = ErrorConstant.GeneralErrors.GetCart
            });
        }

        var cartList = _cartRepository.GetDigitFileList(user);

        var model = new CartViewModel
        {
            Products = new List<ProductCartViewModel>()
        };

        model.Products = new List<ProductCartViewModel>();

        var order = new OrderDetails();

        foreach (var cart in cartList)
        {
            model.Products.Add(new ProductCartViewModel
            {
                Id = cart.Id,
                ProductName = cart.Product.Name,
                ImagePath = Url.Content(_uploadRepository.GetProductThumbnail(cart.ProductId ?? 0).Path)
            });
        }

        if (model.Products.Any(x => x.Id == id))
        {
        }

        return RedirectToAction("Error", "Home", new ErrorViewModel
        {
            Message = ErrorConstant.GeneralErrors.GetFile
        });
    }

    #endregion
}