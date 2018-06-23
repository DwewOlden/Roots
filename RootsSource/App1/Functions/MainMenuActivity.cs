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
    public class MainMenuActivity:Activity
    {
        private DatabaseCreation databaseCreation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var toolbar = base.FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            CreateSupportingDatasets();
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