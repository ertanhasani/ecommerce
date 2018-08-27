using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class Shipping
    {
        public Shipping()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LasUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedOnDate { get; set; }
        public bool? IsDeleted { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
