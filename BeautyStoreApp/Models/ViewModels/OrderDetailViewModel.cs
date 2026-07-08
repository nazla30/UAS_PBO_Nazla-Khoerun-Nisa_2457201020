using System.ComponentModel.DataAnnotations;

namespace BeautyStoreApp.ViewModels
{
    public class OrderDetailViewModel
    {
        public Guid OrderHeaderId { get; set; }

        [Required(ErrorMessage = "Produk wajib dipilih")]
        [Display(Name = "Produk")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Qty wajib diisi")]
        [Range(1, 1000)]
        public int Qty { get; set; }
    }
}