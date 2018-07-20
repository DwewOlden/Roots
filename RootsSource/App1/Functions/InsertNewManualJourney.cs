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

namespace roots.Functions
{
    [Activity(Label = "Insert A Journey")]
    public class InsertNewManualJourney : Activity
    {
        private Button myButton;
        private bool bothSet = false;
        private int currentlySetting = int.MinValue;

        DateTime x1 = DateTime.MinValue;
        DateTime x2 = DateTime.MinValue;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddManualJourney);

            var toolbar = base.FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            myButton = FindViewById<Button>(Resource.Id.add_journey_button);
            myButton.Click += MyButton_Click;


        }

        private void MyButton_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            set_time_dialog dialog = new set_time_dialog();
            dialog.OnTimeSet += Dialog_OnTimeSet;
            dialog.Show(fragmentTransaction, "settime");
            
        }

        private void Dialog_OnTimeSet(object sender, SupportingSystems.SelectedTimeEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ToString());
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