using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Repositories;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages
{
    public class IndexModel : PageModel
    {
        private IProductRepository _productRepository;
        private IUploadRepository _uploadRepository;

        public List<ProductListViewModel> Products { get; set; }
        public List<CarouselViewModel> Carousels { get; set; }

        public IndexModel(IProductRepository productRepository, IUploadRepository uploadRepository)
        {
            _productRepository = productRepository;
            _uploadRepository = uploadRepository;
            Products = new List<ProductListViewModel>();
            Carousels = new List<CarouselViewModel>();
        }

        public void OnGet()
        {
            var products = _productRepository.GetProducts().Take(12);

            foreach(var item in products)
            {
                Products.Add(new ProductListViewModel()
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

            var carousels = _uploadRepository.GetCarousels();
            foreach(var item in carousels)
            {
                Carousels.Add(new CarouselViewModel()
                {
                    Id = item.Id,
                    Name = item.FileName,
                    Path = Url.Content(item.Path)
                });
            }
        }
    }
}
