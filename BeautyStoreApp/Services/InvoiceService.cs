using BeautyStoreApp.Data;
using BeautyStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautyStoreApp.Services
{
    public class InvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public OrderHeader? GetInvoice(Guid id)
        {
            return _context.OrderHeaders
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}