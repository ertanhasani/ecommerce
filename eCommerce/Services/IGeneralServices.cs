using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public interface IGeneralServices
    {
        void AddCategories(int[] categories, int productId, bool isEditing);
    }
}
