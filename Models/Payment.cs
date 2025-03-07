using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(16)]
        public string CardNo { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MaxLength(4)]
        public string CvvNo { get; set; }

        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMode { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
