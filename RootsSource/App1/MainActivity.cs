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
        private ImageView titleImageView;
        private Bitmap titleBitmap;

        private Button button_next;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            DrawTopImageInPurple();

            button_next = FindViewById<Button>(Resource.Id.titlePage_button_ok);
            button_next.Click += Button_next_Click;

        }

        private void Button_next_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(JourneyActivity));
        }

        private void DrawTopImageInPurple()
        {
            titleImageView = (ImageView)FindViewById(Resource.Id.myImageView);
            Android.Graphics.Drawables.BitmapDrawable bd = (Android.Graphics.Drawables.BitmapDrawable)titleImageView.Drawable;
            titleBitmap = bd.Bitmap;
            var vv11 = ApplyBitmapBrightness(23, titleBitmap);

            titleImageView.SetImageBitmap(vv11);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
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

