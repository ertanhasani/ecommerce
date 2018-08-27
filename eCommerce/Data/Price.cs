using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class Price
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public decimal? Price1 { get; set; }
        public bool? IsSale { get; set; }
        public bool? IsActive { get; set; }

        public Product Product { get; set; }
    }
}
