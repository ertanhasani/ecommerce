using WebApp.Data;

namespace WebApp.Repositories;

public class UploadRepository : IUploadRepository
{
    private eCommerceContext _context;

    public UploadRepository(eCommerceContext context)
    {
        _context = context;
    }

    public void AddUpload(Upload upload)
    {
        _context.Upload.Add(upload);
    }

    public void DeleteUpload(Upload upload)
    {
        upload.IsDeleted = true;
        EditUpload(upload);
    }

    public void EditUpload(Upload upload)
    {
        _context.Entry(upload).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    }

    public Upload GetProductThumbnail(int productId)
    {
        return _context.Upload.FirstOrDefault(p => p.ProductId == productId && p.IsThumbnail == true && p.IsDeleted == false);
    }

    public IEnumerable<Upload> GetProductUploads(int productId)
    {
        return _context.Upload.Where(p => p.ProductId == productId && p.IsThumbnail == false && p.IsDeleted == false);
    }

    public Upload GetUpload(int id)
    {
        return GetUploads().FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Upload> GetUploads()
    {
        return _context.Upload;
    }

    public IEnumerable<Upload> GetCarousels()
    {
        return _context.Upload.Where(p => p.IsView == true && p.IsDeleted == false && p.IsThumbnail == false && !p.ProductId.HasValue);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}