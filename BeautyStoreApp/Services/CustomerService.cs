using BeautyStoreApp.Data;
using BeautyStoreApp.Models;

namespace BeautyStoreApp.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================
        public List<Customer> GetAll()
        {
            return _context.Customers
                .OrderBy(x => x.FullName)
                .ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public Customer? GetById(Guid id)
        {
            return _context.Customers.Find(id);
        }

        // =========================
        // CREATE
        // =========================
        public void Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        // =========================
        // UPDATE
        // =========================
        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        // =========================
        // DELETE
        // =========================
        public void Delete(Guid id)
        {
            var customer = _context.Customers.Find(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        // =========================
        // SEARCH
        // =========================
        public List<Customer> Search(string keyword)
        {
            return _context.Customers
                .Where(x =>
                    x.FullName.Contains(keyword) ||
                    x.Email.Contains(keyword) ||
                    x.Phone.Contains(keyword))
                .OrderBy(x => x.FullName)
                .ToList();
        }
    }
}