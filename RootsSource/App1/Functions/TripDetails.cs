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

namespace roots.Functions
{
    [Activity(Label = "Trips", MainLauncher = false, Icon = "@mipmap/icon")]
    public class TripDetails : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var selectedTripId = Intent.Extras.GetInt("tripId");

            SetContentView(Resource.Layout.TripDetails);
            
        }
    }
}