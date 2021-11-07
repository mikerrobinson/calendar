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

    [GoogleScopedAuthorize(CalendarService.ScopeConstants.CalendarReadonly)]
    public async Task<IActionResult> Index(int month = 0, int year = 0, string view = "monthly-table")
    {
      var currentDate = new DateTime((year == 0 ? DateTime.Today.Year : year), (month == 0 ? DateTime.Today.Month : month), ((year == 0 && month == 0) ? DateTime.Today.Day : 1));
      var model = new CalendarViewModel(currentDate);

      var cred = await _auth.GetCredentialAsync();
      var service = new CalendarService(new BaseClientService.Initializer
      {
        HttpClientInitializer = cred
      });
      var eventsRequest = service.Events.List("sv83o5bgqa52flhavimd3kji7pnm335r@import.calendar.google.com");
      eventsRequest.SingleEvents = true;
      eventsRequest.TimeMin = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
      eventsRequest.TimeMax = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month), 23, 59, 59);
      eventsRequest.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
      var events = await eventsRequest.ExecuteAsync();

      foreach (var item in events.Items)
      {
        var day = item.Start.DateTime.Value.Day;
        model.Events[day].Add(new EventViewModel()
        {
          Description = item.Summary,
          Start = item.Start.DateTime.Value,
          End = item.End.DateTime.Value
        });
      }
      return View(view, model);
    }

    [GoogleScopedAuthorize(CalendarService.ScopeConstants.CalendarReadonly)]
    public async Task<IActionResult> Test()
    {
      var model = new CalendarViewModel(DateTime.Now);

      var cred = await _auth.GetCredentialAsync();
      var service = new CalendarService(new BaseClientService.Initializer
      {
        HttpClientInitializer = cred
      });
      var calendars = await service.CalendarList.List().ExecuteAsync();
      var eventsRequest = service.Events.List("sv83o5bgqa52flhavimd3kji7pnm335r@import.calendar.google.com");
      eventsRequest.SingleEvents = true;
      eventsRequest.TimeMin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
      eventsRequest.TimeMax = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month), 23, 59, 59);
      eventsRequest.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
      var events = await eventsRequest.ExecuteAsync();

      //var names = string.Join('\n', calendars.Items.Select(c => $"{c.Summary} ({c.Id})" ));

      //var names = string.Join('\n', events.Items.Select(e => $"{e.Summary} ({e.Id})" ));
      //return Content(names);

      //var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);

      foreach (var item in events.Items)
      {
        var day = item.Start.DateTime.Value.Day;
        model.Events[day].Add(new EventViewModel()
        {
          Description = item.Summary,
          Start = item.Start.DateTime.Value,
          End = item.End.DateTime.Value
        });
        model.EventList.Add(new EventViewModel()
        {
          Description = item.Summary,
          Start = item.Start.DateTime.Value,
          End = item.End.DateTime.Value
        });
      }
      return View(model);
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
