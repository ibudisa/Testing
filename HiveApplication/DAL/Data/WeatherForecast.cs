using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data
{
    public class WeatherForecast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("CityIdVal")]
        public int CityId { get; set; } 
        
        public DateTime Date { get; set; }

        public float Temperature { get; set; }

        public int UVIndex {get; set; }

        public int Visibility { get; set; } 

        public string? Description { get; set; }

        public ForecastT ForecastType { get; set; }

        public int Humidity { get; set; }   

        public float WindSpeed { get; set; }    

        public float FeelsLikeTemperature { get; set; }

        public int Pressure { get; set; }


    }


    public enum ForecastT:byte
    {
        Current=0,
        Hourly=1,
        Daily

    }
}