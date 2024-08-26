using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class UserCity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserCy")]
        [Column(Order = 1)]
        public int UserId { get; set; }

        [ForeignKey("UserCy")]
        [Column(Order = 2)]
        public int CityId { get; set; }
    }
}
