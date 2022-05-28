using System;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using DiffAPICore;
using DiffAPICore.Models;
using System.Threading.Tasks;
using DAL;
using DAL.Data;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace APIIntegrationTest
{
    public class DiffIntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        // setup server and htttpclient
        public DiffIntegrationTest()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        // test get request to controller
        [Fact]
        public async Task TestGetData()
        {
            var response = await _client.GetAsync("v1/diff");

            // Assert
            response.EnsureSuccessStatusCode();
        }


        // empty tables in database

        [Fact]
        public async Task TestEmptyData()
        {
            var response = await _client.DeleteAsync("v1/diff");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        // get response when right and left data don't exist
        [Fact]
        public async Task TestRightAndLeftNotExists()
        {
            var response = await _client.GetAsync("v1/diff/1");

            var statuscode = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statuscode);
        
        }

        // Test adding leftdata
        [Fact]
        public async Task TestAddLeft()
        {
            DataModel model = new DataModel();
            model.Data = "AAAAAA==";
            var value = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            value.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var response = await client.PostAsync(uri, myRequestObject, new JsonMediaTypeFormatter());
            var response = await _client.PostAsync("v1/diff/1/left",model, new JsonMediaTypeFormatter());
            var statuscode=response.StatusCode;
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, statuscode);
        }

        // get difference when only leftdata exists in database
        [Fact]
        public async Task TestLeftOnlyExists()
        {
            var response = await _client.GetAsync("v1/diff/1");
            var contentStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();
            var diff = serializer.Deserialize<DiffInfo>(jsonReader);
            DiffInfo diffInfo = new DiffInfo();

            var statuscode = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statuscode);
        }

        // test adding RightData to database

        [Fact]
        public async Task TestAddRight()
        {
            DataModel model = new DataModel();
            model.Data = "AAAAAA==";
            var value = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            value.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var response = await client.PostAsync(uri, myRequestObject, new JsonMediaTypeFormatter());
            var response = await _client.PostAsync("v1/diff/1/right", model, new JsonMediaTypeFormatter());
            var statuscode = response.StatusCode;
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, statuscode);
        }

        // test when leftdata and rightdata exist in database and are equal
        [Fact]
        public async Task TestLeftAndRightEqual()
        {
            var response = await _client.GetAsync("v1/diff/1");
            var contentStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();
            var diff = serializer.Deserialize<DiffInfo>(jsonReader);
            DiffInfo diffInfo = new DiffInfo();
            diffInfo.DiffResultType = "Equals";
            Assert.Equal(diff.DiffResultType, diffInfo.DiffResultType);
            var statuscode = response.StatusCode;
            Assert.Equal(System.Net.HttpStatusCode.OK, statuscode);
        }

        // test updating rightdata in database with id=1

        [Fact]
        public async Task TestUpdateRight()
        {
            DataModel model = new DataModel();
            model.Data = "AQABAQ==";
            var value = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            value.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var response = await client.PostAsync(uri, myRequestObject, new JsonMediaTypeFormatter());
            var response = await _client.PutAsync("v1/diff/1/Right", model, new JsonMediaTypeFormatter());
            var statuscode = response.StatusCode;
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, statuscode);
        }

        // get differences from rightdata and leftdata 

        [Fact]
        public async Task TestLeftAndRightExistNotEqual()
        {
            var response = await _client.GetAsync("v1/diff/1");
            var contentStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();
            var list = new List<Diff>();
            var data = new DiffInfo();
            data.DiffResultType = "ContentDoNotMatch";
            var diffdata1 = new Diff();
            diffdata1.Offset=1; ;
            diffdata1.Length = 1;
            list.Add(diffdata1);
            var diffdata2 = new Diff();
            diffdata2.Offset = 3; ;
            diffdata2.Length = 1;
            list.Add(diffdata2);
            var diffdata3 = new Diff();
            diffdata3.Offset = 5; 
            diffdata3.Length = 1;
            list.Add(diffdata3);
            data.DiffResult = list;
            var diff= serializer.Deserialize<DiffInfo>(jsonReader);
           
            
            var statuscode=response.StatusCode;
            Assert.Equal(data, diff);
            // Assert
           Assert.Equal(System.Net.HttpStatusCode.OK,statuscode);
        }
    }
}
