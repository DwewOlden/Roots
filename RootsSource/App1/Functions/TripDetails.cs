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

        private JourneyRepository JourneyRepository_;

        private TextView distanceView;
        private TextView timeView;
        private TextView tripView;

        private Button button_viewJourneyDetails;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            tripRepository_ = new TripRepository();
            JourneyRepository_ = new JourneyRepository();

            base.OnCreate(savedInstanceState);
            SelectedTripId = Intent.Extras.GetInt("tripId");

            SetContentView(Resource.Layout.TripDetails);

            button_viewJourneyDetails = FindViewById<Button>(Resource.Id.buttonViewJourneys);
            button_viewJourneyDetails.Click += Button_viewJourneyDetails_Click;

            var makeActiveButton = base.FindViewById<Button>(Resource.Id.buttonMakeTripActive);
            makeActiveButton.Click += MakeActiveButton_Click;

            PopulateOnScreenInformation();
        }

        private void PopulateOnScreenInformation()
        {
            distanceView = FindViewById<TextView>(Resource.Id.tripDetailsMilesTraveledDetails);
            timeView = FindViewById<TextView>(Resource.Id.tripDetailsTimesTraveledDetails);
            tripView = FindViewById<TextView>(Resource.Id.tripDetailsTripsTraveledDetails);
            
            // Get the data
            JourneyRepository_.GetTripTotals(SelectedTripId, out TimeSpan span, out double distance, out int journeys);

            // Display the data
            tripView.Text = Convert.ToString(journeys);
            distanceView.Text = Convert.ToString(Math.Round((distance / 1.61), 2));
            timeView.Text = span.ToString(@"hh\:mm");
            
        }

        private void MakeActiveButton_Click(object sender, EventArgs e)
        {
            tripRepository_.SetActiveTrip(SelectedTripId);
        }

        private void Button_viewJourneyDetails_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(JourneyList));
            intent.PutExtra("tripId", SelectedTripId);
            StartActivity(intent);
        }
    }
}