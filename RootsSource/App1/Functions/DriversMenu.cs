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
using roots.SupportingSystems.Data;
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
        private DriverRepository driverRepository;

        private Action<ImageView> action;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainDriverScreen);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            action = PicSelected;

            mListView = FindViewById<ListView>(Resource.Id.DriversListView);
            mListView.ItemClick += MListView_ItemClick;
            mDrivers = new List<Driver>();

            mAdapter = new DriverListAdapter(this, Resource.Layout.DriverListViewRow, mDrivers, action);
            mListView.Adapter = mAdapter;
            PopulateDriverList();


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

        private void PopulateDriverList()
        {
            if (driverRepository == null)
                driverRepository = new DriverRepository();

            mDrivers = driverRepository.GetAllDrivers();

            mAdapter = new DriverListAdapter(this, Resource.Layout.DriverListViewRow, mDrivers, action);
            mListView.Adapter = mAdapter;
        }

        //public override void OnWindowFocusChanged(bool hasFocus)
        //{
        //    base.OnWindowFocusChanged(hasFocus);

        //    if (hasFocus)
        //        PopulateDriverList();
        //}

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                System.IO.Stream stream = ContentResolver.OpenInputStream(data.Data);
                mSelectedImage.SetImageBitmap(DecodeBitmap(data.Data,100,100));
            }
        }

        private Bitmap DecodeBitmap(Android.Net.Uri dataRequested, int requestedWidth,int RequestedHeight)
        {
            System.IO.Stream s = ContentResolver.OpenInputStream(dataRequested);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeStream(s);

            options.InSampleSize = CalculateScalingFactors(options, RequestedHeight, requestedWidth);

            s = ContentResolver.OpenInputStream(dataRequested);
            options.InJustDecodeBounds = false;

            Bitmap bitmap = BitmapFactory.DecodeStream(s, null, options);

            return bitmap;


        }

        private int CalculateScalingFactors(BitmapFactory.Options options,int RequestedSizeHeight,int RequestedSizeWidth)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > RequestedSizeHeight || width > RequestedSizeWidth)
            {
                int HalfHeight = height / 2;
                int HalfWidth = height / 2;

                while ((HalfHeight / inSampleSize) > RequestedSizeHeight && (HalfWidth / inSampleSize) > RequestedSizeWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;


        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine(mDrivers[e.Position].DriverId + " " + mDrivers[e.Position].Name);
        }

        private void Dialog_OnCreateDriver(object sender, CreateDriverEventArgs e)
        {
            mDrivers.Add(new Driver() { Name = e.DriverName, DriverId = e.DriverId });

            if (driverRepository == null)
                driverRepository = new DriverRepository();
            
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
                case "Edit":
                    StartActivity(typeof(roots.Functions.JourneyActivity));
                    break;

                case "Drivers":
                    StartActivity(typeof(TabViews.DriversMenu));
                    break;

                case "Trips":
                    StartActivity(typeof(TabViews.TripList));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}