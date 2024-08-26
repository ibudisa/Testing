using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DAL.Repositorys;
using DAL.Data;
using DAL.APICurrent;
using DAL.APIForecast;
using DAL.APIWeather;

namespace HiveApplication.Controllers
{
   
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IDataRepository _datar;

        public WeatherController(IDataRepository dataRepository)
        {
            _datar = dataRepository;
        }

        [Route("[controller]/current/{city}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrent(string city)
        {
            try
            {
                if ((city == null) || (city == string.Empty))
                    return BadRequest();
                else
                {
                    string user = Environment.UserName;
                    var data = await _datar.GetWeather(city, user);
                    if(data== null)
                        return NotFound();
                    else
                    return Ok(data);

                }
            }
            catch (Exception ec)
            {
                return StatusCode(500); 
            }
        }

        [Route("[controller]/hourly/{city}")]
        [HttpGet]
        public async Task<IActionResult> GetForecastHourly(string city)
        {
            try
            {
                if ((city == null) || (city == string.Empty))
                    return BadRequest();
                else
                {
                    string user = Environment.UserName;
                    var data = await _datar.GetHourlyForecast(city, user);
                    if (data == null)
                        return NotFound();
                    else
                        return Ok(data);
                }
            }
            catch(Exception ec)
            {
                return StatusCode(500);
            }
        }

        [Route("[controller]/daily/{city}")]
        [HttpGet]
        public async Task<IActionResult> GetForecastDaily(string city)
        {
            try
            {
                if ((city == null) || (city == string.Empty))
                    return BadRequest();
                else
                {
                    string user = Environment.UserName;
                    var data = await _datar.GetDailyForecast(city, user);
                    if (data == null)
                        return NotFound();
                    else
                        return Ok(data);
                }
            }
            catch(Exception ec)
            {
                return StatusCode(500);
            }
        }

        [Route("user/favorite")]
        [HttpPost]
        public IActionResult AddCities([FromQuery] string cities)
        {
            try
            {
                if ((cities == null) || (cities == string.Empty))
                    return BadRequest();
                else
                {
                    string user = Environment.UserName;
                    bool addeddata =  _datar.AddUserCities(cities, user);

                    return Ok(addeddata);
                }
            }
            catch(Exception ec)
            {
                return StatusCode(500);
            }
        }

        [Route("[controller]/favorites")]
        [HttpGet]
        [ResponseCache(CacheProfileName = "120SecondsDuration")]
        public IActionResult GetForecasts()
        {
            try
            {
            string user = Environment.UserName;
            var data =  _datar.GetAllForecastsForUser(user);
            if (data == null)
                return NotFound();
            else
                return Ok(data);

                
            }
            catch (Exception ec)
            {
                return StatusCode(500);
            }
        }

    }
}
