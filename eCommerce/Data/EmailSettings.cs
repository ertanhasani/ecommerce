using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class EmailSettings
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public int? PortNumber { get; set; }
        public string EmailFrom { get; set; }
        public string Password { get; set; }
        public string EmailTo { get; set; }
    }
}
