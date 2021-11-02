using Calendar.Models;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Controllers
{
  public class CalendarController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly IGoogleAuthProvider _auth;

    public CalendarController(ILogger<HomeController> logger, IGoogleAuthProvider auth)
    {
      _logger = logger;
      _auth = auth;
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

    [GoogleScopedAuthorize(CalendarService.ScopeConstants.CalendarReadonly)]
    public async Task<IActionResult> Test()
    {
      var cred = await _auth.GetCredentialAsync();
      var service = new CalendarService(new BaseClientService.Initializer
      {
        HttpClientInitializer = cred
      });
      var calendars = await service.CalendarList.List().ExecuteAsync();
      var events = await service.Events.List("family14235578936734473429@group.calendar.google.com").ExecuteAsync();

      //var names = string.Join('\n', calendars.Items.Select(c => $"{c.Summary} ({c.Id})" ));
      var names = string.Join('\n', events.Items.Select(e => $"{e.Summary} ({FormatTimeRange(e.Start.DateTime.Value, e.End.DateTime.Value)})" ));
      return Content(names);
    }

    private string FormatTimeRange(DateTime start, DateTime end)
    {
      if (start.Hour < 12 && end.Hour < 12)
      {
        return $"{start.Hour}-{end.Hour}am";
      } 
      else if (start.Hour > 12 && end.Hour > 12) 
      {
        return $"{start.Hour}-{end.Hour}pm";
      } 
      else
      {
        return $"{start.Hour}am-{end.Hour}pm";
      }
    }
  }
}
