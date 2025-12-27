using Microsoft.AspNetCore.Mvc;

using Shimakaze.MinimalApi.Plus;

namespace SimpleWebApi;

[ApiEndpoints]
public sealed class SimpleApiEndpoints(SimpleServices services) : ApiEndpoints
{
    /// <summary>
    /// GetWeatherForecast
    /// </summary>
    /// <returns></returns>
    [HttpGet("/weatherforecast")]
    public WeatherForecast[] GetWeatherForecasts()
        => services.GetWeatherForecasts();
}
