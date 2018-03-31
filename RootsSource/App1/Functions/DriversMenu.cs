using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using roots.SupportingSystems.DriverSystem;

namespace roots.TabViews
{
    [Activity(Label = "Driver", MainLauncher = false, Icon = "@mipmap/icon")]
    public class DriversMenu : Activity
    {
        private ListView mListView;
        private BaseAdapter<Driver> mAdapter;
        private List<Driver> mDrivers;
        private ImageView mSelectedImage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainDriverScreen);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            Action<ImageView> action = PicSelected;

            mListView = FindViewById<ListView>(Resource.Id.DriversListView);
            mListView.ItemClick += MListView_ItemClick;
            mDrivers = new List<Driver>();

            mAdapter = new DriverListAdapter(this, Resource.Layout.DriverListViewRow, mDrivers,action);
            mListView.Adapter = mAdapter;
            
            var editToolbar = FindViewById<Toolbar>(Resource.Id.driverMenu);
            editToolbar.InflateMenu(Resource.Menu.driver_menu);
            editToolbar.MenuItemClick += (sender, e) =>
            {

                string ContextMenuSelected = e.Item.TitleFormatted.ToString();

                switch (ContextMenuSelected)
                {
                    case "Add New Driver":
                        FragmentTransaction transaction = FragmentManager.BeginTransaction();
                        var dialog = new Forms.AddNewDriver();
                        dialog.OnCreateDriver += Dialog_OnCreateDriver;
                        dialog.Show(transaction, "dialog");
                        break;
                }
            };
        }

        private void PicSelected(ImageView imageView)
        {
            mSelectedImage = imageView;
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            this.StartActivityForResult(Intent.CreateChooser(intent, "Select a photo"), 0);

        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                System.IO.Stream stream = ContentResolver.OpenInputStream(data.Data);
                mSelectedImage.SetImageBitmap(BitmapFactory.DecodeStream(stream));
            }
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine(mDrivers[e.Position].DriverId + " " + mDrivers[e.Position].Name);
        }

        private void Dialog_OnCreateDriver(object sender, CreateDriverEventArgs e)
        {
            mDrivers.Add(new Driver() { Name = e.DriverName, DriverId = e.DriverId });
            mAdapter.NotifyDataSetChanged();
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