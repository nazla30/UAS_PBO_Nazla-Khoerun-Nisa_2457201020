using BeautyStoreApp.Data;
using BeautyStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautyStoreApp.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================
        public List<Product> GetAll()
        {
            return _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .OrderBy(x => x.Name)
                .ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public Product? GetById(Guid id)
        {
            return _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .FirstOrDefault(x => x.Id == id);
        }

        // =========================
        // CREATE
        // =========================
        public void Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        // =========================
        // UPDATE
        // =========================
        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        // =========================
        // DELETE
        // =========================
        public void Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        // =========================
        // CATEGORY
        // =========================
        public List<Category> GetCategories()
        {
            return _context.Categories
                .OrderBy(x => x.Name)
                .ToList();
        }

        // =========================
        // BRAND
        // =========================
        public List<Brand> GetBrands()
        {
            return _context.Brands
                .OrderBy(x => x.Name)
                .ToList();
        }

        // =========================
        // SEARCH
        // =========================
        public List<Product> Search(string keyword)
        {
            return _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Where(x => x.Name.Contains(keyword))
                .ToList();
        }
    }
}