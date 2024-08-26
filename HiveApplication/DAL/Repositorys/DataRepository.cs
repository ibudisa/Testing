using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.APICurrent;
using DAL.APIForecast;
using DAL.APIWeather;
using DAL.WeatherbitHourly;
using DAL.WeatherbitDaily;
using Newtonsoft.Json;

namespace DAL.Repositorys
{
    public class DataRepository : IDataRepository
    {
        private readonly WeatherDBContext _cntx;

        public DataRepository(WeatherDBContext ctx)
        {
            _cntx = ctx;
        }


        public async Task<WeatherForecast> GetWeather(string city, string user)
        {
            string apiKeyOWeather = "d4e611667c168e673ff781c5e9282c80"; // Replace with your OpenWeatherMap API key
            string apiWeatherstack = "7aae0d0b8034def58abb3a31f6e317b0";

            // URLs for fetching current weather and forecast data.
            string OweatherApiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKeyOWeather}";
            string WeatherstackUrl = $"https://api.weatherstack.com/current?access_key={apiWeatherstack}&query={city}";

            WeatherForecast dataforecast = new WeatherForecast();

            using (HttpClient client = new HttpClient())
            {
                // Make API requests for current weather and forecast data.
                HttpResponseMessage response = await client.GetAsync(OweatherApiUrl);
                HttpResponseMessage response2 = await client.GetAsync(WeatherstackUrl);

                // Check if both API requests are successful.
                response.EnsureSuccessStatusCode();
                response2.EnsureSuccessStatusCode();

                // Read the JSON responses.
                string weatherdata = await response.Content.ReadAsStringAsync();
                string weatherstackdata = await response2.Content.ReadAsStringAsync();

                OpenWeather? openweatherjson = JsonConvert.DeserializeObject<OpenWeather>(weatherdata);
                WeatherstackCurrent? weatherstackjson = JsonConvert.DeserializeObject<WeatherstackCurrent>(weatherstackdata);

                using var transaction = _cntx.Database.BeginTransaction();
                bool contains = _cntx.Citys.Any(p => p.Name.Equals(city));
                if (!contains)
                {
                    DAL.Data.City citydata = new DAL.Data.City();
                    citydata.Name = city;
                    _cntx.Citys.Add(citydata);
                    _cntx.SaveChanges();
                }
                DAL.Data.City? c = _cntx.Citys.FirstOrDefault(p => p.Name.Equals(city));

                if (c != null)
                    dataforecast.CityId = c.Id;
                dataforecast.Date = DateTime.Now;
                dataforecast.Temperature = (float)openweatherjson.main.temp;
                dataforecast.Visibility = openweatherjson.visibility;
                dataforecast.Description = openweatherjson.weather[0].description;
                dataforecast.UVIndex = weatherstackjson.current.uv_index;

                _cntx.WeatherForecasts.Add(dataforecast);
                _cntx.SaveChanges();

                transaction.Commit();
            }
            return dataforecast;
        }

        public async Task<List<WeatherForecast>> GetHourlyForecast(string city, string user)
        {
            string openweatherkey = string.Empty;
            string weatherstack = string.Empty;
            string weatherbit = string.Empty;

            CheckData(city, user);

            User? userd = _cntx.Users.FirstOrDefault(p => p.Name.Equals(user));

            if (userd != null)
            {
                openweatherkey = userd.OpenWeatherMapApiKey;
                weatherstack = userd.WeatherstackApiKey;
                weatherbit = userd.WeatherbitApiKey;
            }

            string OpenweatherApiUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={openweatherkey}";
            string WeatherbitUrl = $"https://api.weatherbit.io/v2.0/forecast/hourly?city={city}&key={weatherbit}&hours=24";

            WeatherForecast dataforecast = new WeatherForecast();
            List<WeatherForecast> dataforecastlist = new List<WeatherForecast>();

            using (HttpClient client = new HttpClient())
            {
                // Make API requests for current weather and forecast data.
                HttpResponseMessage response = await client.GetAsync(OpenweatherApiUrl);
                HttpResponseMessage response2 = await client.GetAsync(WeatherbitUrl);

                // Check if both API requests are successful.
                response.EnsureSuccessStatusCode();
                response2.EnsureSuccessStatusCode();

                // Read the JSON responses.
                string openforecastdata = await response.Content.ReadAsStringAsync();
                string weatherbitdata = await response2.Content.ReadAsStringAsync();

                OpenForecast? openforecastjson = JsonConvert.DeserializeObject<OpenForecast>(openforecastdata);
                WeatherbitHourlyForecast? weatherbitjson = JsonConvert.DeserializeObject<WeatherbitHourlyForecast>(weatherbitdata);

                List<List> datalist = openforecastjson.list.Where(p => (DateTime.Parse(p.dt_txt) > DateTime.Now) && (DateTime.Parse(p.dt_txt) <= DateTime.Now.AddHours(24))).ToList();
                foreach(var datalistitem in datalist)
                {
                    WeatherForecast weatherForecast = new WeatherForecast();
                    weatherForecast.Date = DateTime.Parse(datalistitem.dt_txt);
                    weatherForecast.Visibility = datalistitem.visibility;
                    weatherForecast.Humidity = datalistitem.main.humidity;
                    weatherForecast.Pressure = datalistitem.main.pressure;
                    weatherForecast.Temperature = (float)datalistitem.main.temp;
                    weatherForecast.Description = datalistitem.weather[0].description;
                    weatherForecast.Visibility=datalistitem.visibility;
                    weatherForecast.WindSpeed = (float)datalistitem.wind.speed;
                    weatherForecast.CityId = _cntx.Citys.FirstOrDefault(p => p.Name.Equals(city)).Id;
                    weatherForecast.FeelsLikeTemperature =(float)datalistitem.main.feels_like;
                    weatherForecast.ForecastType = ForecastT.Hourly;
                    DateTime dtime = DateTime.Parse(datalistitem.dt_txt);
                    List<WeatherbitHourly.Datum> dlist=weatherbitjson.data;
                    string s = dlist[0].datetime.ToString();
                    WeatherbitHourly.Datum data1= dlist.FirstOrDefault(p => DateTime.Parse(p.datetime.Replace(':',' ')+":00:00") == dtime);
                    weatherForecast.UVIndex = data1.uv;

                    dataforecastlist.Add(weatherForecast);

                    _cntx.WeatherForecasts.Add(weatherForecast);
                    _cntx.SaveChanges();
                }

                return dataforecastlist; ;
            }
        }
        public async Task<List<WeatherForecast>> GetDailyForecast(string city, string user)
        {

            string openweatherkey = string.Empty;
            string weatherstack = string.Empty;
            string weatherbit = string.Empty;

            CheckData(city, user);

            User? userd = _cntx.Users.FirstOrDefault(p => p.Name.Equals(user));

            if (userd != null)
            {
                openweatherkey = userd.OpenWeatherMapApiKey;
                weatherstack = userd.WeatherstackApiKey;
                weatherbit = userd.WeatherbitApiKey;
            }

            string OpenweatherApiUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={openweatherkey}";
            string WeatherbitUrl = $"https://api.weatherbit.io/v2.0/forecast/daily?city={city}&key={weatherbit}&days=6";

            WeatherForecast dataforecast = new WeatherForecast();
            List<WeatherForecast> dataforecastlist = new List<WeatherForecast>();

            using (HttpClient client = new HttpClient())
            {
                // Make API requests for current weather and forecast data.
                HttpResponseMessage response = await client.GetAsync(OpenweatherApiUrl);
                HttpResponseMessage response2 = await client.GetAsync(WeatherbitUrl);

                // Check if both API requests are successful.
                response.EnsureSuccessStatusCode();
                response2.EnsureSuccessStatusCode();

                // Read the JSON responses.
                string openforecastdata = await response.Content.ReadAsStringAsync();
                string weatherbitdata = await response2.Content.ReadAsStringAsync();

                OpenForecast? openforecastjson = JsonConvert.DeserializeObject<OpenForecast>(openforecastdata);
                WeatherbitDailyForecast? weatherbitjson = JsonConvert.DeserializeObject<WeatherbitDailyForecast>(weatherbitdata);

                List<List> datalist = openforecastjson.list;
                List<List> list1 = new List<List>();
                foreach (var d in datalist)
                {
                    string str = d.dt_txt;
                    string[] arr = str.Split(' ');
                    if (arr[1].Equals("15:00:00"))
                    {
                        list1.Add(d);
                    }
                }

                foreach(var a in list1)
                {

                    WeatherForecast weatherForecast = new WeatherForecast();
                    weatherForecast.Date = DateTime.Parse(a.dt_txt);
                    weatherForecast.Visibility = a.visibility;
                    weatherForecast.Humidity = a.main.humidity;
                    weatherForecast.Pressure = a.main.pressure;
                    weatherForecast.Temperature = (float)a.main.temp;
                    weatherForecast.Description = a.weather[0].description;
                    weatherForecast.Visibility = a.visibility;
                    weatherForecast.WindSpeed = (float)a.wind.speed;
                    weatherForecast.CityId = _cntx.Citys.FirstOrDefault(p => p.Name.Equals(city)).Id;
                    weatherForecast.FeelsLikeTemperature = (float)a.main.feels_like;
                    weatherForecast.ForecastType = ForecastT.Daily;
                    List<WeatherbitDaily.Datum> dlist = weatherbitjson.data;
                    string[] arrdata=a.dt_txt.Split(' ');

                    WeatherbitDaily.Datum data1 = dlist.FirstOrDefault(p => p.datetime.Equals(arrdata[0]));
                    weatherForecast.UVIndex = data1.uv;


                    _cntx.WeatherForecasts.Add(weatherForecast);
                    _cntx.SaveChanges();

                    dataforecastlist.Add(weatherForecast);
                }
                return dataforecastlist;
            }
            }

        private void CheckData(string city, string user)
        {
            bool containsuser = _cntx.Users.Any(p => p.Name.Equals(user));
            if (!containsuser)
            {
                DAL.Data.User userdata = new DAL.Data.User();
                userdata.Name = user;
                userdata.OpenWeatherMapApiKey = "d4e611667c168e673ff781c5e9282c80";
                userdata.WeatherstackApiKey = "7aae0d0b8034def58abb3a31f6e317b0";
                userdata.WeatherbitApiKey = "f56be7215de148449fa54db7d2f03831";
                _cntx.Users.Add(userdata);
                _cntx.SaveChanges();
            }
            DAL.Data.User? u = _cntx.Users.FirstOrDefault(p => p.Name.Equals(user));

            int userID = u.Id;


            bool contains = _cntx.Citys.Any(p => p.Name.Equals(city));
            if (!contains)
            {
                DAL.Data.City citydata = new DAL.Data.City();
                citydata.Name = city;
                _cntx.Citys.Add(citydata);
                _cntx.SaveChanges();
            }
            DAL.Data.City? c = _cntx.Citys.FirstOrDefault(p => p.Name.Equals(city));

            int cityID = c.Id;


            bool containsusercity = _cntx.UserCities.Any(p => (p.UserId == userID) && (p.CityId == cityID));
            if (!containsusercity)
            {
                DAL.Data.UserCity ucitydata = new DAL.Data.UserCity();
                ucitydata.UserId = userID;
                ucitydata.CityId = cityID;
                _cntx.UserCities.Add(ucitydata);
                _cntx.SaveChanges();
            }

        }

        public bool AddUserCities(string cities, string user)
        {
            bool containsuser = _cntx.Users.Any(p => p.Name.Equals(user));
            if (!containsuser)
            {
                DAL.Data.User userdata = new DAL.Data.User();
                userdata.Name = user;
                userdata.OpenWeatherMapApiKey = "d4e611667c168e673ff781c5e9282c80";
                userdata.WeatherstackApiKey = "7aae0d0b8034def58abb3a31f6e317b0";
                userdata.WeatherbitApiKey = "f56be7215de148449fa54db7d2f03831";
                _cntx.Users.Add(userdata);
                _cntx.SaveChanges();
            }
            DAL.Data.User? u = _cntx.Users.FirstOrDefault(p => p.Name.Equals(user));

            int userID = u.Id;

            List<int> cityIds = new List<int>();

            string[] cityarr = cities.Split(',');

            bool containscity = false;

            for(int i = 0; i < cityarr.Length; i++)
            {
                containscity= _cntx.Citys.Any(p => p.Name.Equals(cityarr[i]));
                if (!containscity)
                {
                    DAL.Data.City citydata = new DAL.Data.City();
                    citydata.Name = cityarr[i];
                    _cntx.Citys.Add(citydata);
                    _cntx.SaveChanges();
                }
                DAL.Data.City? c = _cntx.Citys.FirstOrDefault(p => p.Name.Equals(cityarr[i]));

                int cityID = c.Id;
                cityIds.Add(cityID);
            }
            bool result = false;
            foreach(int item in cityIds)
            {
                bool containsusercity= _cntx.UserCities.Any(p => (p.UserId == userID) && (p.CityId == item)); 
                if(!containsusercity)
                {
                    DAL.Data.UserCity ucitydata = new DAL.Data.UserCity();
                    ucitydata.UserId = userID;
                    ucitydata.CityId = item;
                    _cntx.UserCities.Add(ucitydata);
                    _cntx.SaveChanges();
                    result = true;
                }

            }
            return result;
        }

        public List<WeatherForecast> GetAllForecastsForUser(string user)
        {
            User userd = _cntx.Users.FirstOrDefault(p => p.Name.Equals(user));
            int uid = userd.Id;

            List<int> listcities = new List<int>();

            List<UserCity> listdata = _cntx.UserCities.Where(p => p.UserId == uid).ToList();

            foreach(UserCity city in listdata)
            {
                listcities.Add(city.CityId);
            }

            List<WeatherForecast> forecasts =GetWeatherForecastsByCities(listcities);

            return forecasts;

        }

        private List<WeatherForecast> GetWeatherForecastsByCities(List<int> listc)
        {
            List<WeatherForecast> weatherforecasts = new List<WeatherForecast>();

            foreach(int cityId in listc)
            {
                List<WeatherForecast> ls=_cntx.WeatherForecasts.Where(p => p.CityId == cityId).ToList();
                if(ls.Count > 0)
                    weatherforecasts.AddRange(ls);
            }

            return weatherforecasts;
        }
    }
 }
