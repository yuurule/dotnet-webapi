using Microsoft.AspNetCore.Mvc;

namespace EnvironmentDemos.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationController(IConfiguration configuration) : ControllerBase
{
  // private readonly IConfiguration _configuration;
  // public ConfigurationController(IConfiguration configuration)
  // {
  //   _configuration = configuration;
  // }

  [HttpGet]
  [Route("database-configuration")]
  public ActionResult GetDatabaseConfiguration()
  {
    var type = configuration["database:Type"];
    var connectionString = configuration["Database:ConnectionString"];
    return Ok(new { Type=type, ConnectionString=connectionString });
  }
}