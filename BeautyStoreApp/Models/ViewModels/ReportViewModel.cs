using BeautyStoreApp.Models;

namespace BeautyStoreApp.ViewModels
{
    public class ReportViewModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<OrderHeader> Orders { get; set; } = new();

        public decimal TotalRevenue { get; set; }

        public int TotalOrder { get; set; }
    }
}