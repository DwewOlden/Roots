using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App1.SupportingSystems;
using App1.SupportingSystems.Data;

namespace App1.Forms
{
    [Activity(Label = "Add Driver")]
    public class AddNewDriver : DialogFragment
    {
        /// <summary>
        /// A button, when clicked adds the driver to the database
        /// </summary>
        Button mAddDriverButton;

        /// <summary>
        /// A text box for the driver name
        /// </summary>
        EditText mDriverName;

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Android 3.x+ still wants to show title: disable
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            // Create our view
            var view = inflater.Inflate(Resource.Layout.Add_new_driver, container, false);
            InitalizeLocalControls(view);
            InitalizeEvents();

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;

        }

        /// <summary>
        /// Group together all the event contruction
        /// </summary>
        private void InitalizeEvents()
        {
            mAddDriverButton.Click += AddDriverToDatabase;
        }

        /// <summary>
        /// Gets the instances of the controls we will be manipulating
        /// </summary>
        /// <param name="view">The view (see xml)</param>
        private void InitalizeLocalControls(View view)
        {
            // Handle dismiss button click
            mAddDriverButton = view.FindViewById<Button>(Resource.Id.add_new_drivers_button);
            mDriverName = view.FindViewById<EditText>(Resource.Id.add_new_drivers_name);
        }

        public override void OnResume()
        {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // Disable standard dialog styling/frame/theme: our custom view should create full UI
            SetStyle(DialogFragmentStyle.NoTitle, Android.Resource.Style.Theme);
            base.OnResume();
        }

        /// <summary>
        /// Adds a new driver to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDriverToDatabase(object sender, EventArgs e)
        {
            string lDriverName = mDriverName.Text;
            DriverRepository driverRepository = new DriverRepository();
            driverRepository.InsertNewDriver(lDriverName);

            // Copy the default avater to the sysrem
            AvatarManager avatarManager = new AvatarManager();
            if (!avatarManager.DestinationImageIsPresent())
                avatarManager.CopyImage();

            Dismiss();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unwire event
            if (disposing)
                mAddDriverButton.Click -= AddDriverToDatabase;
        }
    }
}