using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using roots.Services;
using roots.SupportingSystems.Data;
using roots.SupportingSystems.DriverSystem;
using Roots.Support;

namespace roots.Functions
{
    [Activity(Label = "Journey", MainLauncher = false, Icon = "@drawable/xs")]
    public class JourneyActivity : Activity
    {
        private int SleepPeriodInMilliSeconds = 5 * 1000;
        System.Timers.Timer timer;

        // Journey related information
        private int SelectedDriverId = int.MinValue;
        private DateTime JourneyStarted;
        private DateTime JourneyEnded;
        private bool JourneyIsInProgress = false;

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
            timer = new System.Timers.Timer(SleepPeriodInMilliSeconds);
            timer.Elapsed += Timer_Elapsed;

            SetContentView(Resource.Layout.JourneyLayout);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            mListView = FindViewById<Spinner>(Resource.Id.driverSelectSpinner);
            mListView.ItemSelected += MListView_ItemSelected;
            mDrivers = new List<Driver>();
            mDriverSelected = null;
            mDriverSelected = DriverSelected;

            mAdapter = new DriverSpinnerAdapter(this, Resource.Layout.DriverJourneyListViewRow, mDrivers, mDriverSelected);
            mListView.Adapter = mAdapter;
            PopulateDriverList();

            var journeyButton = FindViewById<Button>(Resource.Id.btnMainJourneyButton);
            journeyButton.Click += JourneyButton_Click;

            RootApp.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                // notifies us of location changes from the system
                RootApp.Current.LocationService.LocationChanged += HandleLocationChanged;
                //notifies us of user changes to the location provider (ie the user disables or enables GPS)
                RootApp.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                RootApp.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                // notifies us of the changing status of a provider (ie GPS no longer available)
                RootApp.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string Formatter = TimeFormatter.FormatTimes(DateTime.Now, JourneyStarted);

            var TimeTextView = FindViewById<TextView>(Resource.Id.lblTimeData);

            RunOnUiThread(() => {
                TimeTextView.Text = Formatter;
            });


        }

        private void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Status");
        }

        private void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Enabled");
        }

        private void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Disabled");
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Location location = e.Location;
            System.Diagnostics.Debug.WriteLine(location.Latitude + " " + location.Longitude);
        }

        private void JourneyButton_Click(object sender, EventArgs e)
        {
            JourneyIsInProgress = !JourneyIsInProgress;
            var journeyButton = FindViewById<Button>(Resource.Id.btnMainJourneyButton);
            var spinner = FindViewById<Spinner>(Resource.Id.driverSelectSpinner);

            if (JourneyIsInProgress)
            {
                JourneyStarted = DateTime.Now;
                journeyButton.Text = "End Journey";
                spinner.Enabled = false;
                timer.Enabled = true;
                RootApp.StartLocationService();

            }
            else
            {
                RootApp.StopLocationService();
                JourneyEnded = DateTime.Now;
                journeyButton.Text = "Start Journey";
                spinner.Enabled = true;
                timer.Enabled = false;
            }
        }

        private void MListView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} has been selected......", mDrivers[e.Position].Name));

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