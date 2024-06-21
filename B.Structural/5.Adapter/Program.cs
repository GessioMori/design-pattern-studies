using _5.Adapter.API;
using _5.Adapter.XML;

namespace _5.Adapter
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IWeatherProvider weatherProvider = new XMLProvider();

            string xmlResponse = await weatherProvider.GetWeather();

            Console.WriteLine("This is the XML response:");
            Console.WriteLine(xmlResponse);

            APIProvider apiProvider = new();

            IWeatherProvider apiAdapter = new APIAdapter(apiProvider);

            string apiResponse = await apiAdapter.GetWeather();

            Console.WriteLine("This is the API response:");
            Console.WriteLine(apiResponse);
        }
    }
}
