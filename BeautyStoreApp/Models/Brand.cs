using System.ComponentModel.DataAnnotations;

namespace BeautyStoreApp.Models
{
    public class Brand
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nama Brand wajib diisi")]
        [Display(Name = "Nama Brand")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Relasi ke Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}