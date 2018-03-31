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
    [Activity(Label = "Roots", MainLauncher = true, Icon = "@drawable/xs")]
    public class MainActivity : MainMenuActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }




    }
}

