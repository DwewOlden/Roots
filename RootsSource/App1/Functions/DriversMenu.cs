using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace roots.TabViews
{
    [Activity(Label = "Driver", MainLauncher = false, Icon = "@mipmap/icon")]
    public class DriversMenu : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settingsTab);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            var editToolbar = FindViewById<Toolbar>(Resource.Id.edit_toolbar);
            editToolbar.InflateMenu(Resource.Menu.driver_menu);
            editToolbar.MenuItemClick += (sender, e) => {

                string ContextMenuSelected = e.Item.TitleFormatted.ToString();

                switch (ContextMenuSelected)
                {
                    case "Add New Driver":
                        FragmentTransaction transaction = FragmentManager.BeginTransaction();
                        var dialog = new Forms.AddNewDriver();
                        dialog.Show(transaction, "dialog");
                        break;
                }


            };

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
                case "Drivers":
                    StartActivity(typeof(TabViews.DriversMenu));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}