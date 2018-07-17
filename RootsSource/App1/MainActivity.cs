using Android.App;
using Android.Widget;
using Android.OS;
using roots.SupportingSystems.Data;
using System;
using Android.Views;
using Android.Content;
using roots.Functions;
using Android.Graphics;

namespace roots
{
    [Activity(Label = "Roots", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainActivity : Activity
    {
        private ImageView im;
        private Bitmap bmp;
        private Bitmap operation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }




    }
}

