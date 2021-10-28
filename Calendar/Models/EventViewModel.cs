using System;

namespace Calendar.Models
{
  public class EventViewModel
  {
    public TimeSpan Duration { get; set; }

    public string Description { get; set; }

    public string Notes { get; set; }

    public string Location { get; set; }
  }
}
