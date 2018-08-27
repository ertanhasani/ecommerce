using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? ShippingId { get; set; }
        public bool? Payed { get; set; }
        public int? StatusId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LasUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedOnDate { get; set; }
        public bool? IsDeleted { get; set; }

        public Shipping Shipping { get; set; }
        public Status Status { get; set; }
        public AspNetUsers User { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
