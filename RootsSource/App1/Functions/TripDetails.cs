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
using roots.SupportingSystems.Data;

namespace roots.Functions
{
    [Activity(Label = "Trips", MainLauncher = false, Icon = "@mipmap/icon")]
    public class TripDetails : Activity
    {
        /// <summary>
        /// The trip id
        /// </summary>
        private int SelectedTripId { get; set; }

        private TripRepository tripRepository_;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            tripRepository_ = new TripRepository();

            base.OnCreate(savedInstanceState);
            SelectedTripId = Intent.Extras.GetInt("tripId");

            SetContentView(Resource.Layout.TripDetails);

            var makeActiveButton = base.FindViewById<Button>(Resource.Id.buttonMakeTripActive);
            makeActiveButton.Click += MakeActiveButton_Click;
            
        }

        private void MakeActiveButton_Click(object sender, EventArgs e)
        {
            tripRepository_.SetActiveTrip(SelectedTripId);
        }
    }
}