using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
