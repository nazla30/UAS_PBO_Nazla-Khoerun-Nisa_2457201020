using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyStoreApp.Models
{
    public class OrderHeader
    {
        [Key]
        public Guid Id { get; set; }

        public string OrderNumber { get; set; } = "";

        public DateTime OrderDate { get; set; }

        public Guid CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        // Navigation Property
        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();
    }
}