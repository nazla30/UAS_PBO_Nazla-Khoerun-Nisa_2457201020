using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyStoreApp.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        public Brand? Brand { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public int Weight { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}