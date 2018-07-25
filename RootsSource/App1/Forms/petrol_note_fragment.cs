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
using roots.SupportingSystems;

namespace roots.Forms
{
    public class petrol_note_fragment:DialogFragment
    {
        public event EventHandler<PetrolNotesEventArgs> NoteAdded;
        
        Button AddNoteButton;

        EditText UnitText;
        EditText CostPerUnitText;
        EditText TotalCostText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            var view = inflater.Inflate(Resource.Layout.petrol_note_layout, container, false);
            InitalizeLocalControls(view);
            InitalizeEvents();

            return view;
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
        /// Gets the instances of the controls we will be manipulating
        /// </summary>
        /// <param name="view">The view (see xml)</param>
        private void InitalizeLocalControls(View view)
        {
            // Handle dismiss button click
            AddNoteButton = view.FindViewById<Button>(Resource.Id.add_new_petrol_note_button);
            UnitText = view.FindViewById<EditText>(Resource.Id.petrol_unit_purchased);
            CostPerUnitText = view.FindViewById<EditText>(Resource.Id.petrol_unit_cost);
            TotalCostText = view.FindViewById<EditText>(Resource.Id.petrol_unit_totalcost);

        }

        private void InitalizeEvents()
        {
            AddNoteButton.Click += AddNoteToDatabase;
        }

        private void AddNoteToDatabase(object sender, EventArgs e)
        {
            string totalCost = TotalCostText.Text.ToUpper().Trim();
            string costPerUnit = CostPerUnitText.Text.ToUpper().Trim();
            string unitsText = UnitText.Text.ToUpper().Trim();
            
            if ((totalCost.Length > 0) || (costPerUnit.Length > 0) || unitsText.Length > 0)
                if (NoteAdded != null)
                    NoteAdded.Invoke(this, new PetrolNotesEventArgs(unitsText,costPerUnit,totalCost));

            Dismiss();
        }
    }
}