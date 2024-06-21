namespace _5.Adapter
{
    internal interface IWeatherProvider
    {
        public Task<string> GetWeather();
    }
}
