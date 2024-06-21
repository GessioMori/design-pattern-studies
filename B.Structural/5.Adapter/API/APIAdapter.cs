using System.Text.Json;
using System.Xml.Linq;

namespace _5.Adapter.API
{
    internal class APIAdapter : IWeatherProvider
    {
        private readonly APIProvider _provider;

        public APIAdapter(APIProvider provider)
        {
            this._provider = provider;
        }

        public async Task<string> GetWeather()
        {
            WeatherAPIParameters parameters = new()
            {
                Latitude = -22.0175,
                Longitude = -47.8908,
                Data = ["temperature_2m"],
                ForecastDays = 1
            };

            string jsonResponse = await this._provider.FetchWeatherData(parameters);

            JsonDocument jsonObj = JsonDocument.Parse(jsonResponse);

            XElement root = new("WeatherData");

            root.Add(new XElement("Latitude", jsonObj.RootElement.GetProperty("latitude").GetDouble()));
            root.Add(new XElement("Longitude", jsonObj.RootElement.GetProperty("longitude").GetDouble()));
            root.Add(new XElement("GenerationTimeMs", jsonObj.RootElement.GetProperty("generationtime_ms").GetDouble()));
            root.Add(new XElement("UtcOffsetSeconds", jsonObj.RootElement.GetProperty("utc_offset_seconds").GetInt32()));
            root.Add(new XElement("Timezone", jsonObj.RootElement.GetProperty("timezone").GetString()));
            root.Add(new XElement("TimezoneAbbreviation", jsonObj.RootElement.GetProperty("timezone_abbreviation").GetString()));
            root.Add(new XElement("Elevation", jsonObj.RootElement.GetProperty("elevation").GetDouble()));

            XElement hourlyUnits = new("HourlyUnits");
            foreach (JsonProperty unit in jsonObj.RootElement.GetProperty("hourly_units").EnumerateObject())
            {
                hourlyUnits.Add(new XElement(unit.Name, unit.Value.GetString()));
            }
            root.Add(hourlyUnits);

            XElement hourly = new("Hourly");
            JsonElement.ArrayEnumerator times = jsonObj.RootElement.GetProperty("hourly").GetProperty("time").EnumerateArray();
            JsonElement.ArrayEnumerator temperatures = jsonObj.RootElement.GetProperty("hourly").GetProperty("temperature_2m").EnumerateArray();

            JsonElement.ArrayEnumerator timeEnumerator = times.GetEnumerator();
            JsonElement.ArrayEnumerator temperatureEnumerator = temperatures.GetEnumerator();

            while (timeEnumerator.MoveNext() && temperatureEnumerator.MoveNext())
            {
                XElement timeElement = new ("Time",
                    new XElement("Timestamp", timeEnumerator.Current.GetString()),
                    new XElement("Temperature2m", temperatureEnumerator.Current.GetDouble()));
                hourly.Add(timeElement);
            }

            root.Add(hourly);

            return root.ToString();
        }
    }
}
