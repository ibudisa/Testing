using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entiteti
{
    public class Kvarovi
    {
        public int Id { get; set; }
        public int Idstroja { get; set; }   
        public string Naziv { get; set; }   
        public string Prioritet { get; set; }
        public DateTime Vrijemepocetka { get; set; }
        public DateTime Vrijemezavrsetka { get; set; }
        public string Opis { get; set; }

        public bool Status { get; set; }

    }
}
