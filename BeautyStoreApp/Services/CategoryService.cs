using BeautyStoreApp.Data;
using BeautyStoreApp.Models;

namespace BeautyStoreApp.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll()
        {
            return _context.Categories
                .OrderBy(x => x.Name)
                .ToList();
        }

        public Category? GetById(Guid id)
        {
            return _context.Categories.Find(id);
        }

        public void Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public List<Category> Search(string keyword)
        {
            return _context.Categories
                .Where(x => x.Name.Contains(keyword))
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}