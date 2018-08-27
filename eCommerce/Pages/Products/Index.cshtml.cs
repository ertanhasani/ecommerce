using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Repositories;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages.Products
{
    public class IndexModel : PageModel
    {
        private IProductRepository _productRepository;
        private IUploadRepository _uploadRepository;
        private ICategoryRepository _categoryRepository;

        public int? CategoryId { get; set; }
        public string SearchQuery { get; set; }

        public IndexModel(IProductRepository productRepository, IUploadRepository uploadRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _uploadRepository = uploadRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult OnGet(int? id,string searchQuery, string toastr, bool error = false)
        {
            if(id != null)
            {
                var category = _categoryRepository.GetCategory((int)id);
                if (category == null)
                    return RedirectToPage("/Index");
            }

            if(!String.IsNullOrEmpty(toastr) && !error)
            {
                ViewData["Success"] = toastr;
            }
            if (!String.IsNullOrEmpty(toastr) && error)
            {
                ViewData["Error"] = toastr;
            }

            CategoryId = id;
            if (!String.IsNullOrEmpty(searchQuery))
                SearchQuery = searchQuery;

            return Page();
        }

        public IActionResult OnGetProducts(int pageNr, int categoryId, string searchQuery)
        {
            var model = new List<ProductListViewModel>();
            var products = _productRepository.GetProducts();

            if (!String.IsNullOrEmpty(searchQuery))
                products = products.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()));

            if (categoryId != 0)
                products = products.Where(p => p.ProductCategory.Any(o => o.CategoryId == categoryId));

            decimal pagesInDecimal = (decimal)products.Count() / 20;
            var totalPages = pagesInDecimal % 1 == 0 ? pagesInDecimal : (int)pagesInDecimal + 1;

            products = products.Skip((pageNr - 1) * 20).Take(20);

            foreach (var item in products)
            {
                model.Add(new ProductListViewModel()
                {
                    Id = item.Id,
                    Url = "/Producti/" + item.Id,
                    Title = item.Name,
                    IsOnSale = (bool)item.IsOnSale,
                    Price = (decimal)_productRepository.GetCurrentPrice(item.Id).Price1,
                    SalePrice = _productRepository.GetSalePrice(item.Id) != null ? (decimal)_productRepository.GetSalePrice(item.Id).Price1 : 0.00M,
                    ImagePath = Url.Content(_uploadRepository.GetProductThumbnail(item.Id).Path)
                });
            }

            return new JsonResult(new {
                totalPages = totalPages,
                products = model
            });
        }
    }
}