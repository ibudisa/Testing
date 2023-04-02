using DAL.Entiteti;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Servisi
{
    public class StrojeviServis : IStrojeviServis
    {
        private readonly IDbServis _dbService;

        public StrojeviServis(IDbServis dbService)
        {
            _dbService = dbService;
        }
        public async Task<bool> DodajStroj(Strojevi stroj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Naziv", stroj.Naziv, DbType.String);
            var result =
            await _dbService.UrediPodatke(
                "INSERT INTO public.strojevi (naziv) VALUES (@naziv)",
                parameters);
            return true;
        }

        public async Task<StrojeviPrikaz> DohvatiStroj(int id)
        {
            StrojeviPrikaz result=null;
            var parameters1 = new DynamicParameters();
            parameters1.Add("Id", id, DbType.Int32);
            
            var stroj = await _dbService.Dohvati<Strojevi>("SELECT * FROM public.strojevi where id=@id", parameters1);
            var parameters2 = new DynamicParameters();
            if (stroj != null)
            {
                parameters2.Add("Idstroja", id, DbType.Int32);
                List<Kvarovi> kvarovi = await _dbService.DohvatiSve<Kvarovi>("SELECT * FROM public.kvarovi where idstroja=@idstroja", parameters2);
                stroj.KvaroviLista = kvarovi;

                result = new StrojeviPrikaz(stroj);
            }
            return result;
        }

        public async Task<List<StrojeviPrikaz>> DohvatiStrojeve()
        {
            var strojevi = await _dbService.DohvatiSve<Strojevi>("SELECT * FROM public.strojevi",new { });
            List<Strojevi> li = new List<Strojevi>();
            List<StrojeviPrikaz> lista = new List<StrojeviPrikaz>();
            if (strojevi.Count > 0)
            {
                foreach (var stroj in strojevi)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Idstroja", stroj.Id, DbType.Int32);
                    List<Kvarovi> kvarovi = await _dbService.DohvatiSve<Kvarovi>("SELECT * FROM public.kvarovi where idstroja=@Idstroja", parameters);
                    stroj.KvaroviLista = kvarovi;
                    li.Add(stroj);
                }

               

                foreach (var s in li)
                {
                    StrojeviPrikaz result = new StrojeviPrikaz(s);
                    lista.Add(result);
                }
            }
            return lista;
        }

        public async Task<bool> ObrisiStroj(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            var stroj = await _dbService.UrediPodatke("DELETE FROM public.strojevi WHERE id=@Id", parameters);
            return true;
        }

        public async Task<bool> PromijeniStroj(Strojevi stroj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", stroj.Id, DbType.Int32);
            parameters.Add("Naziv", stroj.Naziv, DbType.String);
            var strojuredi =
            await _dbService.UrediPodatke(
                "Update public.strojevi SET naziv=@Naziv WHERE id=@Id",
                parameters);
            return true;
        }

        public async Task<bool> ProvjeriNaziv(string naziv)
        {   
            bool result = false;
            var strojevi = await _dbService.DohvatiSve<Strojevi>("SELECT * FROM public.strojevi", new { });

            foreach(var s in strojevi)
            {
                if(s.Naziv.Equals(naziv))
                    result = true;
            }
            return result;  
        }
    }
}
