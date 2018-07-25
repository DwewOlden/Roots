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

namespace roots.Forms
{
    public class set_start_point_fragment : DialogFragment
    {
        public event EventHandler<GenericNoteEventArgs> NoteAdded;

        /// <summary>
        /// A button, when clicked adds the driver to the database
        /// </summary>
        Button AddNoteButton;

        /// <summary>
        /// A text box for the driver name
        /// </summary>
        EditText NoteText;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            var view = inflater.Inflate(Resource.Layout.journey_startpoint_layout, container, false);
            InitalizeLocalControls(view);
            InitalizeEvents();

            return view;
        }

        /// <summary>
        /// Group together all the event contruction
        /// </summary>
        private void InitalizeEvents()
        {
            AddNoteButton.Click += AddNoteToDatabase;
        }

        private void AddNoteToDatabase(object sender, EventArgs e)
        {
            string noteDetails = NoteText.Text;
            noteDetails = noteDetails.ToUpper().Trim();

            if (noteDetails.Length > 0)
                if (NoteAdded != null)
                    NoteAdded.Invoke(this, new GenericNoteEventArgs(noteDetails));

            Dismiss();

        }

        /// <summary>
        /// Gets the instances of the controls we will be manipulating
        /// </summary>
        /// <param name="view">The view (see xml)</param>
        private void InitalizeLocalControls(View view)
        {
            // Handle dismiss button click
            AddNoteButton = view.FindViewById<Button>(Resource.Id.setstartpoint_button);
            NoteText = view.FindViewById<EditText>(Resource.Id.startpoint_details);
        }

        public override void OnResume()
        {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // Disable standard dialog styling/frame/theme: our custom view should create full UI
            SetStyle(DialogFragmentStyle.NoTitle, Android.Resource.Style.Theme);
            base.OnResume();
        }
    }
}