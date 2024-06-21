namespace _5.Adapter.XML
{
    internal class XMLProvider : IWeatherProvider
    {
        public XMLProvider()
        {

        }
        public async Task<string> GetWeather()
        {
            string filePath = ".\\XML\\data.xml";
            string fileContent = await File.ReadAllTextAsync(filePath);
            return fileContent;
        }
    }
}
