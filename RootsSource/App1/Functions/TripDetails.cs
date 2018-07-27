using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using roots.SupportingSystems;
using roots.SupportingSystems.Data;

namespace roots.Functions
{
    [Activity(Label = "Trips", MainLauncher = false, Icon = "@mipmap/icon")]
    public class TripDetails : Activity
    {
        private ImageView titleImageView;
        private Bitmap titleBitmap;

        private ProgressDialog _progressDialog;

        private Thread _exportThread;

        /// <summary>
        /// The trip id
        /// </summary>
        private int SelectedTripId { get; set; }

        private TripRepository tripRepository_;

        private JourneyRepository JourneyRepository_;

        private TextView distanceView;
        private TextView timeView;
        private TextView tripView;

        private Button button_viewJourneyDetails;

        private MyFileWriter _fileWriter;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            tripRepository_ = new TripRepository();
            JourneyRepository_ = new JourneyRepository();

            base.OnCreate(savedInstanceState);
            SelectedTripId = Intent.Extras.GetInt("tripId");

            SetContentView(Resource.Layout.TripDetails);
            DrawTopImageInPurple();
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            //button_viewJourneyDetails = FindViewById<Button>(Resource.Id.buttonViewJourneys);
            //button_viewJourneyDetails.Click += Button_viewJourneyDetails_Click;

            //var makeActiveButton = base.FindViewById<Button>(Resource.Id.buttonMakeTripActive);
            //makeActiveButton.Click += MakeActiveButton_Click;

            PopulateOnScreenInformation();

            var editToolbar = FindViewById<Toolbar>(Resource.Id.tripInstanceMenu);
            editToolbar.InflateMenu(Resource.Menu.trip_instance_menu);
            editToolbar.MenuItemClick += (sender, e) =>
            {

                string ContextMenuSelected = e.Item.TitleFormatted.ToString();
                FragmentTransaction transaction = FragmentManager.BeginTransaction();

                switch (ContextMenuSelected)
                {
                    case "View Journeys":
                        var intent = new Intent(this, typeof(JourneyList));
                        intent.PutExtra("tripId", SelectedTripId);
                        StartActivity(intent);
                        break;
                    case "Make Active":
                        tripRepository_.SetActiveTrip(SelectedTripId);
                        break;
                    case "Add Manual Journey":
                        var NewJourneyIntent = new Intent(this, typeof(InsertNewManualJourney));
                        StartActivity(NewJourneyIntent);
                        break;
                    case "Set Start Point":
                        var startpointdialog = new Forms.set_start_point_fragment();
                        startpointdialog.NoteAdded += Startpointdialog_NoteAdded;
                        startpointdialog.Show(transaction, "dialog");
                        break;
                    case "Export Data To SD":
                        PerformExport();
                        break;
                        

                }
            };
        }

        private void PerformExport()
        {
            _exportThread = new Thread(PerformExportWork);
            _exportThread.Start();

            _progressDialog = new Android.App.ProgressDialog(this);
            _progressDialog.Indeterminate = true;
            _progressDialog.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            _progressDialog.SetMessage("Exporting Is In Progress");
            _progressDialog.SetCancelable(false);
            _progressDialog.Show();
        }

        private void PerformExportWork()
        {
            _fileWriter = new MyFileWriter();

            try
            {
                System.Diagnostics.Debug.WriteLine("Getting Start Information");

                string startPoint = tripRepository_.GetStartPointFromTrip(SelectedTripId);
                System.Threading.Thread.Sleep(300);

                var recordList = JourneyRepository_.GetExportDataSet(SelectedTripId);
                System.Diagnostics.Debug.WriteLine(String.Format("There are {0} records in the list", recordList.Count()));

                _fileWriter.WriteLine(ExportingTripRecord.GetHeader());

                foreach (var record in recordList)
                {
                    record.StartPoint = startPoint;
                    _fileWriter.WriteLine(record.GetAsString());
                    startPoint = record.EndPoint;
                    System.Threading.Thread.Sleep(50);
                }

                System.Threading.Thread.Sleep(1 * 1000);

                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Export Completed", ToastLength.Long).Show();
                });
                
            }
            catch (Exception)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Export Completed", ToastLength.Long).Show();
                });
            }
            finally
            {
                _progressDialog.Dismiss();
                
            }

            
           


            





           

        }

        private void Startpointdialog_NoteAdded(object sender, SupportingSystems.GenericNoteEventArgs e)
        {
            bool Outcome = tripRepository_.UpdateWithStartPlace(SelectedTripId, e.Notes);
            DisplayToast(Outcome);
        }

        private void DisplayToast(bool outcome)
        {
            if (outcome)
                Toast.MakeText(this, "Note Was Added", ToastLength.Long).Show();
            else
                Toast.MakeText(this, "Acchh, wee gremlins have been at us!", ToastLength.Long).Show();
        }

        private void PopulateOnScreenInformation()
        {
            distanceView = FindViewById<TextView>(Resource.Id.tripDetailsMilesTraveledDetails);
            timeView = FindViewById<TextView>(Resource.Id.tripDetailsTimesTraveledDetails);
            tripView = FindViewById<TextView>(Resource.Id.tripDetailsTripsTraveledDetails);
            
            // Get the data
            JourneyRepository_.GetTripTotals(SelectedTripId, out TimeSpan span, out double distance, out int journeys);

            // Display the data
            tripView.Text = Convert.ToString(journeys);
            distanceView.Text = Convert.ToString(Math.Round((distance / 1.61), 2));
            timeView.Text = span.ToString(@"hh\:mm");
            
        }

        private void MakeActiveButton_Click(object sender, EventArgs e)
        {
            
            
        }

        private void Button_viewJourneyDetails_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(JourneyList));
            intent.PutExtra("tripId", SelectedTripId);
            StartActivity(intent);
        }

        public static Bitmap ApplyBitmapBrightness(int brightnessLevel, Bitmap bitmap)
        {
            // Create a temporary bitmap to preserve the original image for the final draw
            Bitmap brightnessAdjustedBitmap = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
            Canvas c = new Canvas(brightnessAdjustedBitmap);
            Paint paint = new Paint();
            ColorMatrix colorMatrix = new ColorMatrix();

            // Apply the brightness filter and draw the final bitmap using 
            // the paint object and the origin bitmap
            ColorMatrixColorFilter brightnessFilter = AdjustBrightness(brightnessLevel);
            paint.SetColorFilter(brightnessFilter);
            c.DrawBitmap(bitmap, 0, 0, paint);
            return brightnessAdjustedBitmap;
        }

        private void DrawTopImageInPurple()
        {
            titleImageView = (ImageView)FindViewById(Resource.Id.myImageViewV3);
            Android.Graphics.Drawables.BitmapDrawable bd = (Android.Graphics.Drawables.BitmapDrawable)titleImageView.Drawable;
            titleBitmap = bd.Bitmap;
            var vv11 = ApplyBitmapBrightness(23, titleBitmap);

            titleImageView.SetImageBitmap(vv11);
        }

        public static ColorMatrixColorFilter AdjustBrightness(int BrightnessLevel)
        {

            ColorMatrix matrix = new ColorMatrix();

            // This is essentially an identity matrix that adjusts colors based on the fourth 
            // element of each row of the matrix
            matrix.Set(new float[] {
            1F, 0, 0, 0, 0.4F,
            0, 0, 0, 0,0,
            0, 0, 1F, 0, 0.4F,
            0, 0, 0, 0.8F, 0 });

            ColorMatrixColorFilter brightnessFilter = new ColorMatrixColorFilter(matrix);
            return brightnessFilter;
        }
    }
}