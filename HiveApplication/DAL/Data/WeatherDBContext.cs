using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class WeatherDBContext : DbContext
    {
        public WeatherDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCity> UserCities { get; set; }
    }
}
