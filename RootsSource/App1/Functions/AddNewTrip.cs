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
    [Activity(Label = "Adding a Trip", MainLauncher = false, Icon = "@mipmap/icon")]
    public class AddNewTrip : Activity
    {
        private TripRepository tripReposiory;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddTripScreen);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            var addTripButton = FindViewById<Button>(Resource.Id.AddTripButton);
            addTripButton.Click += AddTripButton_Click;
            
        }

        

        private void AddTripButton_Click(object sender, EventArgs e)
        {
            if (tripReposiory == null)
                tripReposiory = new TripRepository();

            var name = FindViewById<TextView>(Resource.Id.editTripName).Text;
            var description = FindViewById<TextView>(Resource.Id.editTripDescription).Text;
            var when = FindViewById<TextView>(Resource.Id.editTripWhen).Text;

            if (!tripReposiory.InsertNewTrip(name, description, when))
                Toast.MakeText(this, "Unable To Add Trip", ToastLength.Long).Show();
            else
                Toast.MakeText(this, "Trips and outings. Yay.", ToastLength.Long).Show() ;


        }
    }
}