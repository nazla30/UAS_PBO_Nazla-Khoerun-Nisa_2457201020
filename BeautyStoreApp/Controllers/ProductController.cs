using BeautyStoreApp.Models;
using BeautyStoreApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyStoreApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _service;
        private readonly IWebHostEnvironment _environment;

        public ProductController(
            ProductService service,
            IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index(string search)
        {
            var data = string.IsNullOrWhiteSpace(search)
                ? _service.GetAll()
                : _service.Search(search);

            return View(data);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            LoadDropdown();
            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdown();
                return View(model);
            }

            if (model.ImageFile != null)
            {
                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(model.ImageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                model.Image = fileName;
            }

            _service.Create(model);

            TempData["Success"] = "Produk berhasil ditambahkan.";

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        public IActionResult Edit(Guid id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            LoadDropdown();

            return View(product);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdown();
                return View(model);
            }

            if (model.ImageFile != null)
            {
                // Hapus gambar lama
                if (!string.IsNullOrEmpty(model.Image))
                {
                    string oldFile = Path.Combine(
                        _environment.WebRootPath,
                        "images",
                        "products",
                        model.Image);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }

                // Upload gambar baru
                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(model.ImageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                model.Image = fileName;
            }

            _service.Update(model);

            TempData["Success"] = "Produk berhasil diperbarui.";

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public IActionResult Delete(Guid id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            // Hapus file gambar
            if (!string.IsNullOrEmpty(product.Image))
            {
                string filePath = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products",
                    product.Image);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _service.Delete(product);

            TempData["Success"] = "Produk berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // LOAD DROPDOWN
        // =========================
        private void LoadDropdown()
        {
            ViewBag.Categories = new SelectList(
                _service.GetCategories(),
                "Id",
                "Name");

            ViewBag.Brands = new SelectList(
                _service.GetBrands(),
                "Id",
                "Name");
        }
    }
}