﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
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
        private ImageView titleImageView;
        private Bitmap titleBitmap;

        private int SleepPeriodInMilliSeconds = 5 * 1000;
        System.Timers.Timer timer;

        // Journey related information
        private int SelectedDriverId = int.MinValue;
        private int NewJourneyId;
        private DateTime JourneyStarted;
        private DateTime JourneyEnded;
        private TimeSpan timeToday = new TimeSpan();
        private bool JourneyIsInProgress = false;
        private Location previousLocation = null;
        private Haversine HaversineCalculator;
        private double distance = 0.0;
        private double todayDistance = 0.0;
        private int JourneyPointIndex = 1;

        private Spinner mListView;
        private BaseAdapter<Driver> mAdapter;
        private List<Driver> mDrivers;
        private DriverRepository driverRepository;
        private Action<Driver> mDriverSelected;


        private JourneyRepository JourneyRepository;
        private JourneyPointRespository journeyPointRespository;

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
            JourneyRepository = new JourneyRepository();
            journeyPointRespository = new JourneyPointRespository();

            NewJourneyId = int.MinValue;
            timer = new System.Timers.Timer(SleepPeriodInMilliSeconds);
            timer.Elapsed += Timer_Elapsed;

            HaversineCalculator = new Haversine();

            SetContentView(Resource.Layout.JourneyLayout);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            DrawTopImageInPurple();

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
            DateTime now = DateTime.Now;
            string timeOnTrip = TimeFormatter.FormatTimes(now, JourneyStarted);
            string timeTravelledToday = TimeFormatter.FormatTimes(timeToday, now, JourneyStarted);

            var TripTime = FindViewById<TextView>(Resource.Id.lblTimeData);
            var TotalTodayTime = FindViewById<TextView>(Resource.Id.lblDayTimeData);
            var DistanceTextView = FindViewById<TextView>(Resource.Id.lblDistanceData);
            var TodayDistanceTextVoew = FindViewById<TextView>(Resource.Id.lblDayDistanceData);

            RunOnUiThread(() => {
                TripTime.Text = timeOnTrip;
                TotalTodayTime.Text = timeTravelledToday;

                if (distance > 0)
                {
                    DistanceTextView.Text = Convert.ToString(Math.Round((distance / 1.61), 1));
                    TodayDistanceTextVoew.Text = Convert.ToString(Math.Round((todayDistance / 1.61), 1));
                }
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
            System.Diagnostics.Debug.WriteLine(string.Format("New Journey Id: {0} ",NewJourneyId));
            if (previousLocation != null)
            {
                distance = Math.Round(distance + HaversineCalculator.CalculateHaversineDistiance(e.Location, previousLocation), 5);
                todayDistance = Math.Round(todayDistance + HaversineCalculator.CalculateHaversineDistiance(e.Location, previousLocation), 5);

                if (NewJourneyId != int.MinValue)
                {
                    JourneyRepository.UpdateJourneyDetails(NewJourneyId, distance);
                    journeyPointRespository.InsertPointIntoTrack(NewJourneyId, JourneyPointIndex, e.Location.Latitude, e.Location.Longitude);
                }
            }
           
            previousLocation = e.Location;
            JourneyPointIndex = JourneyPointIndex + 1;
            
        }

        private void JourneyButton_Click(object sender, EventArgs e)
        {
            JourneyPointIndex = 1;

            if (JourneyRepository == null)
                JourneyRepository = new JourneyRepository();
            
            JourneyIsInProgress = !JourneyIsInProgress;
            var journeyButton = FindViewById<Button>(Resource.Id.btnMainJourneyButton);
            var spinner = FindViewById<Spinner>(Resource.Id.driverSelectSpinner);

            if (JourneyIsInProgress)
            {
                if (TripId == int.MinValue)
                {
                    Toast.MakeText(this, "Set An Active Trip", ToastLength.Long).Show();
                    return;
                }

                distance = 0;
                JourneyRepository.CountMetricToday(TripId, out todayDistance, out timeToday);
                
                NewJourneyId = JourneyRepository.StartNewJourney(SelectedDriverId, TripId);
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

                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                var dialog = new Forms.AddNewStopPoint();
                dialog.JourneyId = NewJourneyId;
                dialog.OnGetPlaceName += Dialog_OnGetPlaceName; 
                dialog.Show(transaction, "dialog");
                NewJourneyId = int.MinValue;
                previousLocation = null;       
            }
        }

        private void Dialog_OnGetPlaceName(object sender, SupportingSystems.CreateStopLocationEventArgs e)
        {
            JourneyRepository.UpdateWithEndPoint(e.TripId, e.PlaceName);
        }

        private void MListView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            SelectedDriverId = mDrivers[e.Position].DriverId;
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

        public static Bitmap ApplyBitmapBrightness(int brightnessLevel, Bitmap bitmap)
        {
            // Create a temporary bitmap to preserve the original image for the final draw
            Bitmap brightnessAdjustedBitmap = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
            Canvas c = new Canvas(brightnessAdjustedBitmap);
            Paint paint = new Paint();
            ColorMatrix colorMatrix = new ColorMatrix();

            // Apply the brightness filter and draw the final bitmap using 
            // the paint object and the origin bitmap
            ColorMatrixColorFilter brightnessFilter = AdjustBrightness(brightnessLevel);
            paint.SetColorFilter(brightnessFilter);
            c.DrawBitmap(bitmap, 0, 0, paint);
            return brightnessAdjustedBitmap;
        }

        private void DrawTopImageInPurple()
        {
            titleImageView = (ImageView)FindViewById(Resource.Id.myImageViewV2);
            Android.Graphics.Drawables.BitmapDrawable bd = (Android.Graphics.Drawables.BitmapDrawable)titleImageView.Drawable;
            titleBitmap = bd.Bitmap;
            var vv11 = ApplyBitmapBrightness(23, titleBitmap);

            titleImageView.SetImageBitmap(vv11);
        }

        public static ColorMatrixColorFilter AdjustBrightness(int BrightnessLevel)
        {

            ColorMatrix matrix = new ColorMatrix();

            // This is essentially an identity matrix that adjusts colors based on the fourth 
            // element of each row of the matrix
            matrix.Set(new float[] {
            1F, 0, 0, 0, 0.4F,
            0, 0, 0, 0,0,
            0, 0, 1F, 0, 0.4F,
            0, 0, 0, 0.8F, 0 });

            ColorMatrixColorFilter brightnessFilter = new ColorMatrixColorFilter(matrix);
            return brightnessFilter;
        }
    }
}