using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class ProductCategory
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }

        public Category Category { get; set; }
        public Product Product { get; set; }
    }
}
