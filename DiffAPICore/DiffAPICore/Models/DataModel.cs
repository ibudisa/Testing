using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DiffAPICore.Models
{

    // represents data sent to controller
    public class DataModel
    {
        [Required]
        public string Data { get; set; }
    }
}
