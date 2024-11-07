using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    public class EventIds
    {
        public const int LoginEvent = 2000;
        public const int LogoutEvent = 2001;
        public const int FileUploadEvent = 2002;
        public const int FileDownloadEvent = 2003;
        public const int UserRegistrationEvent = 2004;
        public const int PasswordChangeEvent = 2005;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // _logger.LogTrace("This is logging message.");
        // _logger.LogInformation(EventIds.LoginEvent, "This is a logging message with event id.");
        try
        {
            _logger.LogInformation("This is a logging message with args: Today is {Week}. It is {Time}.", DateTime.Now.DayOfWeek, DateTime.Now.ToLongTimeString());
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "This is a logging message with exception.");
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
