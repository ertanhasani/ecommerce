using eCommerce.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductsController : Controller
    {
        private IUploadRepository _uploadRepository;
        private IHostingEnvironment _env;

        public ProductsController(IUploadRepository uploadRepository, IHostingEnvironment env)
        {
            _uploadRepository = uploadRepository;
            _env = env;
        }

        [Route("{productId}/{uploadId}")]
        public IActionResult DeleteProductUpload(int productId, int uploadId)
        {
            try
            {
                var upload = _uploadRepository.GetUpload(uploadId);
                upload.IsDeleted = true;
                _uploadRepository.EditUpload(upload);
                _uploadRepository.SaveChanges();

                var path = System.IO.Path.Combine(_env.WebRootPath, "uploads", "products", productId.ToString(), upload.FileName);
                System.IO.File.Delete(path);

                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }
        
        //[Route("{searchQuery}")]
        [HttpPost]
        public IActionResult SearchProducts(string searchQuery)
        {
            return RedirectToPage("/Products/Index", new { searchQuery = searchQuery });
        }
    }
}
