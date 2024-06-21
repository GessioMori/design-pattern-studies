using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace _5.Adapter.API
{
    internal class APIProvider
    {
        private readonly string _apiURL;

        public APIProvider()
        {
            this._apiURL = "https://api.open-meteo.com/v1/forecast";
        }

        public async Task<string> FetchWeatherData(WeatherAPIParameters parameters)
        {
            using HttpClient client = new HttpClient();

            try
            {
                UriBuilder builder = new(this._apiURL);
                NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);
                query["latitude"] = parameters.Latitude.ToString(CultureInfo.InvariantCulture);
                query["longitude"] = parameters.Longitude.ToString(CultureInfo.InvariantCulture);
                query["hourly"] = string.Join(',', parameters.Data);
                query["forecast_days"] = parameters.ForecastDays.ToString();
                builder.Query = query.ToString();
                string url = builder.ToString();

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                return responseData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return "Unable to fetch weather data from API.";
            }
        }
    }

    public record WeatherAPIParameters
    {
        public required double Latitude;
        public required double Longitude;
        public required string[] Data;
        public required int ForecastDays;
    }
}
