using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Customer
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
