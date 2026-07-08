using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyStoreApp.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderHeaderId { get; set; }

        public OrderHeader? OrderHeader { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        public Product? Product { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        public decimal Price { get; set; }

        [NotMapped]
        public decimal SubTotal
        {
            get
            {
                return Qty * Price;
            }
        }
    }
}