using eCommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Repositories
{
    public interface IUploadRepository
    {
        IEnumerable<Upload> GetUploads();
        Upload GetUpload(int id);
        void AddUpload(Upload upload);
        void EditUpload(Upload upload);
        void DeleteUpload(Upload upload);
        Upload GetProductThumbnail(int productId);
        IEnumerable<Upload> GetProductUploads(int productId);
        IEnumerable<Upload> GetCarousels();
        void SaveChanges();
    }
}
