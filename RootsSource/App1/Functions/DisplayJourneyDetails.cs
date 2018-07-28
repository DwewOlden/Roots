using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using roots.SupportingSystems.Data;

namespace roots.Functions
{
    [Activity(Label = "Selected Journey Details")]
    public class DisplayJourneyDetails : Activity, IOnMapReadyCallback
    {
        private GoogleMap _map;
        private MapFragment _mapFragment;


        private int SelectedJourneyId = int.MinValue; 

        private JourneyRepository journeyRepository;
        private JourneyPointRespository journeyPointRespository;

        TextView textDriverName;
        TextView textDuration;
        TextView textEndPoint;
        TextView textStartTime;
        TextView textDistance;

        Button drawTracksButton;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            journeyRepository = new JourneyRepository();
            journeyPointRespository = new JourneyPointRespository();
            base.OnCreate(savedInstanceState);
            SelectedJourneyId = Intent.Extras.GetInt("journeyId");

            SetContentView(Resource.Layout.DisplayJourneyDetails);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            InitMapFragment();
            GetTextValues();
            DrawJourneyDetails();

            var editToolbar = FindViewById<Toolbar>(Resource.Id.notemenutoolbar);
            editToolbar.InflateMenu(Resource.Menu.note_menu);
            editToolbar.MenuItemClick += (sender, e) =>
            {

                string ContextMenuSelected = e.Item.TitleFormatted.ToString();
                FragmentTransaction transaction = FragmentManager.BeginTransaction();

                switch (ContextMenuSelected)
                {
                    case "Add Note":
                        var dialog = new Forms.generic_note_fragment();
                        dialog.NoteAdded += Dialog_NoteAdded; 
                        dialog.Show(transaction, "dialog");
                        break;

                    case "Add Petrol Details":
                        var petroldialog = new Forms.petrol_note_fragment();
                        petroldialog.NoteAdded += Petroldialog_NoteAdded;
                        petroldialog.Show(transaction, "dialog");
                        break;
                }
            };

        }

        private void Petroldialog_NoteAdded(object sender, SupportingSystems.PetrolNotesEventArgs e)
        {
            bool Outcome = journeyRepository.UpdateWithNote(SelectedJourneyId, e.TotalCost, e.CostPerUnit, e.Units);
            DisplayToast(Outcome);
        }

        private void Dialog_NoteAdded(object sender, SupportingSystems.GenericNoteEventArgs e)
        {
            bool Outcome = journeyRepository.UpdateWithNote(SelectedJourneyId, e.Notes);
            DisplayToast(Outcome);
        }

        private void DisplayToast(bool outcome)
        {
            if (outcome)
                Toast.MakeText(this, "Note Was Added", ToastLength.Long).Show();
            else
                Toast.MakeText(this, "Acchh, wee gremlins have been at us!", ToastLength.Long).Show();
        }

        private void GetTextValues()
        {
            textDistance = FindViewById<TextView>(Resource.Id.journey_details_distance_value);
            textDriverName = FindViewById<TextView>(Resource.Id.journey_details_driver_value);
            textDuration = FindViewById<TextView>(Resource.Id.journey_details_duration_value);
            textEndPoint = FindViewById<TextView>(Resource.Id.journey_details_endpoint_value);
            textStartTime = FindViewById<TextView>(Resource.Id.journey_details_starttime_value);

            drawTracksButton = FindViewById<Button>(Resource.Id.trackDetailsButton);
            drawTracksButton.Click += DrawTracksButton_Click;
        }

        private void DrawTracksButton_Click(object sender, EventArgs e)
        {
            var points = journeyPointRespository.GetTrackPointsForJourney(SelectedJourneyId);
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            PolylineOptions line = new PolylineOptions();
            

            if (points.Count() > 0)
            {
                foreach (var v in points)
                {
                    LatLng l = new LatLng(v.Lat, v.Lon);
                    builder.Include(l);
                    line.Add(l);

                }

                _map.AddPolyline(line);
                _map.MoveCamera(CameraUpdateFactory.NewLatLngBounds(builder.Build(), 50));

            }
        }

        private void DrawJourneyDetails()
        {
            var dto = journeyRepository.GetSpecificJourneyStats(SelectedJourneyId);

            textStartTime.Text = dto.Starting.ToString("HH:mm");
            textDriverName.Text = dto.DriverName;
            textEndPoint.Text = dto.EndPoint;
            textDistance.Text = string.Format("{0} Miles", Math.Round((dto.Distance / 1.61),1));
            textDuration.Text = string.Format("{0} Hours {1} Minutes", dto.Duration.Hours, dto.Duration.Minutes);
            
        }

        private void InitMapFragment()
        {
            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeSatellite)
                    .InvokeZoomControlsEnabled(true)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
            }
            _mapFragment.GetMapAsync(this);

        }

        public void OnMapReady(GoogleMap map)
        {
            _map = map;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenuItems, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            PerformSelectedMenuFunction(item);

            return base.OnOptionsItemSelected(item);
        }

        private void PerformSelectedMenuFunction(IMenuItem item)
        {
            string titleFormatted = item.TitleFormatted.ToString();
            switch (titleFormatted)
            {
                case "Edit":
                    Intent intent = new Intent(this.ApplicationContext, typeof(JourneyActivity));
                    intent.SetFlags(ActivityFlags.SingleTop);
                    StartActivity(intent);
                    break;

                case "Save":
                    StartActivity(typeof(reporting_section));
                    break;


                case "Drivers":
                    StartActivity(typeof(TabViews.DriversMenu));
                    break;

                case "Trips":
                    StartActivity(typeof(TabViews.TripList));
                    break;

                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
               ToastLength.Short).Show();
                    break;
            }
        }
    }
}