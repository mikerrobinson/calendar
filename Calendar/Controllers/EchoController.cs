using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Calendar.Controllers
{
  public class EchoController : Controller
  {
    private readonly IConfiguration _configuration;

    public EchoController(ILogger<HomeController> logger, IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public IActionResult Index()
    {
      return View(_configuration.AsEnumerable());
    }
  }
}
