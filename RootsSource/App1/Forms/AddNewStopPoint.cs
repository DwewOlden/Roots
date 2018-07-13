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
using roots.SupportingSystems;
using roots.SupportingSystems.DriverSystem;

namespace roots.Forms
{
    [Activity(Label = "Add Stop Location")]
    public class AddNewStopPoint : DialogFragment
    {
        /// <summary>
        /// An event that is finred when a new contact is being created.
        /// </summary>
        public event EventHandler<CreateStopLocationEventArgs> OnGetPlaceName;

        /// <summary>
        /// A button, when clicked adds the driver to the database
        /// </summary>
        Button mAddPlaceButton;

        /// <summary>
        /// A text box for the driver name
        /// </summary>
        EditText mPlaceName;

        public int JourneyId { get; internal set; }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            var view = inflater.Inflate(Resource.Layout.Add_New_Stopping_Point, container, false);
            InitalizeLocalControls(view);
            InitalizeEvents();

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }

        public override void OnResume()
        {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // Disable standard dialog styling/frame/theme: our custom view should create full UI
            SetStyle(DialogFragmentStyle.NoTitle, Android.Resource.Style.Theme);
            base.OnResume();
        }

        private void InitalizeEvents()
        {
            mAddPlaceButton.Click += GetNewPlaceName;
        }

        /// <summary>
        /// Gets the instances of the controls we will be manipulating
        /// </summary>
        /// <param name="view">The view (see xml)</param>
        private void InitalizeLocalControls(View view)
        {
            // Handle dismiss button click
            mAddPlaceButton = view.FindViewById<Button>(Resource.Id.add_stopplace_button);
            mPlaceName = view.FindViewById<EditText>(Resource.Id.add_stopplace_text);
        }

        private void GetNewPlaceName(object sender, EventArgs e)
        {
            string lDriverName = mPlaceName.Text;

            if (OnGetPlaceName != null)
                OnGetPlaceName.Invoke(this, new CreateStopLocationEventArgs(mPlaceName.Text, JourneyId));

            Dismiss();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unwire event
            if (disposing)
                mAddPlaceButton.Click -= GetNewPlaceName;
        }
    }
}