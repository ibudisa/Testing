using DAL.Entiteti;
using VanadoWebAPI.Models;

namespace VanadoWebAPI.Helpers
{
    public class MapData
    {
        public static Strojevi MapirajStrojevi(StrojeviModel model,int id)
        {
            var data=new Strojevi();
            data.Id = id;
            data.Naziv = model.Naziv;

            return data;
        }

        public static Kvarovi MapirajKvarovi(KvaroviModel model,int id)
        {
            var data =new Kvarovi();
            data.Id = id;
            data.Naziv = model.Naziv;
            data.Idstroja = model.Idstroja;
            data.Vrijemepocetka = model.Vrijemepocetka;
            data.Vrijemezavrsetka = model.Vrijemezavrsetka;
            data.Opis=model.Opis;
            data.Status=model.Status;
            data.Prioritet = model.Prioritet;

            return data;
        }
    }
}
