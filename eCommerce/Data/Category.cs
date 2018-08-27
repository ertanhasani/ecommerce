using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class Category
    {
        public Category()
        {
            ProductCategory = new HashSet<ProductCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LasUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedOnDate { get; set; }
        public bool? IsDeleted { get; set; }

        public ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
