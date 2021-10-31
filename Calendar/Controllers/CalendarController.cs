using Calendar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Calendar.Controllers
{
  public class CalendarController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public CalendarController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index(int month = 0, int year = 0, string view = "monthly-table")
    {
      var currentDate = new DateTime((year == 0 ? DateTime.Today.Year : year), (month == 0 ? DateTime.Today.Month : month), ((year == 0 && month == 0) ? DateTime.Today.Day : 1));
      var model = new CalendarViewModel(currentDate);

      model.Events[1].Add(new EventViewModel()
      {
        Description = "Test Event 1"
      });

      model.Events[1].Add(new EventViewModel()
      {
        Description = "Test Event 2"
      });

      model.Events[2].Add(new EventViewModel()
      {
        Description = "Test Event 3"
      });

      return View(view, model);
    }
  }
}
