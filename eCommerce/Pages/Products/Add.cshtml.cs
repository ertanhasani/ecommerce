using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Repositories;
using eCommerce.Services;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerce.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class AddModel : PageModel
    {
        private IHostingEnvironment _env;
        private UserManager<ApplicationUser> _userManager;
        private IProductRepository _productRepository;
        private IUploadRepository _uploadRepository;
        private IGeneralServices _generalServices;
        private ICategoryRepository _categoryRepository;

        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public decimal? Price { get; set; }
        [BindProperty]
        public decimal? SalePrice { get; set; }
        [BindProperty]
        public bool IsOnSale { get; set; }
        [BindProperty]
        public string ThumbnailString { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public int[] Categories { get; set; }
        [BindProperty]
        public string ThumbnailUrl { get; set; }

        public AddModel(IHostingEnvironment env, UserManager<ApplicationUser> userManager, IProductRepository productRepository, IUploadRepository uploadRepository, IGeneralServices generalServices, ICategoryRepository categoryRepository)
        {
            _env = env;
            _userManager = userManager;
            _productRepository = productRepository;
            _uploadRepository = uploadRepository;
            _generalServices = generalServices;
            _categoryRepository = categoryRepository;
        }

        public IActionResult OnGet(int id)
        {
            if (id != 0)
            {
                var product = _productRepository.GetProduct(id);
                if (product == null)
                {
                    return RedirectToPage("/Index");
                }

                Id = id;
                Name = product.Name;
                Description = product.Description;
                Price = _productRepository.GetCurrentPrice(id).Price1;
                IsOnSale = (bool)product.IsOnSale;
                SalePrice = _productRepository.GetSalePrice(id) == null ? null : _productRepository.GetSalePrice(id).Price1;
                Quantity = (int)product.Quantity;

                ThumbnailUrl = Url.Content(_uploadRepository.GetProductThumbnail(id).Path);

                var categories = _categoryRepository.GetCategories();
                var selectedCategories = _categoryRepository.GetProductCategories(id).Select(p => new
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                });
                Categories = selectedCategories.Select(p => p.Id).ToArray();
                ViewData["Categories"] = new MultiSelectList(categories.Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name
                }), "Id", "Name", selectedCategories);
            }
            else
            {
                Id = id;
                var categories = _categoryRepository.GetCategories();
                ViewData["Categories"] = new MultiSelectList(categories.Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name
                }), "Id", "Name");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> Uploads)
        {
            var toastr = "";
            if (Id != 0)
            {
                var model = _productRepository.GetProduct(Id);
                model.Name = Name;
                model.Description = Description;
                model.Quantity = Quantity;
                model.IsOnSale = IsOnSale;
                model.IsDeleted = false;
                model.LasUpdatedByUserId = _userManager.GetUserId(User);
                model.LastUpdatedOnDate = DateTime.Now;

                _productRepository.EditProduct(model);

                var price = _productRepository.GetCurrentPrice(model.Id);
                price.IsActive = true;
                price.IsSale = false;
                price.Price1 = Price;

                var oldPrice = _productRepository.GetSalePrice(model.Id);
                if (IsOnSale)
                {
                    if (oldPrice != null)
                    {
                        oldPrice.IsActive = true;
                        oldPrice.IsSale = true;
                        oldPrice.Price1 = SalePrice;
                        _productRepository.EditProductPrice(oldPrice);
                    }
                    else
                    {
                        var salePrice = new Price();
                        price.IsSale = true;
                        price.Price1 = SalePrice;
                        price.ProductId = model.Id;
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
                _generalServices.AddCategories(Categories, Id, true);

                if(!String.IsNullOrEmpty(ThumbnailString))
                {
                    AddUpload(ThumbnailString, model.Id, true);
                }

                if(Uploads.Count != 0)
                {
                    await SaveUploads(Uploads, model.Id);
                }
                toastr = "Product updated successfully.";
            }
            else
            {
                var model = new Product();
                model.Name = Name;
                model.Description = Description;
                model.Quantity = Quantity;
                model.IsOnSale = IsOnSale;
                model.IsDeleted = false;
                model.CreatedByUserId = _userManager.GetUserId(User);
                model.CreatedOnDate = DateTime.Now;

                _productRepository.AddProduct(model);
                _productRepository.SaveChanges();

                var price = new Price();
                price.IsSale = false;
                price.Price1 = Price;
                price.ProductId = model.Id;
                price.IsActive = true;
                _productRepository.AddPriceToProduct(price);
                _productRepository.SaveChanges();

                if (IsOnSale)
                {
                    var salePrice = new Price();
                    salePrice.IsSale = true;
                    salePrice.Price1 = SalePrice;
                    salePrice.ProductId = model.Id;
                    salePrice.IsActive = true;
                    _productRepository.AddPriceToProduct(salePrice);
                    _productRepository.SaveChanges();
                }
                
                _generalServices.AddCategories(Categories, model.Id, false);
                AddUpload(ThumbnailString, model.Id, false);
                await SaveUploads(Uploads, model.Id);
                toastr = "Product added successfully.";
            }

            return RedirectToPage("/Products/Index", new { toastr = toastr });
        }


        public IActionResult OnGetUploads(int productIda)
        {
            var model = new List<UploadViewModel>();
            var uploads = _uploadRepository.GetProductUploads(productIda);

            foreach(var item in uploads)
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

        public void AddUpload(string imageBytes, int productId, bool isEditing)
        {
            try
            {
                var path = System.IO.Path.Combine(_env.WebRootPath, "uploads", "products", productId.ToString());
                if (isEditing)
                {
                    var currentThumbnail = _uploadRepository.GetProductThumbnail(productId);
                    if(currentThumbnail != null)
                    {
                        currentThumbnail.IsDeleted = true;
                        _uploadRepository.EditUpload(currentThumbnail);
                        _uploadRepository.SaveChanges();
                        System.IO.File.Delete(System.IO.Path.Combine(path, "thumbnail.png"));
                    }
                }

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                imageBytes = imageBytes.Remove(0, 22);
                byte[] image = Convert.FromBase64String(imageBytes);
                var imagePath = System.IO.Path.Combine(path, "thumbnail.png");
                System.IO.File.WriteAllBytes(imagePath, image);
                var userId = _userManager.GetUserId(User);
                var upload = new Upload()
                {
                    Path = "~/uploads/products/" + productId + "/thumbnail.png",
                    FileName = "thumbnail.png",
                    ProductId = productId,
                    CreatedOnDate = DateTime.Now,
                    CreatedByUserId = userId,
                    IsThumbnail = true,
                    IsCarousel = false,
                    IsDeleted = false
                };
                _uploadRepository.AddUpload(upload);
                _uploadRepository.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SaveUploads(List<IFormFile> uploads, int productId)
        {
            try
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

                        using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
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
                            IsCarousel = false,
                            IsThumbnail = false,
                            IsDeleted = false
                        };
                        _uploadRepository.AddUpload(upload);
                    }
                }
                _uploadRepository.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
    }
}