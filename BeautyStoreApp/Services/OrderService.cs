using BeautyStoreApp.Data;
using BeautyStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautyStoreApp.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================
        // ORDER
        // ===========================

        public List<OrderHeader> GetAllOrders()
        {
            return _context.OrderHeaders
                .Include(x => x.Customer)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }

        public OrderHeader? GetOrder(Guid id)
        {
            return _context.OrderHeaders
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == id);
        }

        public void CreateOrder(OrderHeader order)
        {
            _context.OrderHeaders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(OrderHeader order)
        {
            _context.OrderHeaders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(Guid id)
        {
            var order = _context.OrderHeaders.Find(id);

            if (order == null)
                return;

            _context.OrderHeaders.Remove(order);
            _context.SaveChanges();
        }

        // ===========================
        // CUSTOMER
        // ===========================

        public List<Customer> GetCustomers()
        {
            return _context.Customers
                .OrderBy(x => x.FullName)
                .ToList();
        }

        // ===========================
        // PRODUCT
        // ===========================

        public List<Product> GetProducts()
        {
            return _context.Products
                .OrderBy(x => x.Name)
                .ToList();
        }

        public Product? GetProduct(Guid id)
        {
            return _context.Products.Find(id);
        }

        // ===========================
        // ORDER DETAIL
        // ===========================

        public List<OrderDetail> GetDetails(Guid orderId)
        {
            return _context.OrderDetails
                .Include(x => x.Product)
                .Where(x => x.OrderHeaderId == orderId)
                .ToList();
        }

        public OrderDetail? GetDetail(Guid id)
        {
            return _context.OrderDetails
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == id);
        }

        public void AddDetail(Guid orderId, Guid productId, int qty)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
                return;

            var detail = new OrderDetail
            {
                OrderHeaderId = orderId,
                ProductId = productId,
                Qty = qty,
                Price = product.Price
            };

            _context.OrderDetails.Add(detail);
            _context.SaveChanges();

            UpdateTotal(orderId);
        }

        public void UpdateDetail(OrderDetail detail)
        {
            _context.OrderDetails.Update(detail);
            _context.SaveChanges();

            UpdateTotal(detail.OrderHeaderId);
        }

        public void DeleteDetail(Guid id)
        {
            var detail = _context.OrderDetails.Find(id);

            if (detail == null)
                return;

            Guid orderId = detail.OrderHeaderId;

            _context.OrderDetails.Remove(detail);
            _context.SaveChanges();

            UpdateTotal(orderId);
        }

        // ===========================
        // TOTAL ORDER
        // ===========================

        public decimal CalculateTotal(Guid orderId)
        {
            return _context.OrderDetails
                .Where(x => x.OrderHeaderId == orderId)
                .Sum(x => x.Price * x.Qty);
        }

        public void UpdateTotal(Guid orderId)
        {
            var order = _context.OrderHeaders.Find(orderId);

            if (order == null)
                return;

            order.TotalAmount = CalculateTotal(orderId);

            _context.SaveChanges();
        }

        // ===========================
        // COMPLETE ORDER
        // ===========================

        public bool CompleteOrder(Guid orderId)
        {
            var order = _context.OrderHeaders
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == orderId);

            if (order == null)
                return false;

            // Jangan diproses jika sudah selesai
            if (order.Status == "Completed")
                return false;

            // Validasi stok
            foreach (var item in order.OrderDetails)
            {
                if (item.Product == null)
                    return false;

                if (item.Product.Stock < item.Qty)
                {
                    return false;
                }
            }

            // Kurangi stok
            foreach (var item in order.OrderDetails)
            {
                item.Product!.Stock -= item.Qty;
            }

            // Update status
            order.Status = "Completed";

            _context.SaveChanges();

            return true;
        }

//order header
        public List<OrderHeader> GetSalesReport(
    DateTime? startDate,
    DateTime? endDate)
        {
            var query = _context.OrderHeaders
                .Include(x => x.Customer)
                .Where(x => x.Status == "Completed")
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(x => x.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => x.OrderDate <= endDate.Value);
            }

            return query
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }
    }
}