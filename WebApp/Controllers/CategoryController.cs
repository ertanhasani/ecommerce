using Microsoft.AspNetCore.Mvc;
using WebApp.Repositories;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryController(ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
    {
        _categoryRepository = categoryRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult List()
    {
        List<TreeViewModel> nodes = new List<TreeViewModel>();

        var list = _categoryRepository.GetCategories().ToList();

        foreach (var item in list.Where(x => x.IsDeleted == false))
        {
            nodes.Add(new TreeViewModel {id = item.Id.ToString(), parent = item.ParentId == null ? "#" : item.ParentId.ToString(), text = item.Name});
        }

        ViewData["Categories"] = new MultiSelectList(list.Where(x => x.IsDeleted == false).Select(p => new
        {
            p.Id,
            p.Name
        }), "Id", "Name", null);

        ViewBag.Json = JsonSerializer.Serialize(nodes);
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        var item = new Category
        {
            ParentId = category.ParentId,
            Name = category.Name,
            CreatedOnDate = DateTime.Now,
            CreatedByUserId = _userManager.GetUserId(User),
            IsDeleted = false
        };

        _categoryRepository.AddCategory(item);
        _categoryRepository.SaveChanges();

        return RedirectToAction("List");
    }

    [HttpPost]
    public IActionResult Delete(string selectedItems)
    {
        var list = JsonSerializer.Deserialize<List<TreeViewModel>>(selectedItems);

        if (list == null)
            return RedirectToAction("List");

        var categories = new List<Category>();

        foreach (var item in list)
        {
            categories.Add(_categoryRepository.GetCategory(Convert.ToInt32(item.id)));
        }

        foreach (var item in categories)
        {
            item.IsDeleted = true;
        }

        _categoryRepository.UpdateCategories(categories);
        _categoryRepository.SaveChanges();

        return RedirectToAction("List");
    }
}