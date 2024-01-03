using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.Repositories;
using WebApp.Resources;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Obsolete("Obsolete")]
[Route(GeneralConstant.Routes.ControllerDefault)]
public class ProductsController : Controller
{
    private readonly IUploadRepository _uploadRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IHostingEnvironment _env;
    private readonly IGeneralServices _generalServices;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProductsController(UserManager<ApplicationUser> userManager, IGeneralServices generalServices, IProductRepository productRepository, IUploadRepository uploadRepository, ICategoryRepository categoryRepository, IHostingEnvironment env)
    {
        _uploadRepository = uploadRepository;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
        _generalServices = generalServices;
        _env = env;
    }

    #region Info

    [HttpGet]
    public IActionResult Index(int? id, string searchQuery = null)
    {
        var model = new AllProductViewModel();

        var q = _productRepository.GetProducts().AsQueryable();

        if (id != null)
        {
            var category = _categoryRepository.GetCategory((int) id);

            model.Category = category;

            if (category != null)

                q = q.Where(x => x.ProductCategory.Any(z => z.CategoryId == category.Id));
        }

        if (!string.IsNullOrEmpty(searchQuery))
        {
            model.SearchQuery = searchQuery;

            q = q.Where(x => x.Name.Contains(searchQuery));
        }

        model.ProductList = q.Select(p => new ProductListViewModel
        {
            Id = p.Id,
            Url = GeneralConstant.Routes.SlashProductItem + GeneralConstant.Routes.IdParameter + p.Id,
            Title = p.Name,
            IsOnSale = p.IsOnSale != null && p.IsOnSale != false,
            Price = _productRepository.GetCurrentPrice(p.Id).Price1 ?? 0,
            SalePrice = _productRepository.GetSalePrice(p.Id) != null ? _productRepository.GetSalePrice(p.Id).Price1 ?? 0 : 0,
            ImagePath = Url.Content(_uploadRepository.GetProductThumbnail(p.Id).Path),
        }).ToList();

        foreach (var item in model.ProductList)
        {
            item.ProductCategory = _categoryRepository.GetProductCategories(item.Id).Select(x => x.Category.Name).ToList();
        }

        return View(model);
    }

    [Route("{productId}/{uploadId}")]
    public IActionResult DeleteProductUpload(int productId, int uploadId)
    {
        try
        {
            var upload = _uploadRepository.GetUpload(uploadId);
            upload.IsDeleted = true;
            _uploadRepository.EditUpload(upload);
            _uploadRepository.SaveChanges();

            var path = Path.Combine(_env.WebRootPath, "uploads", "products", productId.ToString(), upload.FileName);
            System.IO.File.Delete(path);

            return Ok(true);
        }
        catch (Exception)
        {
            return Ok(false);
        }
    }

    [HttpPost]
    public IActionResult SearchProducts(string searchQuery)
    {
        return RedirectToAction("Index", new {searchQuery});
    }

    #endregion

    #region Add

    [HttpGet]
    public IActionResult Add(int id)
    {
        var model = new ProductsViewModel();

        if (id != 0)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            model.Id = id;
            model.Name = product.Name;
            model.Description = product.Description;
            model.Price = _productRepository.GetCurrentPrice(id).Price1;
            model.IsOnSale = product.IsOnSale ?? false;
            model.IsDigit = product.IsDigit;
            model.SalePrice = _productRepository.GetSalePrice(id) == null ? null : _productRepository.GetSalePrice(id).Price1;
            model.Quantity = product.Quantity ?? 0;

            model.ThumbnailUrl = Url.Content(_uploadRepository.GetProductThumbnail(id).Path);

            var categories = _categoryRepository.GetCategories().Where(x => x.IsDeleted == false);
            var selectedCategories = _categoryRepository.GetProductCategories(id).Select(p => new
            {
                p.Category.Id,
                p.Category.Name
            }).ToList();
            model.Categories = selectedCategories.Select(p => p.Id).ToArray();
            ViewData["Categories"] = new MultiSelectList(categories.Select(p => new
            {
                p.Id,
                p.Name
            }), "Id", "Name", selectedCategories);
        }
        else
        {
            model.Id = id;
            var categories = _categoryRepository.GetCategories().Where(x => x.IsDeleted == false);
            ViewData["Categories"] = new MultiSelectList(categories.Select(p => new
            {
                p.Id,
                p.Name
            }), "Id", "Name");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(List<IFormFile> uploads, ProductsViewModel model, string title, string description, string shortDescription, string pageName, string sectionName, string className, int order)
    {
        if (model.Id != 0)
        {
            var product = _productRepository.GetProduct(model.Id);
            product.Name = model.Name;
            product.Description = model.Description;
            product.Quantity = model.Quantity;
            product.IsOnSale = model.IsOnSale;
            product.IsDigit = model.IsDigit;
            product.IsDeleted = false;
            product.LasUpdatedByUserId = _userManager.GetUserId(User);
            product.LastUpdatedOnDate = DateTime.Now;

            _productRepository.EditProduct(product);

            var price = _productRepository.GetCurrentPrice(product.Id);
            price.IsActive = true;
            price.IsSale = false;
            price.Price1 = model.Price;

            var oldPrice = _productRepository.GetSalePrice(product.Id);
            if (model.IsOnSale)
            {
                if (oldPrice != null)
                {
                    oldPrice.IsActive = true;
                    oldPrice.IsSale = true;
                    oldPrice.Price1 = model.SalePrice;
                    _productRepository.EditProductPrice(oldPrice);
                }
                else
                {
                    var salePrice = new Price();
                    price.IsSale = true;
                    price.Price1 = model.SalePrice;
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
            _generalServices.AddCategories(model.Categories, model.Id, true);

            if (!String.IsNullOrEmpty(model.ThumbnailString))
            {
                AddUpload(model.ThumbnailString, product.Id, true, title, description, shortDescription, pageName, sectionName, className, order);
            }

            if (uploads.Count != 0)
            {
                await SaveUploads(uploads, product.Id);
            }
        }
        else
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Quantity = model.Quantity,
                IsOnSale = model.IsOnSale,
                IsDigit = model.IsDigit,
                IsDeleted = false,
                CreatedByUserId = _userManager.GetUserId(User),
                CreatedOnDate = DateTime.Now
            };

            _productRepository.AddProduct(product);
            _productRepository.SaveChanges();

            var price = new Price
            {
                IsSale = false,
                Price1 = model.Price,
                ProductId = product.Id,
                IsActive = true
            };

            _productRepository.AddPriceToProduct(price);
            _productRepository.SaveChanges();

            if (model.IsOnSale)
            {
                var salePrice = new Price
                {
                    IsSale = true,
                    Price1 = model.SalePrice,
                    ProductId = product.Id,
                    IsActive = true
                };
                _productRepository.AddPriceToProduct(salePrice);
                _productRepository.SaveChanges();
            }

            _generalServices.AddCategories(model.Categories, product.Id, false);
            AddUpload(model.ThumbnailString, product.Id, false, title, description, shortDescription, pageName, sectionName, className, order);
            await SaveUploads(uploads, product.Id);
        }

        return RedirectToAction("Index");
    }

    #endregion

    #region Upload

    [HttpGet]
    public IActionResult Uploads(int productIda)
    {
        var model = new List<UploadViewModel>();
        var uploads = _uploadRepository.GetProductUploads(productIda);

        foreach (var item in uploads)
        {
            model.Add(new UploadViewModel()
            {
                Id = item.Id,
                FileName = item.FileName,
                Path = Url.Content(item.Path)
            });
        }

        return new JsonResult(model);
    }

    public void AddUpload(string imageBytes, int productId, bool isEditing, string title, string description, string shortDescription, string pageName, string sectionName, string style, int order)
    {
        var path = Path.Combine(_env.WebRootPath, "uploads", "products", productId.ToString());
        if (isEditing)
        {
            var currentThumbnail = _uploadRepository.GetProductThumbnail(productId);
            if (currentThumbnail != null)
            {
                currentThumbnail.IsDeleted = true;
                _uploadRepository.EditUpload(currentThumbnail);
                _uploadRepository.SaveChanges();
                System.IO.File.Delete(Path.Combine(path, "thumbnail.png"));
            }
        }

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        imageBytes = imageBytes.Remove(0, 22);
        byte[] image = Convert.FromBase64String(imageBytes);
        var imagePath = Path.Combine(path, "thumbnail.png");
        System.IO.File.WriteAllBytes(imagePath, image);
        var userId = _userManager.GetUserId(User);
        var upload = new Upload
        {
            Id = 0,
            Path = "~/uploads/products/" + productId + "/thumbnail.png",
            FileName = "thumbnail.png",
            ProductId = productId,
            CreatedOnDate = DateTime.Now,
            CreatedByUserId = userId,
            IsThumbnail = true,
            IsView = false,
            Title = title,
            Description = description,
            ShortDescription = shortDescription,
            PageName = pageName,
            SectionName = sectionName,
            Style = style,
            Order = order,
            IsDeleted = false,
        };

        _uploadRepository.AddUpload(upload);
        _uploadRepository.SaveChanges();
    }

    public async Task SaveUploads(List<IFormFile> uploads, int productId)
    {
        var filePath = Path.Combine(_env.WebRootPath, "uploads", "products", productId.ToString());

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        foreach (var file in uploads)
        {
            if (file.Length > 0)
            {
                var name = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                string time = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds).ToString();

                var fileName = name + "_" + time + extension;

                await using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var upload = new Upload()
                {
                    Path = "~/uploads/products/" + productId + "/" + fileName,
                    FileName = fileName,
                    ProductId = productId,
                    CreatedOnDate = DateTime.Now,
                    CreatedByUserId = _userManager.GetUserId(User),
                    IsView = false,
                    IsThumbnail = false,
                    IsDeleted = false
                };
                _uploadRepository.AddUpload(upload);
            }
        }

        _uploadRepository.SaveChanges();
    }

    #endregion
}