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

namespace roots.SupportingSystems.Trips
{
    public class CreateTripEventArgs:EventArgs
    {
        public int TripId { get; set; }

        public string TripName { get; set; }

        public string TripDescription { get; set; }

        public string TripWhen { get; set; }

        public CreateTripEventArgs()
        {

        }

        public CreateTripEventArgs(int pId,string pName,string pDescription,string pWhen)
        {
            TripId = pId;
            TripName = pName;
            TripDescription = pDescription;
            TripWhen = pWhen;
        }
    }
}