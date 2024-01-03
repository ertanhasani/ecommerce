using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Repositories;
using WebApp.Resources;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUploadRepository _uploadRepository;

    public HomeController(IProductRepository productRepository, IUploadRepository uploadRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _uploadRepository = uploadRepository;
        _categoryRepository = categoryRepository;
    }

    #region Info

    public IActionResult Index()
    {
        var products = _productRepository.GetProducts().Take(12).Select(item =>
        {
            return new ProductListViewModel
            {
                Id = item.Id,
                Url = GeneralConstant.Routes.SlashProductItem + GeneralConstant.Routes.IdParameter + item.Id,
                Title = item.Name,
                IsOnSale = item.IsOnSale != null && item.IsOnSale != false,
                Price = _productRepository.GetCurrentPrice(item.Id).Price1 ?? 0,
                SalePrice = _productRepository.GetSalePrice(item.Id) != null ? _productRepository.GetSalePrice(item.Id).Price1 ?? 0 : 0,
                ImagePath = Url.Content(_uploadRepository.GetProductThumbnail(item.Id)?.Path),
                ProductCategory = _categoryRepository.GetProductCategories(item.Id).Select(x => x.Category.Name).ToList()
            };
        }).ToList();

        var carousels = _uploadRepository.GetCarousels().Select(item => new CarouselViewModel
        {
            Id = item.Id,
            Name = item.FileName,
            ProductId = item.ProductId,
            IsThumbnail = item.IsThumbnail,
            IsView = item.IsView,
            Title = item.Title,
            Description = item.Description,
            ShortDescription = item.ShortDescription,
            PageName = item.PageName,
            SectionName = item.SectionName,
            Link = item.Link,
            Style = item.Style,
            Order = item.Order,
            CreatedByUserId = item.CreatedByUserId,
            CreatedOnDate = item.CreatedOnDate,
            LasUpdatedByUserId = item.LasUpdatedByUserId,
            LastUpdatedOnDate = item.LastUpdatedOnDate,
            IsDeleted = item.IsDeleted,
            Product = item.Product,
            Path = Url.Content(item.Path)
        }).ToList();

        return View(new HomeModel
        {
            Products = products,
            Carousels = carousels
        });
    }

    #endregion

    #region Error

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    #endregion

    #region Access Denied

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult AccessDenied()
    {
        return View();
    }

    #endregion
}