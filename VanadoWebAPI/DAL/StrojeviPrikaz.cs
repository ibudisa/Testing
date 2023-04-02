using DAL.Entiteti;

namespace DAL
{
    public class StrojeviPrikaz
    {
        public string Naziv { get; set; }
        

        public TimeSpan TrajanjeKvara { get; private set; }

        public StrojeviPrikaz(Strojevi stroj)
        {
            Naziv = stroj.Naziv;
            TrajanjeKvara = this.ProsjecnoTrajanje(stroj.KvaroviLista);
        }

        private TimeSpan ProsjecnoTrajanje(List<Kvarovi> kvarovis)
        {
            List<TimeSpan> Trajanje = new List<TimeSpan>();
            TimeSpan time = TimeSpan.Zero;
            TimeSpan prosjek = TimeSpan.Zero;
            if (kvarovis.Count > 0)
            {
                foreach (Kvarovi kvarovi in kvarovis)
                {
                    TimeSpan t = new TimeSpan();

                    DateTime d1 = kvarovi.Vrijemezavrsetka;
                    DateTime d2 = kvarovi.Vrijemepocetka;

                    t = d1.Subtract(d2);
                    time += t;
                }
                prosjek = time / kvarovis.Count;
            }
            return prosjek;
        }

    }
}