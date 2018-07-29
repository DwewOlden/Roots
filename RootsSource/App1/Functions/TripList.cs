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
using roots.Functions;
using roots.SupportingSystems;
using roots.SupportingSystems.Data;
using roots.SupportingSystems.Trips;

namespace roots.TabViews
{
    [Activity(Label = "Trips", MainLauncher = false, Icon = "@mipmap/icon")]
    public class TripList : Activity
    {
        private ListView mListView;
        private List<Trip> mTrips;
        private BaseAdapter<Trip> mAdapter;
        private TripRepository tripRepository;

        Action<TextView> action;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainTripScreen);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            mListView = FindViewById<ListView>(Resource.Id.TripListView);
            mListView.ItemClick += MListView_ItemClick;
            mTrips = new List<Trip>();

           

            action = TripSelected;
            
            var editToolbar = FindViewById<Toolbar>(Resource.Id.tripMenu);
            editToolbar.InflateMenu(Resource.Menu.trip_menu);
            editToolbar.MenuItemClick += (sender, e) =>
            {

                string ContextMenuSelected = e.Item.TitleFormatted.ToString();

                switch (ContextMenuSelected)
                {
                    case "Add New Trip":
                        StartActivity(typeof(AddNewTrip));
                        break;
                    
                }
            };
        }

       
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
                PopulateTripList();
        }


        private void PopulateTripList()
        {
            if (tripRepository == null)
                tripRepository = new TripRepository();

            mTrips = tripRepository.GetAllTrips();

            mAdapter = new TripListAdapter(this, Resource.Layout.TripListViewRow, mTrips, action);
            mListView.Adapter = mAdapter;
        }

        private void TripSelected(TextView textView)
        {
            System.Diagnostics.Debug.WriteLine(textView.Text);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;
            var trip = mTrips[pos];
            var tripId = trip.TripId;

            var intent = new Intent(this, typeof(TripDetails));
            intent.PutExtra("tripId", tripId);
            StartActivity(intent);
        }

        private void Dialog_OnCreateTrip(object sender, CreateTripEventArgs e)
        {
            mTrips.Add(new Trip() { Name = e.TripName, Description = e.TripDescription, When = e.TripWhen, TripId = e.TripId });
            mAdapter.NotifyDataSetChanged();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenuItems, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string titleFormatted = item.TitleFormatted.ToString();
            switch (titleFormatted)
            {
                case "Edit":
                    Intent intent = new Intent(this.ApplicationContext, typeof(JourneyActivity));
                    intent.SetFlags(ActivityFlags.SingleTop);
                    StartActivity(intent);
                    break;

                case "Save":
                    StartActivity(typeof(reporting_section));
                    break;

                case "Drivers":
                    StartActivity(typeof(TabViews.DriversMenu));
                    break;

                case "Trips":
                    StartActivity(typeof(TabViews.TripList));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}