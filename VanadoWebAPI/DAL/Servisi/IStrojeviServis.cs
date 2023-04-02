using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entiteti;

namespace DAL.Servisi
{
    public interface IStrojeviServis
    {
        Task<bool> DodajStroj(Strojevi stroj);
        Task<List<StrojeviPrikaz>> DohvatiStrojeve();
        Task<StrojeviPrikaz> DohvatiStroj(int id);
        Task<bool> PromijeniStroj(Strojevi stroj);
        Task<bool> ObrisiStroj(int id);
        Task<bool> ProvjeriNaziv(string naziv);
    }
}
