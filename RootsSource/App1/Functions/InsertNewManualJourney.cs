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
    [Activity(Label = "Insert A Journey")]
    public class InsertNewManualJourney : Activity
    {
        private int SelectedDriverId;
        private int SelectedTripId = int.MinValue;


        private JourneyRepository journeyRepository;
        private DriverRepository driverRepository;
        private TripRepository tripRepository;

        private Spinner mListView;
        private BaseAdapter<Driver> mAdapter;
        private List<Driver> mDrivers;

        private Action<Driver> mDriverSelected;


        EditText distanceText;
        EditText endLocationText;

        Button StartTimeButton;
        Button EndTimeButton;
        Button saveButton;

        TextView StartTimeText;
        TextView EndTimeText;

        bool startTimeSet = false;
        bool endTimeSet = false;
        private bool bothSet = false;
        private int currentlySetting = int.MinValue;

        DateTime selectedStartTime = DateTime.MinValue;
        DateTime selectedEndTime = DateTime.MinValue;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            tripRepository = new TripRepository();
            driverRepository = new DriverRepository();
            journeyRepository = new JourneyRepository();

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddManualJourney);

            var toolbar = base.FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            StartTimeButton = FindViewById<Button>(Resource.Id.manual_insert_starting_button);
            StartTimeButton.Click += StartTime_Click;

            EndTimeButton = FindViewById<Button>(Resource.Id.manual_insert_ending_button);
            EndTimeButton.Click += EndTimeButton_Click;

            StartTimeText = FindViewById<TextView>(Resource.Id.manual_insert_starting_label);
            EndTimeText = FindViewById<TextView>(Resource.Id.manual_insert_ending_label);

            distanceText = FindViewById<EditText>(Resource.Id.edit_distance_travelled);
            endLocationText = FindViewById<EditText>(Resource.Id.edit_endpoint_location);

            saveButton = FindViewById<Button>(Resource.Id.add_journey_button);
            saveButton.Click += SaveButton_Click;

            mListView = FindViewById<Spinner>(Resource.Id.driverSelectSpinner);
            mListView.ItemSelected += MListView_ItemSelected;
            mDrivers = new List<Driver>();
            mDriverSelected = null;
            mDriverSelected = DriverSelected;

            mAdapter = new DriverSpinnerAdapter(this, Resource.Layout.DriverJourneyListViewRow, mDrivers, mDriverSelected);
            mListView.Adapter = mAdapter;
            PopulateDriverList();

        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                SelectedTripId = tripRepository.GetActiveTrip();
            }
        }

        private void DriverSelected(Driver obj)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} has been selected", obj.Name));
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            double distanceValue = 0.0;


            if (!bothSet)
            {
                Toast.MakeText(this, "Both times must be set, you silly billy!", ToastLength.Long).Show();
                return;
            }

            var v = distanceText.Text;
            v = v.Trim();
            if (string.IsNullOrEmpty(v))
            {
                Toast.MakeText(this, "How faarrr did ye go!", ToastLength.Long).Show();
                return;
            }

            distanceValue = Convert.ToDouble(v);

            var k = endLocationText.Text;
            k = k.Trim();
            if (string.IsNullOrEmpty(k))
            {
                Toast.MakeText(this, "FFS, I place, I need a place!", ToastLength.Long).Show();
                return;
            }

            if (SelectedTripId == int.MinValue)
            {
                Toast.MakeText(this, "We do need an active trip we do!", ToastLength.Long).Show();
                return;
            }

            // Now we do the magic saving bit.
            journeyRepository.ManuallyInsertNewJourney(SelectedDriverId, SelectedTripId, selectedStartTime, selectedEndTime, distanceValue, k);







        }

        private void EndTimeButton_Click(object sender, EventArgs e)
        {
            currentlySetting = 2;
            OpenDialogFragment();
        }

        private void OpenDialogFragment()
        {
            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            set_time_dialog dialog = new set_time_dialog();
            dialog.OnTimeSet += Dialog_OnTimeSet;
            dialog.Show(fragmentTransaction, "settime");
        }

        private void StartTime_Click(object sender, EventArgs e)
        {
            currentlySetting = 1;
            OpenDialogFragment();
        }

        private void Dialog_OnTimeSet(object sender, SupportingSystems.SelectedTimeEventArgs e)
        {
            if (currentlySetting == 1)
            {
                selectedStartTime = e.SelectedTime;
                StartTimeText.Text = GetTimeString(e.SelectedTime);
                startTimeSet = true;

                if (endTimeSet)
                    bothSet = true;

            }

            if (currentlySetting == 2)
            {
                selectedEndTime = e.SelectedTime;
                EndTimeText.Text = GetTimeString(e.SelectedTime);
                endTimeSet = true;

                if (startTimeSet)
                    bothSet = true;
            }

            currentlySetting = int.MinValue;
        }

        private string GetTimeString(DateTime dateTime)
        {
            int Hour = dateTime.Hour;
            int Minute = dateTime.Minute;

            string HourString = Convert.ToString(Hour);
            string MinuteString = Convert.ToString(Minute);

            HourString = HourString.PadLeft(2, '0');
            MinuteString = MinuteString.PadLeft(2, '0');

            return string.Format("{0}:{1}", HourString, MinuteString);

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

                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
               ToastLength.Short).Show();
                    break;
            }
        }
    }
}