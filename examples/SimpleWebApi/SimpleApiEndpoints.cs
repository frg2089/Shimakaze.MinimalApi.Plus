using Microsoft.AspNetCore.Http.HttpResults;
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

    /// <summary>
    /// SetWeatherForecast
    /// </summary>
    /// <param name="weathers">WeatherForecasts</param>
    /// <returns></returns>
    [HttpPost("/weatherforecast")]
    public Ok<WeatherForecast[]> SetWeatherForecasts([FromBody] WeatherForecast[] weathers)
        => TypedResults.Ok(weathers);
}