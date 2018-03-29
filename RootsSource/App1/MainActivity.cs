using Android.App;
using Android.Widget;
using Android.OS;
using roots.SupportingSystems.Data;
using System;
using Android.Views;
using Android.Content;

namespace roots
{
    [Activity(Label = "Roots", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        private DatabaseCreation databaseCreation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            CreateSupportingDatasets();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string titleFormatted = item.TitleFormatted.ToString();
            switch (titleFormatted)
            {
                case "Edit":
                    StartActivity(typeof(TabViews.SettingTab));
                    break;

                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
               ToastLength.Short).Show();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void CreateSupportingDatasets()
        {
            databaseCreation = new DatabaseCreation();
            databaseCreation.InitaliseDatabase();
        }


    }
}

