using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.APICurrent;
using DAL.APIForecast;
using DAL.APIWeather;

namespace DAL.Repositorys
{
    public interface IDataRepository
    {
        public Task<WeatherForecast> GetWeather(string city, string user);
        public Task<List<WeatherForecast>> GetHourlyForecast(string city, string user);
        public Task<List<WeatherForecast>> GetDailyForecast(string city, string user);
        public bool AddUserCities(string cities, string user);

        public List<WeatherForecast> GetAllForecastsForUser(string user);
        
    }
}
