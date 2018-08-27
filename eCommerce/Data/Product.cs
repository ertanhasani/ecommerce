using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetails>();
            Price = new HashSet<Price>();
            ProductCategory = new HashSet<ProductCategory>();
            Upload = new HashSet<Upload>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public bool? IsOnSale { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LasUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedOnDate { get; set; }
        public bool? IsDeleted { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
        public ICollection<Price> Price { get; set; }
        public ICollection<ProductCategory> ProductCategory { get; set; }
        public ICollection<Upload> Upload { get; set; }
    }
}
