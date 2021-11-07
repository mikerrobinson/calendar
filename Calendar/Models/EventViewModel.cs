using System;

namespace Calendar.Models
{
  public class EventViewModel
  {
    public TimeSpan Duration { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string TimeRangeDescription { 
      get
      {
        var startRange = Start.Minute == 0 ? Start.ToString("%h") : Start.ToString("h:mm");
        var endRange = End.Minute == 0 ? End.ToString("%h") : End.ToString("h:mm");
        return $"{startRange}-{endRange}";
      } 
    }

    public string Description { get; set; }

    public string Notes { get; set; }

    public string Location { get; set; }
  }
}
