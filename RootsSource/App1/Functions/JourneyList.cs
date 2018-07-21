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
using roots.SupportingSystems.Data;
using roots.SupportingSystems.Journey;

namespace roots.Functions
{
    [Activity(Label = "JourneyList")]
    public class JourneyList : Activity
    {
        private ListView mListView;
        private List<Journey> mJourneys;
        private BaseAdapter<Journey> mAdapter;
        private JourneyRepository journeyRepository;

        Action<TextView> action;
        
        private int SelectedTripId { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            journeyRepository = new JourneyRepository();
            base.OnCreate(savedInstanceState);
            SelectedTripId = Intent.Extras.GetInt("tripId");

            SetContentView(Resource.Layout.MainJourneyListScreen);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            mListView = FindViewById<ListView>(Resource.Id.JourneyListView);
            mListView.ItemClick += MListView_ItemClick;
            mJourneys = new List<Journey>();

            action = JourneySelected;

            var editToolbar = FindViewById<Toolbar>(Resource.Id.journeyMenu);
            editToolbar.InflateMenu(Resource.Menu.trip_menu);
            editToolbar.MenuItemClick += (sender, e) =>
            {

                string ContextMenuSelected = e.Item.TitleFormatted.ToString();

                switch (ContextMenuSelected)
                {
                    case "Insert A Journey":
                        StartActivity(typeof(AddNewTrip));
                        break;
                }
            };


        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
                PopulateJourneyList();
        }

        private void PopulateJourneyList()
        {
            mJourneys = journeyRepository.GetAllTripJourneys(SelectedTripId);
            
            mAdapter = new JourneyListAdapter(this, Resource.Layout.JourneyListVewRowLayout, mJourneys, action);
            mListView.Adapter = mAdapter;
        }

        private void JourneySelected(TextView textView)
        {
            System.Diagnostics.Debug.WriteLine(textView.Text);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;
            var journey = mJourneys[pos];
            var journeyId = journey.JourneyId;

            var intent = new Intent(this, typeof(DisplayJourneyDetails));
            intent.PutExtra("journeyId", journeyId);
            StartActivity(intent);
        }
    }
}