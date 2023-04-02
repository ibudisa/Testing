using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entiteti
{
    public class Strojevi
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public List<Kvarovi> KvaroviLista { get; set; } = new List<Kvarovi>();



      
    }
}
