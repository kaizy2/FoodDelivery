using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order
    {
        [Key]
        public int OrderDetailsId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNo { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public Product Product { get; set; }
        public User User { get; set; }
        public Payment Payment { get; set; }
    }
}
