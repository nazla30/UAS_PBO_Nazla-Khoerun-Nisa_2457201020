using BeautyStoreApp.Data;
using BeautyStoreApp.Models;
using BeautyStoreApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BeautyStoreApp.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ReportViewModel GetReport(DateTime? start, DateTime? end)
        {
            var query = _context.OrderHeaders
                .Include(x => x.Customer)
                .Where(x => x.Status == "Completed")
                .AsQueryable();

            if (start.HasValue)
                query = query.Where(x => x.OrderDate >= start.Value);

            if (end.HasValue)
                query = query.Where(x => x.OrderDate <= end.Value);

            var orders = query
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return new ReportViewModel
            {
                StartDate = start,
                EndDate = end,
                Orders = orders,
                TotalOrder = orders.Count,
                TotalRevenue = orders.Sum(x => x.TotalAmount)
            };
        }
    }
}