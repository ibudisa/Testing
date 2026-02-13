using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RentWebApplication.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace RentWebApplication.Controllers
{
    public class DataController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public DataController(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;
        public async Task<IActionResult> Index()
        {
            try
            {
                List<ObjectInfo> list = new List<ObjectInfo>();

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,"https://apit.my-rent.net/objects/simple_list");
                httpRequestMessage.Headers.Add("guid", "0e8d9575-7a20-4755-9c45-68e695131d8a");
                httpRequestMessage.Headers.Add("token", "559a513b-2b93-4575-b1b2-077e2f06379a");


                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string g = await httpResponseMessage.Content.ReadAsStringAsync();
                    list = await httpResponseMessage.Content.ReadFromJsonAsync<List<ObjectInfo>>();

                    //list = await JsonConvert.DeserializeObject<List<ObjectInfo>>(content);
                    int i = 0;
                }
                return View(list);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id_hash)
        {
            try
            {
                List<ObjectInfo> ls = new List<ObjectInfo>();
                ObjectInfo obj = new ObjectInfo();
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,"https://apit.my-rent.net/objects/simple_details?id_hash=" + id_hash);
                httpRequestMessage.Headers.Add("guid", "0e8d9575-7a20-4755-9c45-68e695131d8a");
                httpRequestMessage.Headers.Add("token", "559a513b-2b93-4575-b1b2-077e2f06379a");


                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string g = await httpResponseMessage.Content.ReadAsStringAsync();
                    ls = await httpResponseMessage.Content.ReadFromJsonAsync<List<ObjectInfo>>();

                    //list = await JsonConvert.DeserializeObject<List<ObjectInfo>>(content);
                    if (ls.Count > 0) obj = ls[0];
                 bool show =await Showpictures(id_hash);
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
;
        }

        private async Task<bool> Showpictures(string id_hash)
        {
            List<Picture> pictures = new List<Picture>();
            List<string> srcs = new List<string>();

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,"https://apit.my-rent.net/objects/get_pictures_links?id_hash=" + id_hash);
            httpRequestMessage.Headers.Add("guid", "0e8d9575-7a20-4755-9c45-68e695131d8a");
            httpRequestMessage.Headers.Add("token", "559a513b-2b93-4575-b1b2-077e2f06379a");

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string g = await httpResponseMessage.Content.ReadAsStringAsync();
                pictures = await httpResponseMessage.Content.ReadFromJsonAsync<List<Picture>>();

                //list = await JsonConvert.DeserializeObject<List<ObjectInfo>>(content);
                if (pictures.Count > 0)
                {
                    foreach (var picture in pictures)
                    {
                        srcs.Add(picture.picture_link);
                    }
                    ViewBag.sources = srcs;
                }
                return true;
            }
            return false;   
        }
    }
}
