using BeautyStoreApp.Data;
using BeautyStoreApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BeautyStoreApp.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public DashboardViewModel GetDashboard()
        {
            var months = Enumerable.Range(1, 12)
                .Select(i => new
                {
                    Month = i,
                    Name = new DateTime(DateTime.Now.Year, i, 1).ToString("MMM")
                })
                .ToList();

            var categories = _context.Categories
                .Select(c => new
                {
                    c.Name,
                    Total = c.Products.Count()
                })
                .ToList();

            return new DashboardViewModel
            {
                // Statistik
                TotalBrand = _context.Brands.Count(),

                TotalCategory = _context.Categories.Count(),

                TotalProduct = _context.Products.Count(),

                TotalCustomer = _context.Customers.Count(),

                TotalOrder = _context.OrderHeaders.Count(),

                TotalSales = _context.OrderHeaders
                    .Where(x => x.Status == "Completed")
                    .Sum(x => x.TotalAmount),

                // Produk stok sedikit
                LowStockProducts = _context.Products
                    .Where(x => x.Stock <= 5)
                    .OrderBy(x => x.Stock)
                    .ToList(),

                // Order terbaru
                RecentOrders = _context.OrderHeaders
                    .Include(x => x.Customer)
                    .OrderByDescending(x => x.OrderDate)
                    .Take(5)
                    .ToList(),

                // Grafik Penjualan
                MonthLabels = months
                    .Select(x => x.Name)
                    .ToList(),

                MonthlySales = months
                    .Select(x =>
                        _context.OrderHeaders
                            .Where(o =>
                                o.Status == "Completed" &&
                                o.OrderDate.Year == DateTime.Now.Year &&
                                o.OrderDate.Month == x.Month)
                            .Sum(o => o.TotalAmount))
                    .ToList(),

                // Grafik Kategori
                CategoryLabels = categories
                    .Select(x => x.Name)
                    .ToList(),

                CategoryTotals = categories
                    .Select(x => x.Total)
                    .ToList()
            };
        }
    }
}