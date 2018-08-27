using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.ViewModels
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public int Orders { get; set; }
        public bool CurrentCart { get; set; }
    }
}
