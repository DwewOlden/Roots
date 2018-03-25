using Android.App;
using Android.Widget;
using Android.OS;
using roots.SupportingSystems.Data;
using System;
using Android.Views;
using Android.Content;
using roots.Functions;

namespace roots
{
    [Activity(Label = "Roots", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : MainMenuActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var toolbar = base.FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }




    }
}

