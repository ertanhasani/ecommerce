using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Repositories;
using WebApp.Resources;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(Roles = GeneralConstant.Roles.Admin)]
[Route(GeneralConstant.Routes.ControllerDefault)]
[Obsolete("Obsolete")]
public class PanelController : Controller
{
    private readonly IUploadRepository _uploadRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHostingEnvironment _env;
    private readonly IPanelRepository _panelRepository;
    private readonly IProductRepository _productRepository;

    public PanelController(IProductRepository productRepository, IPanelRepository panelRepository, IUploadRepository uploadRepository, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
    {
        _uploadRepository = uploadRepository;
        _userManager = userManager;
        _env = env;
        _panelRepository = panelRepository;
        _productRepository = productRepository;
    }

    #region Carousel

    [HttpGet]
    public IActionResult Carousel()
    {
        var carousels = _uploadRepository.GetCarousels();

        var model = new List<CarouselViewModel>();

        foreach (var item in carousels)
        {
            model.Add(new CarouselViewModel
            {
                Id = item.Id,
                Name = item.FileName,
                ProductId = null,
                IsThumbnail = null,
                IsView = false,
                Title = item.Title,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                PageName = item.PageName,
                SectionName = item.SectionName,
                Link = item.Link,
                Style = item.Style,
                Order = item.Order,
                Path = Url.Content(item.Path)
            });
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult CarouselItem(int id)
    {
        var carousels = _uploadRepository.GetCarousels().FirstOrDefault(x => x.Id == id);


        var model = new CarouselViewModel
        {
            Id = carousels!.Id,
            Name = carousels.FileName,
            ProductId = null,
            IsThumbnail = carousels.IsThumbnail,
            IsView = false,
            Title = carousels.Title,
            Description = carousels.Description,
            ShortDescription = carousels.ShortDescription,
            PageName = carousels.PageName,
            SectionName = carousels.SectionName,
            Link = carousels.Link,
            Style = carousels.Style,
            Order = carousels.Order,
            Path = Url.Content(carousels.Path)
        };


        return View(model);
    }

    [HttpPost]
    public IActionResult AddUpload(string imageBytes, string name, string title, string description, string shortDescription, string pageName, string sectionName, string link, int order, string style)
    {
        try
        {
            var upload = new Upload
            {
                Id = 0,
                FileName = name,
                IsView = true,
                Title = title,
                Description = description,
                ShortDescription = shortDescription,
                PageName = pageName,
                SectionName = sectionName,
                Link = link,
                Style = style,
                Order = order,
                IsThumbnail = false,
                IsDeleted = false,
                CreatedOnDate = DateTime.Now,
                CreatedByUserId = _userManager.GetUserId(User)
            };

            _uploadRepository.AddUpload(upload);

            _uploadRepository.SaveChanges();

            var path = Path.Combine(_env.WebRootPath, "uploads", "carousels");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = "Banner" + upload.Id + ".png";

            imageBytes = imageBytes.Remove(0, 22);

            var image = Convert.FromBase64String(imageBytes);

            var imagePath = Path.Combine(path, fileName);

            System.IO.File.WriteAllBytes(imagePath, image);

            upload.Path = "~/uploads/carousels/" + fileName;

            upload.FileName = fileName;

            _uploadRepository.EditUpload(upload);

            _uploadRepository.SaveChanges();

            return RedirectToAction("Carousel");
        }
        catch (Exception)
        {
            return View("Error", new ErrorViewModel
            {
                Message = "UploadError"
            });
        }
    }

    public IActionResult UpdateUpload(int id, string imageBytes, string name, string title, string description, string shortDescription, string pageName, string sectionName, string link, int order, string style)
    {
        try
        {
            var upload = new Upload
            {
                Id = id,
                FileName = name,
                IsView = true,
                Title = title,
                Description = description,
                ShortDescription = shortDescription,
                PageName = pageName,
                SectionName = sectionName,
                Link = link,
                Style = style,
                Order = order,
                IsThumbnail = false,
                IsDeleted = false,
                CreatedOnDate = DateTime.Now,
                CreatedByUserId = _userManager.GetUserId(User)
            };

            _uploadRepository.EditUpload(upload);

            _uploadRepository.SaveChanges();

            var path = Path.Combine(_env.WebRootPath, "uploads", "carousels");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = "Banner" + upload.Id + ".png";

            imageBytes = imageBytes.Remove(0, 22);

            var image = Convert.FromBase64String(imageBytes);

            var imagePath = Path.Combine(path, fileName);

            System.IO.File.WriteAllBytes(imagePath, image);

            upload.Path = "~/uploads/carousels/" + fileName;

            upload.FileName = fileName;

            _uploadRepository.EditUpload(upload);

            _uploadRepository.SaveChanges();

            return RedirectToAction("Carousel");
        }
        catch (Exception)
        {
            return View("Error", new ErrorViewModel
            {
                Message = "UploadError"
            });
        }
    }

    [HttpPost]
    public IActionResult DeleteImage(int uploadId)
    {
        try
        {
            var upload = _uploadRepository.GetUpload(uploadId);

            upload.IsDeleted = true;

            upload.LasUpdatedByUserId = _userManager.GetUserId(User);

            upload.LastUpdatedOnDate = DateTime.Now;

            _uploadRepository.EditUpload(upload);

            _uploadRepository.SaveChanges();

            var filePath = Path.Combine(_env.WebRootPath, "uploads", "carousels", upload.FileName);

            System.IO.File.Delete(filePath);

            return new JsonResult(true);
        }
        catch (Exception)
        {
            return new JsonResult(false);
        }
    }

    #endregion

    #region Orders

    [HttpGet]
    public IActionResult Orders()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Orders(string search, int pageNr)
    {
        var model = new List<OrdersViewModel>();
        var orders = _panelRepository.GetOrders().Where(p => p.StatusId == 1);

        var enumerable = orders.ToList();
        decimal pagesInDecimal = (decimal) enumerable.Count() / 20;
        var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int) pagesInDecimal + 1;

        if (!String.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            enumerable.Where(p => p.Id.ToString().Contains(search) || p.Status.Name.ToLower().Contains(search) || (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).Contains(search) || p.User.Email.ToLower().Contains(search));
        }

        orders = enumerable.OrderByDescending(p => p.LastUpdatedOnDate).Skip((pageNr - 1) * 20).Take(20);
        foreach (var item in orders)
        {
            model.Add(new OrdersViewModel()
            {
                Id = item.Id,
                OrderedDate = item.LastUpdatedOnDate!.Value.ToString("dd.MM.yyyy"),
                Status = item.Status.Name,
                TotalPrice = (decimal) item.TotalPrice!,
                UsersFullName = item.User.FirstName + " " + item.User.LastName,
                UsersEmail = item.User.Email
            });
        }

        return new JsonResult(new
        {
            totalPages = pages,
            orders = model
        });
    }

    [HttpPost]
    public IActionResult ProcessingOrders(string search, int pageNr)
    {
        var orders = _panelRepository.GetOrders().Where(p => p.StatusId == 2).ToList();

        var pagesInDecimal = (decimal) orders.Count / 20;

        var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int) pagesInDecimal + 1;

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();

            orders = orders.Where(p => p.Id.ToString().Contains(search) || p.Status.Name.ToLower().Contains(search) || (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).Contains(search) || p.User.Email.ToLower().Contains(search)).ToList();
        }

        orders = orders.OrderByDescending(p => p.LastUpdatedOnDate).Skip((pageNr - 1) * 20).Take(20).ToList();

        var model = orders.Select(item => new OrdersViewModel()
            {
                Id = item.Id,
                OrderedDate = item.LastUpdatedOnDate?.ToString("yyyy.MM.dd"),
                Status = item.Status.Name,
                TotalPrice = item.TotalPrice ?? 0,
                UsersFullName = item.User.FirstName + " " + item.User.LastName,
                UsersEmail = item.User.Email
            })
            .ToList();

        return new JsonResult(new
        {
            totalPages = pages,
            orders = model
        });
    }

    [HttpPost]
    public IActionResult ShippedOrders(string search, int pageNr)
    {
        var orders = _panelRepository.GetOrders().Where(p => p.StatusId == 3).ToList();

        var pagesInDecimal = (decimal) orders.Count / 20;
        var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int) pagesInDecimal + 1;

        if (!String.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            orders = orders.Where(p => p.Id.ToString().Contains(search) || p.Status.Name.ToLower().Contains(search) || (p.User.FirstName.ToLower() + " " + p.User.LastName.ToLower()).Contains(search) || p.User.Email.ToLower().Contains(search)).ToList();
        }

        orders = orders.OrderByDescending(p => p.LastUpdatedOnDate).Skip((pageNr - 1) * 20).Take(20).ToList();

        var model = orders.Select(item => new OrdersViewModel()
            {
                Id = item.Id,
                OrderedDate = item.LastUpdatedOnDate?.ToString("yyyy.MM.dd"),
                Status = item.Status.Name,
                TotalPrice = item.TotalPrice ?? 0,
                UsersFullName = item.User.FirstName + " " + item.User.LastName,
                UsersEmail = item.User.Email
            })
            .ToList();

        return new JsonResult(new
        {
            totalPages = pages,
            orders = model
        });
    }

    #endregion

    #region Statistics

    [HttpGet]
    public IActionResult Statistics()
    {
        var model = new StatisticsViewModel
        {
            Users = new List<UsersViewModel>(),
            Products = new List<ProductCartViewModel>(),
            CostOrders = new List<OrdersViewModel>(),
            ProductOrders = new List<OrdersViewModel>()
        };

        var users = _panelRepository.GetTopUsers().Take(10);

        foreach (var item in users)
        {
            model.Users.Add(new UsersViewModel()
            {
                Id = item.Id,
                Email = item.Email,
                FullName = item.FirstName + " " + item.LastName,
                Orders = _panelRepository.HowManyOrder(item.Id)
            });
        }

        var products = _panelRepository.GetTopProducts().Take(10);
        foreach (var item in products)
        {
            model.Products.Add(new ProductCartViewModel()
            {
                Id = item.Id,
                ProductName = item.Name,
                Price = _productRepository.GetCurrentPrice(item.Id).Price1 ?? 0,
                Quantity = item.Quantity ?? 0,
                Sold = _panelRepository.GetProductSales(item.Id)
            });
        }

        var costOrders = _panelRepository.GetTopCostOrders().Take(10);
        foreach (var item in costOrders)
        {
            model.CostOrders.Add(new OrdersViewModel()
            {
                Id = item.Id,
                OrderedDate = item.LastUpdatedOnDate?.ToString("yyyy.MM.dd"),
                UsersFullName = item.User.FirstName + " " + item.User.LastName,
                UserId = item.User.Id,
                Status = item.Status.Name,
                TotalPrice = item.TotalPrice ?? 0
            });
        }

        var productOrders = _panelRepository.GetTopProductOrders().Take(10);
        foreach (var item in productOrders)
        {
            model.ProductOrders.Add(new OrdersViewModel()
            {
                Id = item.Id,
                OrderedDate = item.LastUpdatedOnDate?.ToString("yyyy.MM.dd"),
                UsersFullName = item.User.FirstName + " " + item.User.LastName,
                UserId = item.User.Id,
                Status = item.Status.Name,
                TotalPrice = item.TotalPrice ?? 0
            });
        }

        return View(model);
    }

    #endregion

    #region Users

    [HttpGet]
    public IActionResult Users()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Users(string search, int pageNr)
    {
        var users = _panelRepository.GetUsers().ToList();

        var pagesInDecimal = (decimal) users.Count / 20;
        var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int) pagesInDecimal + 1;

        if (!String.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            users = users.Where(p => p.Id.ToLower().Contains(search) || p.Email.ToLower().Contains(search) || (p.FirstName + " " + p.LastName).ToLower().Contains(search)).ToList();
        }

        users = users.OrderByDescending(p => p.CreatedOnDate).Skip((pageNr - 1) * 20).Take(20).ToList();

        var list = users.Select(item => new UsersViewModel()
            {
                Id = item.Id,
                FullName = item.FirstName + " " + item.LastName,
                Email = item.Email,
                CurrentCart = _panelRepository.HasOpenCart(item.Id),
                Orders = _panelRepository.HowManyOrder(item.Id),
                BirthDate = item.BirthDate.ToString("yyyy.MM.dd")
            })
            .ToList();

        return new JsonResult(new
        {
            totalPages = pages,
            users = list
        });
    }

    [HttpPost]
    public IActionResult AllUsers(string search, int pageNr)
    {
        var users = _panelRepository.GetAllUsers().ToList();

        var pagesInDecimal = (decimal) users.Count / 20;
        var pages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int) pagesInDecimal + 1;

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            users = users.Where(p => p.Id.ToLower().Contains(search) || p.Email.ToLower().Contains(search) || (p.FirstName + " " + p.LastName).ToLower().Contains(search)).ToList();
        }

        users = users.OrderByDescending(p => p.CreatedOnDate).Skip((pageNr - 1) * 20).Take(20).ToList();

        var list = users.Select(item => new UsersViewModel
            {
                Id = item.Id,
                UserName = item.UserName,
                FullName = item.FirstName + " " + item.LastName,
                Email = item.Email,
                CurrentCart = _panelRepository.HasOpenCart(item.Id),
                Orders = _panelRepository.HowManyOrder(item.Id),
                BirthDate = item.BirthDate.ToString("yyyy.MM.dd")
            })
            .ToList();

        return new JsonResult(new
        {
            totalPages = pages,
            users = list
        });
    }

    #endregion
}