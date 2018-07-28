using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microcharts;
using Microcharts.Droid;
using roots.SupportingSystems.Data;
using SkiaSharp;

namespace roots.Functions
{
    [Activity(Label = "reporting_section")]
    public class reporting_section : Activity
    {
        private ImageView titleImageView;
        private Bitmap titleBitmap;
        private JourneyRepository journeyRepository;
        private TripRepository tripRepository;


        private int SelectedTripId { get; set; }


        private List<Entry> _timeEntries;
        private List<Entry> _distanceEntries;

        ChartView timeChart;
        ChartView distanceChart;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            journeyRepository = new JourneyRepository();
            tripRepository = new TripRepository();

            base.OnCreate(savedInstanceState);

            SelectedTripId = tripRepository.GetActiveTrip();

            SetContentView(Resource.Layout.chart_reporting_layout);
            DrawTopImageInPurple();
            GetData();

            timeChart = FindViewById<ChartView>(Resource.Id.timeChartView);
            distanceChart = FindViewById<ChartView>(Resource.Id.distanceChartView);

            distanceChart.Chart = new DonutChart() { Entries = _distanceEntries };
            timeChart.Chart = new DonutChart() { Entries = _timeEntries };

        }

        private void GetData()
        {

            _timeEntries = new List<Entry>();
            _distanceEntries = new List<Entry>();

            Dictionary<string, int> time = new Dictionary<string, int>();
            Dictionary<string, double> distance = new Dictionary<string, double>();

            int CurrentIndex = 0;


            if (journeyRepository.GetChartingDataSet(SelectedTripId, out time, out distance))
            {

                foreach (KeyValuePair<string, double> distancePairs in distance)
                {

                    Entry entry = new Entry((float)distancePairs.Value)
                    {
                        Color = SKColor.Parse(GetAColor(CurrentIndex)),
                        Label = distancePairs.Key,
                        ValueLabel = distancePairs.Value + " miles"
                    };

                    _distanceEntries.Add(entry);
                    CurrentIndex++;

                }


            }


            CurrentIndex = 0;


            foreach (KeyValuePair<string, int> timePairs in time)
            {

                TimeSpan timeLabelSpan = new TimeSpan(0, timePairs.Value,0);
                string timeLabel = timeLabelSpan.ToString(@"hh\:mm");


                Entry entry = new Entry(timePairs.Value)
                {
                    Color = SKColor.Parse(GetAColor(CurrentIndex)),
                    Label = timePairs.Key,
                    ValueLabel = timeLabel
                };

                _timeEntries.Add(entry);
                CurrentIndex++;
            
            }
        }


       

        private string GetAColor(int Index)
        {
            string colorString;

            switch (Index)
            {

                case 0:
                    colorString= "#FF1943";
                    break;
                case 1:
                    colorString = "#00BFFF";
                    break;
                case 2:
                    colorString = "#00CED1";
                    break;
                default:
                    colorString = HexConverter();
                    break;
            }

            return colorString;
            
        }

        private string HexConverter()
        {
            Android.Graphics.Color c = new Android.Graphics.Color((int)(Java.Lang.Math.Random() * 0x1000000));
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void DrawTopImageInPurple()
        {
            titleImageView = (ImageView)FindViewById(Resource.Id.finalbackgroundimage);
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
            1F, 0, 0, 0, 1F,
            0, 0, 0, 0,0,
            0, 0, 1F, 0, 1F,
            0, 0, 0, 0.8F, 0 });

            ColorMatrixColorFilter brightnessFilter = new ColorMatrixColorFilter(matrix);
            return brightnessFilter;
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
    }
}