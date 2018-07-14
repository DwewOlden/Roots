using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace roots.SupportingSystems.Journey
{
    public class Journey
    {
        public string EndPoint { get; set; }

        public DateTime Starting { get; set; }

        public DateTime Ending { get; set; }

        public double Distance { get; set; }

        public int JourneyId { get; set; }

        public int DriverId { get; set; }

        public int TripId { get; set; }

        public string DriverName { get; set; }

        public string JourneyTimes
        {
            get
            {
                return string.Format("{0} - {1}", Starting.ToShortTimeString(), Ending.ToShortTimeString());
            }
            
        }
    }
}