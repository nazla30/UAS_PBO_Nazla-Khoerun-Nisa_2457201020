using BeautyStoreApp.Models;

namespace BeautyStoreApp.ViewModels
{
    public class SalesReportViewModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<OrderHeader> Orders { get; set; }
            = new();

        public decimal GrandTotal { get; set; }
    }
}