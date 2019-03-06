// Your code here!



    var city1 = { Name: "London,united+kingdom", PostalCode: 11111 };
    var city2 = { Name: "Los Angeles,USA", PostalCode: 2222 };
    var city3 = { Name: "Detroit,USA",PostalCode: 3333};

    var array = [];

    array.push(city1);
    array.push(city2);
    array.push(city3);

    //getWeather(city1);
    
const appKey = "f24f40b1c24505685fce3b8acd0fcffc";
let cityName = document.getElementById("city-name");

let temperature = document.getElementById("temp");
let humidity = document.getElementById("humidity-div");
let searchButton = document.getElementById("btn");
//searchButton.addEventListener("click", findWeatherDetails);

array.forEach(function (element) {
    findWeatherDetails(element.Name);

});

function findWeatherDetails(city) {
    //var city = "Washington,USA";
    let searchLink = "https://api.openweathermap.org/data/2.5/weather?q=" +city + "&appid=" + appKey;
    httpRequestAsync(searchLink, theResponse);
    
}
function theResponse(response) {
    let jsonObject = JSON.parse(response);
    //cityName.innerHTML = jsonObject.name;
    var city = jsonObject.name;
    //temperature.innerHTML = parseInt(jsonObject.main.temp - 273) + "°";
    var citytemperature = parseInt(jsonObject.main.temp - 273);
    //humidity.innerHTML = jsonObject.main.humidity + "%";
    var cityhumidity = jsonObject.main.humidity;
    var table = document.getElementById("tbl");
    var row = "<tr><td> " + city + "</td><td>" + citytemperature + "°C</td><td>" + cityhumidity + "%</td></tr>";
    table.innerHTML += row;
}

function httpRequestAsync(url, callback) {
    console.log("hello");
    var httpRequest = new XMLHttpRequest();
    httpRequest.onreadystatechange = () => {
        if (httpRequest.readyState == 4 && httpRequest.status == 200)
            callback(httpRequest.responseText);
    }
    httpRequest.open("GET", url, true); // true for asynchronous 
    httpRequest.send();
}
   
