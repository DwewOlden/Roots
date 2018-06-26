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
using roots.SupportingSystems.DriverSystem;

namespace roots.Functions
{
    [Activity(Label = "Journey", MainLauncher = false, Icon = "@drawable/xs")]
    public class JourneyActivity : Activity
    {
        private Spinner mListView;
        private BaseAdapter<Driver> mAdapter;
        private List<Driver> mDrivers;
        private DriverRepository driverRepository;
        private Action<Driver> mDriverSelected;


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

            mListView = FindViewById<Spinner>(Resource.Id.driverSelectSpinner);
            mDrivers = new List<Driver>();
            mDriverSelected = null;
            mDriverSelected = DriverSelected;

            mAdapter = new DriverSpinnerAdapter(this, Resource.Layout.DriverJourneyListViewRow, mDrivers, mDriverSelected);
            mListView.Adapter = mAdapter;
            PopulateDriverList();

        }

        private void PopulateDriverList()
        {
            mAdapter.Dispose();
            mAdapter = null;
           

            if (driverRepository == null)
                driverRepository = new DriverRepository();

            mDrivers = driverRepository.GetAllDrivers();

            mAdapter = new DriverSpinnerAdapter(this, Resource.Layout.DriverJourneyListViewRow, mDrivers, mDriverSelected);
            mListView.Adapter = mAdapter;

        }

        private void DriverSelected(Driver obj)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} has been selected", obj.Name));
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
                    StartActivity(typeof(JourneyActivity));
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