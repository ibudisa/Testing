using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Servisi
{
    public interface IDbServis
    {
        Task<T> Dohvati<T>(string command, object parms);
        Task<List<T>> DohvatiSve<T>(string command, object parms);
        Task<int> UrediPodatke(string command, object parms);
    }
}
