﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using roots.SupportingSystems;
using roots.SupportingSystems.Data;
using roots.SupportingSystems.DriverSystem;

namespace roots.Forms
{
    [Activity(Label = "Add Driver")]
    public class AddNewDriver : DialogFragment
    {
        /// <summary>
        /// An event that is finred when a new contact is being created.
        /// </summary>
        public event EventHandler<CreateDriverEventArgs> OnCreateDriver;

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
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

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
            int DriverId = driverRepository.GetLastDriverID();

            if (DriverId != -1)
            {

                // Copy the default avater to the sysrem
                AvatarManager avatarManager = new AvatarManager();
                if (!avatarManager.DestinationImageIsPresent())
                    avatarManager.CopyImage();

                if (OnCreateDriver != null)
                    OnCreateDriver.Invoke(this, new CreateDriverEventArgs(mDriverName.Text,DriverId));
            }

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