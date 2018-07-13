using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Support
{
    public class SQLLiteDateTimes
    {
        public static string DateTimeSQLite(DateTime datetime)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, datetime.Year, Convert.ToString(datetime.Month).PadLeft(2, '0'), Convert.ToString(datetime.Day).PadLeft(2, '0'),
                Convert.ToString(datetime.Hour).PadLeft(2, '0'), Convert.ToString(datetime.Minute).PadLeft(2, '0'),
                Convert.ToString(datetime.Second).PadLeft(2, '0'), datetime.Millisecond);
        }
    }
}
