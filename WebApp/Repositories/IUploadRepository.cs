using WebApp.Data;

namespace WebApp.Repositories;

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