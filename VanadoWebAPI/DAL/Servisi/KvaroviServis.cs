using DAL.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using System.Data;

namespace DAL.Servisi
{
    public class KvaroviServis : IKvaroviServis
    {
        private readonly IDbServis _dbService;

        public KvaroviServis(IDbServis dbService)
        {
            _dbService = dbService;
        }
        public async Task<bool> DodajKvar(Kvarovi kvar)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Idstroja", kvar.Idstroja, DbType.Int32);
            parameters.Add("Naziv", kvar.Naziv, DbType.String);
            parameters.Add("Prioritet", kvar.Prioritet, DbType.String);
            var date1 = DateTime.SpecifyKind(kvar.Vrijemepocetka, DateTimeKind.Utc);
            parameters.Add("Vrijemepocetka", date1, DbType.DateTime);
            var date2 = DateTime.SpecifyKind(kvar.Vrijemezavrsetka, DateTimeKind.Utc);
            parameters.Add("Vrijemezavrsetka", date2, DbType.DateTime);
            parameters.Add("Opis", kvar.Opis, DbType.String);
            parameters.Add("Status", kvar.Status, DbType.Boolean);
            var result =
           await _dbService.UrediPodatke(
               "INSERT INTO public.kvarovi (idstroja,naziv,prioritet,vrijemepocetka,vrijemezavrsetka,opis,status) VALUES (@idstroja,@naziv,@prioritet,@vrijemepocetka,@vrijemezavrsetka,@opis,@status)",
               parameters);
            return true;
        }

        public async Task<Kvarovi> DohvatiKvar(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            var kvar = await _dbService.Dohvati<Kvarovi>("SELECT * FROM public.kvarovi where id=@id", parameters);
            return kvar;
        }

        public async Task<List<Kvarovi>> DohvatiKvarove()
        {
            var kvarovi = await _dbService.DohvatiSve<Kvarovi>("SELECT * FROM public.kvarovi", new { });
            return kvarovi;
        }

        public async Task<List<Kvarovi>> DohvatiOdredjeneKvarove(int odmak, int broj)
        {
            var kvarovi = await _dbService.DohvatiSve<Kvarovi>("SELECT * FROM public.kvarovi", new { });
            var lista=new List<Kvarovi>();
            var listaordera = new List<Kvarovi>();
            var listaorderb = new List<Kvarovi>();
            lista =kvarovi.Skip(odmak).Take(broj).ToList();
            listaordera = lista.OrderBy(p => p.Prioritet).ToList();
            listaorderb = listaordera.OrderByDescending(p => p.Vrijemepocetka).ToList();

            return listaorderb;
        }

        public async Task<bool> ObrisiKvar(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            var kvar = await _dbService.UrediPodatke("DELETE FROM public.kvarovi WHERE id=@Id", parameters);
            return true;
        }

        public async Task<bool> PromijeniKvar(Kvarovi kvar)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", kvar.Id, DbType.Int32);
            parameters.Add("Idstroja", kvar.Idstroja, DbType.Int32);
            parameters.Add("Naziv", kvar.Naziv, DbType.String);
            parameters.Add("Prioritet", kvar.Prioritet, DbType.String);
            var date1 = DateTime.SpecifyKind(kvar.Vrijemepocetka, DateTimeKind.Utc);
            parameters.Add("Vrijemepocetka",date1 , DbType.DateTime);
            var date2 = DateTime.SpecifyKind(kvar.Vrijemezavrsetka, DateTimeKind.Utc);
            parameters.Add("Vrijemezavrsetka", date2, DbType.DateTime);
            parameters.Add("Opis", kvar.Opis, DbType.String);
            parameters.Add("Status", kvar.Status, DbType.Boolean);
            var kvaruredi =
            await _dbService.UrediPodatke(
                "Update public.kvarovi SET idstroja=@Idstroja, naziv=@Naziv,prioritet=@Prioritet,vrijemepocetka=@Vrijemepocetka,vrijemezavrsetka=@VrijemeZavrsetka,opis=@Opis,status=@Status WHERE id=@Id",
                parameters);
            return true;
        }

        public async Task<bool> PromijeniStatusKvara(bool status,int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Status", status, DbType.Boolean);
            var kvaruredi =
           await _dbService.UrediPodatke(
               "Update public.kvarovi SET status=@Status WHERE id=@Id",
               parameters);
            return true;
        }

        public async Task<bool> ProvjeriKvar(int strojid)
        {
            bool b = false;
            var parameters = new DynamicParameters();
            parameters.Add("Idstroja", strojid, DbType.Int32);
            var kvar = await _dbService.DohvatiSve<Kvarovi>("SELECT * FROM public.kvarovi where idstroja=@idstroja", parameters);

            var list= new List<Kvarovi>();

            list=(from a in kvar
                 where a.Status==true
                 select a).ToList();
                      
            if(list.Count>0) b=true;

            return b;   
        }
    }
}
