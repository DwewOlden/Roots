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
    [Activity(Label = "Selected Journey Details")]
    public class DisplayJourneyDetails : Activity
    {
        private int SelectedJourneyId = int.MinValue; 

        private JourneyRepository journeyRepository;

        TextView textDriverName;
        TextView textDuration;
        TextView textEndPoint;
        TextView textStartTime;
        TextView textDistance;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            journeyRepository = new JourneyRepository();
            base.OnCreate(savedInstanceState);
            SelectedJourneyId = Intent.Extras.GetInt("journeyId");

            SetContentView(Resource.Layout.DisplayJourneyDetails);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            GetTextValues();
            DrawJourneyDetails();

        }

        private void GetTextValues()
        {
            textDistance = FindViewById<TextView>(Resource.Id.journey_details_distance_value);
            textDriverName = FindViewById<TextView>(Resource.Id.journey_details_driver_value);
            textDuration = FindViewById<TextView>(Resource.Id.journey_details_duration_value);
            textEndPoint = FindViewById<TextView>(Resource.Id.journey_details_endpoint_value);
            textStartTime = FindViewById<TextView>(Resource.Id.journey_details_starttime_value);
        }

        private void DrawJourneyDetails()
        {
            var dto = journeyRepository.GetSpecificJourneyStats(SelectedJourneyId);

            textStartTime.Text = dto.Starting.ToString("HH:mm");
            textDriverName.Text = dto.DriverName;
            textEndPoint.Text = dto.EndPoint;
            textDistance.Text = string.Format("{0} Miles", (dto.Distance / 1.61));
            textDuration.Text = string.Format("{0} Hours {1} Minutes", dto.Duration.Hours, dto.Duration.Minutes);
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenuItems, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            PerformSelectedMenuFunction(item);

            return base.OnOptionsItemSelected(item);
        }

        private void PerformSelectedMenuFunction(IMenuItem item)
        {
            string titleFormatted = item.TitleFormatted.ToString();
            switch (titleFormatted)
            {
                case "Edit":
                    StartActivity(typeof(JourneyActivity));
                    break;

                case "Drivers":
                    StartActivity(typeof(TabViews.DriversMenu));
                    break;

                case "Trips":
                    StartActivity(typeof(TabViews.TripList));
                    break;

                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
               ToastLength.Short).Show();
                    break;
            }
        }
    }
}