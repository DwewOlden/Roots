using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Support
{
    public class TimeFormatter
    {
        public static string FormatTimes(DateTime current,DateTime starting)
        {
            TimeSpan timeSpan = current.Subtract(starting);
            return timeSpan.ToString(@"hh\:mm");
        }

        public static string FormatTimes(TimeSpan baseSpan,DateTime current, DateTime starting)
        {
            TimeSpan timeSpan = current.Subtract(starting);
            timeSpan = baseSpan.Add(timeSpan);
            return timeSpan.ToString(@"hh\:mm");
        }
    }
}
