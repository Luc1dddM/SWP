using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryServices _categoryService;


        public CategoryController(CategoryServices categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult CategoryAdd() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryAdd(string name, string type, string status)
        {
            try
            {
                Category service = new Category()
                {
                    category_name = name,
                    category_type = type,
                    active = status == "Active" ? true:false
                };
                _categoryService.createCategory(service);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return Redirect("Categories");
        }

        public IActionResult CategoryEdit(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public IActionResult CategoryEdit(string id, string name, string type, string status)
        {
            try
            {
                Category service = new Category()
                {
                    category_id = id,
                    category_name = name,
                    category_type = type,
                    active = status == "Active" ? true : false
                };
                _categoryService.editCategory(service);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return Redirect("Categories");
        }
    }
}
