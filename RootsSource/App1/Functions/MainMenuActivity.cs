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
    public class MainMenuActivity:Activity
    {
        private DatabaseCreation databaseCreation;
        private Spinner mListView;
        private BaseAdapter<Driver> mAdapter;
        private List<Driver> mDrivers;
        private DriverRepository driverRepository;
        private Action<Driver> mDriverSelected;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var toolbar = base.FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            CreateSupportingDatasets();

           
            mListView = FindViewById<Spinner>(Resource.Id.spinner);
            mDrivers = new List<Driver>();
            mDriverSelected = DriverSelected;

            mAdapter = new DriverSpinnerAdapter(this, Resource.Layout.DriverJourneyListViewRow, mDrivers,mDriverSelected);
            mListView.Adapter = mAdapter;
            PopulateDriverList();


        }

        private void DriverSelected(Driver obj)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} has been selected in the main", obj.Name));
        }

        private void MListView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var pos = e.Position;
            var driver = mDrivers[pos];

           
        }

        private void PopulateDriverList()
        {
            if (driverRepository == null)
                driverRepository = new DriverRepository();

            mDrivers = driverRepository.GetAllDrivers();

            mAdapter = new DriverSpinnerAdapter(this, Resource.Layout.DriverJourneyListViewRow, mDrivers,mDriverSelected);
            mListView.Adapter = mAdapter;
           
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

        private void CreateSupportingDatasets()
        {
            databaseCreation = new DatabaseCreation();
            databaseCreation.InitaliseDatabase();
        }



    }
}