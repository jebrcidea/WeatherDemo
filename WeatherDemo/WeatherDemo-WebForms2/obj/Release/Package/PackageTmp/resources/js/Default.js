var finalGPS;

//function getLocation() {
if (navigator.geolocation) {
	navigator.geolocation.getCurrentPosition(showPosition);
} else {
	x.innerHTML = "Geolocation is not supported by this browser.";
}
//}

function showPosition(position) {
	finalGPS = position;
	var latitude = document.getElementById('<%= latitude.ClientID %>');
	var longitude = document.getElementById('<%= longitude.ClientID %>');
	console.log(latitude);
	console.log(longitude);
	console.log('<%= latitude.ClientID %>');
	console.log('<%= longitude.ClientID %>');

	latitude.innerHTML = position.coords.latitude;
	longitude.innerHTML = position.coords.longitude;

	var url = "https://api.openweathermap.org/data/2.5/weather?lat=" + position.coords.latitude + "&lon=" + position.coords.longitude + "&appid=bd04dc3ee7c7b0f24a22199153e40eae";
	console.log(url);

	$.ajax({
		url: url,
		type: 'GET',
		//jsonpCallback: "callback",
		//dataType: "jsonp",
		success: function (weatherResponse) {
			console.log(weatherResponse);
			var htmlWeather = document.getElementById("weather");
			htmlWeather.innerHTML = JSON.stringify(weatherResponse);
			//http://openweathermap.org/img/wn/10d@2x.png

			var feels_like = document.getElementById("feels_like");
			var humidity = document.getElementById("humidity");
			var pressure = document.getElementById("pressure");
			var temp = document.getElementById("temp");
			var temp_max = document.getElementById("temp_max");
			var temp_min = document.getElementById("temp_min");
			var name = document.getElementById("name");
			var weatherDescription = document.getElementById("weatherDescription");
			var weatherIcon = document.getElementById("weatherIcon");
			var country = document.getElementById("country");


			feels_like.innerHTML = weatherResponse.main.feels_like - 273.15;
			humidity.innerHTML = weatherResponse.main.humidity;
			pressure.innerHTML = weatherResponse.main.pressure;
			temp.innerHTML = weatherResponse.main.temp - 273.15;
			temp_max.innerHTML = weatherResponse.main.temp_max - 273.15;
			temp_min.innerHTML = weatherResponse.main.temp_min - 273.15;
			name.innerHTML = weatherResponse.name;
			weatherDescription.innerHTML = weatherResponse.weather[0].description;
			weatherIcon.src = "http://openweathermap.org/img/wn/" + weatherResponse.weather[0].icon + ".png";
			country.innerHTML = weatherResponse.sys.country;
		}
	}).done(function () {
		setTimeout(function () {
			$("#overlay").fadeOut(300);
		}, 500);
	});
}
$(document).ajaxSend(function () {
	$("#overlay").fadeIn(300);
});

$('#button').click(function () {
	showPosition(finalGPS);
});	