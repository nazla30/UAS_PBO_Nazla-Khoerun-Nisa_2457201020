using System.ComponentModel.DataAnnotations;

namespace BeautyStoreApp.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Satu Category memiliki banyak Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}