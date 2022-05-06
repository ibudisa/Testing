using System;

namespace DAL.Models
{
    public class CleaningPlan
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreationDate { get; set; }

        public string Description { get; set; }
      
    }
}
