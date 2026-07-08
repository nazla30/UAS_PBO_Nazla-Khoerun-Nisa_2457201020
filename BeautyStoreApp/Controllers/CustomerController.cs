using BeautyStoreApp.Models;
using BeautyStoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeautyStoreApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        // INDEX
        public IActionResult Index(string search)
        {
            var data = string.IsNullOrWhiteSpace(search)
                ? _service.GetAll()
                : _service.Search(search);

            return View(data);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Create(model);

            TempData["Success"] = "Customer berhasil ditambahkan.";

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public IActionResult Edit(Guid id)
        {
            var customer = _service.GetById(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Update(model);

            TempData["Success"] = "Customer berhasil diperbarui.";

            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET)
        public IActionResult Delete(Guid id)
        {
            var customer = _service.GetById(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.Delete(id);

            TempData["Success"] = "Customer berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }
    }
}