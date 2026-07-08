using BeautyStoreApp.Services;
using BeautyStoreApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BeautyStoreApp.Documents;
using QuestPDF.Fluent;

namespace BeautyStoreApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly OrderService _service;

        public ReportController(OrderService service)
        {
            _service = service;
        }

        public IActionResult Sales(
            DateTime? startDate,
            DateTime? endDate)
        {
            var data = _service.GetSalesReport(
                startDate,
                endDate);

            var model = new SalesReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Orders = data,
                GrandTotal = data.Sum(x => x.TotalAmount)
            };

            return View(model);
        }
        public IActionResult ExportPdf(DateTime? startDate,
                               DateTime? endDate)
        {
            var data = _service.GetSalesReport(
                startDate,
                endDate);

            var document = new SalesReportDocument(data);

            byte[] pdf = document.GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                "LaporanPenjualan.pdf");
        }
    }
}