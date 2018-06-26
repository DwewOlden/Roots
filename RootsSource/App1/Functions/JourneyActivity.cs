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
    [Activity(Label = "JourneyActivity")]
    public class JourneyActivity : Activity
    {
        /// <summary>
        /// An instance of the trip repository
        /// </summary>
        private TripRepository tripRepository;

        /// <summary>
        /// An instance of the trip id
        /// </summary>
        private int TripId { get; set; }

        private bool TripIsRecording { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.JourneyLayout);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
                if (!TripIsRecording)
                {
                    if (tripRepository == null)
                        tripRepository = new TripRepository();

                    TripId = tripRepository.GetActiveTrip();
                }
        }
    }
}