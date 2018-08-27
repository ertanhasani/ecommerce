using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Repositories;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerce.Pages.Panel
{
    [Authorize(Roles = "Admin")]
    public class CarouselModel : PageModel
    {
        private IUploadRepository _uploadRepository;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _env;

        public List<CarouselViewModel> Carousels { get; set; }

        public CarouselModel(IUploadRepository uploadRepository, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
        {
            _uploadRepository = uploadRepository;
            _userManager = userManager;
            _env = env;
            Carousels = new List<CarouselViewModel>();
        }

        public void OnGet()
        {
            var carousels = _uploadRepository.GetCarousels();
            foreach(var item in carousels)
            {
                Carousels.Add(new CarouselViewModel()
                {
                    Id = item.Id,
                    Name = item.FileName,
                    Path = Url.Content(item.Path)
                });
            }
        }


        public IActionResult OnPostAddUpload(string imageBytes)
        {
            try
            {
                var upload = new Upload();
                upload.IsCarousel = true;
                upload.IsDeleted = false;
                upload.IsThumbnail = false;
                upload.CreatedOnDate = DateTime.Now;
                upload.CreatedByUserId = _userManager.GetUserId(User);
                _uploadRepository.AddUpload(upload);
                _uploadRepository.SaveChanges();

                var path = System.IO.Path.Combine(_env.WebRootPath, "uploads", "carousels");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                var fileName = "Banner" + upload.Id + ".png";

                imageBytes = imageBytes.Remove(0, 22);
                byte[] image = Convert.FromBase64String(imageBytes);

                var imagePath = System.IO.Path.Combine(path, fileName);
                System.IO.File.WriteAllBytes(imagePath, image);

                upload.Path = "~/uploads/carousels/" + fileName;
                upload.FileName = fileName;
                
                _uploadRepository.EditUpload(upload);
                _uploadRepository.SaveChanges();

                return RedirectToPage("/Panel/Carousel");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Panel/Carousel");
            }
        }

        public IActionResult OnGetDeleteImage(int uploadId)
        {
            try
            {
                var upload = _uploadRepository.GetUpload(uploadId);
                upload.IsDeleted = true;
                upload.LasUpdatedByUserId = _userManager.GetUserId(User);
                upload.LastUpdatedOnDate = DateTime.Now;
                _uploadRepository.EditUpload(upload);
                _uploadRepository.SaveChanges();

                var filePath = System.IO.Path.Combine(_env.WebRootPath, "uploads", "carousels", upload.FileName);
                System.IO.File.Delete(filePath);
                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }
    }
}