using System.ComponentModel.DataAnnotations;


namespace VanadoWebAPI.Models
{
    public class KvaroviModel
    {
        [Required]
        public int Idstroja { get; set; }
        [Required]
        public string Naziv { get; set; }
        public string Prioritet { get; set; }
        public DateTime Vrijemepocetka { get; set; }
        public DateTime Vrijemezavrsetka { get; set; }
        [Required]
        public string Opis { get; set; }

        public bool Status { get; set; }
    }
}
