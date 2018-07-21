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
    public class JourneyDetailsDTO
    {
        public string DriverName { get; set; }

        public double Distance { get; set; }

        public string EndPoint { get; set; }

        public DateTime Starting { get; set; }

        public TimeSpan Duration { get; set; }

        public int Id { get; set; }

        public JourneyDetailsDTO()
        {

        }
    }
}