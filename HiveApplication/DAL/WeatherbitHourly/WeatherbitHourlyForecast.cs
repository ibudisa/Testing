using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.WeatherbitHourly
{
    public class Datum
    {
        public double app_temp { get; set; }
        public int clouds { get; set; }
        public int clouds_hi { get; set; }
        public int clouds_low { get; set; }
        public int clouds_mid { get; set; }
        public string datetime { get; set; }
        public double dewpt { get; set; }
        public int dhi { get; set; }
        public int dni { get; set; }
        public int ghi { get; set; }
        public int ozone { get; set; }
        public string pod { get; set; }
        public int pop { get; set; }
        public int precip { get; set; }
        public int pres { get; set; }
        public int rh { get; set; }
        public int slp { get; set; }
        public int snow { get; set; }
        public int snow_depth { get; set; }
        public double solar_rad { get; set; }
        public double temp { get; set; }
        public DateTime timestamp_local { get; set; }
        public DateTime timestamp_utc { get; set; }
        public int ts { get; set; }
        public int uv { get; set; }
        public int vis { get; set; }
        public Weather weather { get; set; }
        public string wind_cdir { get; set; }
        public string wind_cdir_full { get; set; }
        public int wind_dir { get; set; }
        public double wind_gust_spd { get; set; }
        public double wind_spd { get; set; }
    }

    public class WeatherbitHourlyForecast
    {
        public string city_name { get; set; }
        public string country_code { get; set; }
        public List<Datum> data { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string state_code { get; set; }
        public string timezone { get; set; }
    }

    public class Weather
    {
        public string description { get; set; }
        public int code { get; set; }
        public string icon { get; set; }
    }


}
