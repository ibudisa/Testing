using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entiteti;

namespace DAL.Servisi
{
    public interface IKvaroviServis
    {
        Task<bool> DodajKvar(Kvarovi kvar);
        Task<List<Kvarovi>> DohvatiKvarove();
        Task<Kvarovi> DohvatiKvar(int id);
        Task<bool> PromijeniKvar(Kvarovi kvar);
        Task<bool> PromijeniStatusKvara(bool status,int id);
        Task<bool> ObrisiKvar(int id);
        Task<List<Kvarovi>> DohvatiOdredjeneKvarove(int odmak,int broj);
        Task<bool> ProvjeriKvar(int strojid);
    }
}
