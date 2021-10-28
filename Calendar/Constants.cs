using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Calendar
{
  public static class Constants
  {
    public static readonly IList<String> AbbreviatedDays = new ReadOnlyCollection<String>(new List<String> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" });
  }
}
