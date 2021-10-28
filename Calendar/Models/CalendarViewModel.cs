using System;
using System.Collections.Generic;

namespace Calendar.Models
{
  public class CalendarViewModel
  {
    public CalendarViewModel(DateTime currentDate)
    {
      CurrentDate = currentDate;

      var daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
      
      GridStart = (int)new DateTime(currentDate.Year, currentDate.Month, 1).DayOfWeek;

      var weeksShown = (int)Math.Ceiling((GridStart + daysInMonth) / 7.0);
      GridEnd = (weeksShown * 7) - (GridStart + daysInMonth);

      Events = new Dictionary<int, List<EventViewModel>>(daysInMonth);
      for(var day = 1; day <= daysInMonth; day++)
      {
        Events.Add(day, new List<EventViewModel>());
      }
    }

    public DateTime CurrentDate { get; private set; }

    public int GridStart { get; private set; }

    public int GridEnd { get; private set; }

    public int DaysInMonth { 
      get 
      { 
        return DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month); 
      } 
    }

    public Dictionary<int, List<EventViewModel>> Events { get; private set; }
  }
}
