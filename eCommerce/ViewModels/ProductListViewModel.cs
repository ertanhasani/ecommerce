using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.ViewModels
{
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsOnSale { get; set; }
    }
}
