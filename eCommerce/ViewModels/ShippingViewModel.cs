using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.ViewModels
{
    public class ShippingViewModel
    {        
        public string FullName { get; set; }        
        public string Address1 { get; set; }        
        public string Address2 { get; set; }        
        public string City { get; set; }        
        public string State { get; set; }        
        public string ZipCode { get; set; }        
        public string NameOnCard { get; set; }        
        public string CardNumber { get; set; }        
        public string ExpireDate { get; set; }        
        public int? CVV { get; set; }
    }
}
