using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.ViewModels
{
    public class OrdersViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string OrderedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string UsersFullName { get; set; }
        public string UsersEmail { get; set; }
        public string UserId { get; set; }
    }
}
