using BeautyStoreApp.Documents;
using BeautyStoreApp.Models;
using BeautyStoreApp.Services;
using BeautyStoreApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Fluent;

namespace BeautyStoreApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _service;
        private readonly InvoiceService _invoice;

        public OrderController(
            OrderService service,
            InvoiceService invoice)
        {
            _service = service;
            _invoice = invoice;
        }

        // ==========================
        // LIST ORDER
        // ==========================
        public IActionResult Index()
        {
            return View(_service.GetAllOrders());
        }

        // ==========================
        // CREATE ORDER
        // ==========================
        public IActionResult Create()
        {
            LoadDropdown();

            return View(new OrderHeader
            {
                OrderDate = DateTime.Now,
                Status = "Pending"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderHeader model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdown();
                return View(model);
            }

            model.OrderNumber = GenerateOrderNumber();

            _service.CreateOrder(model);

            TempData["Success"] = "Pesanan berhasil dibuat.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // DETAIL ORDER
        // ==========================
        public IActionResult Detail(Guid id)
        {
            var order = _service.GetOrder(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // ==========================
        // DELETE ORDER
        // ==========================
        public IActionResult Delete(Guid id)
        {
            var order = _service.GetOrder(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.DeleteOrder(id);

            TempData["Success"] = "Pesanan berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // ADD PRODUCT
        // ==========================
        public IActionResult AddProduct(Guid id)
        {
            ViewBag.Products = new SelectList(
                _service.GetProducts(),
                "Id",
                "Name");

            return View(new OrderDetailViewModel
            {
                OrderHeaderId = id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(OrderDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = new SelectList(
                    _service.GetProducts(),
                    "Id",
                    "Name");

                return View(model);
            }

            _service.AddDetail(
                model.OrderHeaderId,
                model.ProductId,
                model.Qty);

            return RedirectToAction(nameof(Detail),
                new { id = model.OrderHeaderId });
        }

        // ==========================
        // EDIT DETAIL
        // ==========================
        public IActionResult EditDetail(Guid id)
        {
            var detail = _service.GetDetail(id);

            if (detail == null)
                return NotFound();

            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDetail(OrderDetail model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.UpdateDetail(model);

            TempData["Success"] = "Produk berhasil diperbarui.";

            return RedirectToAction(nameof(Detail),
                new { id = model.OrderHeaderId });
        }

        // ==========================
        // DELETE DETAIL
        // ==========================
        public IActionResult DeleteDetail(Guid id)
        {
            var detail = _service.GetDetail(id);

            if (detail == null)
                return NotFound();

            return View(detail);
        }

        [HttpPost, ActionName("DeleteDetail")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDetailConfirmed(Guid id)
        {
            var detail = _service.GetDetail(id);

            if (detail == null)
                return RedirectToAction(nameof(Index));

            Guid orderId = detail.OrderHeaderId;

            _service.DeleteDetail(id);

            return RedirectToAction(nameof(Detail),
                new { id = orderId });
        }

        // ==========================
        // COMPLETE ORDER
        // ==========================
        public IActionResult Complete(Guid id)
        {
            var order = _service.GetOrder(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Complete(OrderHeader model)
        {
            bool success = _service.CompleteOrder(model.Id);

            if (!success)
            {
                TempData["Error"] =
                    "Stok produk tidak mencukupi atau order sudah selesai.";

                return RedirectToAction(nameof(Detail),
                    new { id = model.Id });
            }

            TempData["Success"] =
                "Order berhasil diselesaikan.";

            return RedirectToAction(nameof(Detail),
                new { id = model.Id });
        }

        // ==========================
        // INVOICE
        // ==========================
        public IActionResult Invoice(Guid id)
        {
            var order = _invoice.GetInvoice(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // ==========================
        // DOWNLOAD PDF
        // ==========================
        public IActionResult DownloadInvoice(Guid id)
        {
            var order = _invoice.GetInvoice(id);

            if (order == null)
                return NotFound();

            var document = new InvoiceDocument(order);

            byte[] pdf = document.GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Invoice-{order.OrderNumber}.pdf");
        }

        // ==========================
        // DROPDOWN CUSTOMER
        // ==========================
        private void LoadDropdown()
        {
            ViewBag.Customers = new SelectList(
                _service.GetCustomers(),
                "Id",
                "FullName");
        }

        // ==========================
        // GENERATE ORDER NUMBER
        // ==========================
        private string GenerateOrderNumber()
        {
            return "ORD-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}