using BeautyStoreApp.Models;
using BeautyStoreApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyStoreApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        public IActionResult Index(string search)
        {
            var data = string.IsNullOrEmpty(search)
                ? _service.GetAll()
                : _service.Search(search);

            return View(data);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _service.Create(model);

            TempData["Success"] = "Produk berhasil ditambahkan.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            var category = _service.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _service.Update(model);

            TempData["Success"] = "Produk berhasil diperbarui.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            var category = _service.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.Delete(id);

            TempData["Success"] = "Produk berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdown()
        {
            var categories = _service.GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToList();

            ViewBag.Categories = categories;
        }
        
    }
    
}
