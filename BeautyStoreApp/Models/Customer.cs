using System.ComponentModel.DataAnnotations;

namespace BeautyStoreApp.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nama wajib diisi")]
        [StringLength(100)]
        [Display(Name = "Nama Customer")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Nomor HP")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        [Display(Name = "Alamat")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Kota")]
        public string City { get; set; } = string.Empty;

        [StringLength(10)]
        [Display(Name = "Kode Pos")]
        public string? PostalCode { get; set; }
    }
}