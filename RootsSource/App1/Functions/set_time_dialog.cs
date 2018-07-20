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

namespace roots.Functions
{
    public class set_time_dialog:DialogFragment
    {

        /// <summary>
        /// An event that is finred when a new contact is being created.
        /// </summary>
        public event EventHandler<SelectedTimeEventArgs> OnTimeSet;

        private Button selected_button;
        private TimePicker picker;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
      
            var view = inflater.Inflate(Resource.Layout.set_time_dialog_fragment,container,false);
            selected_button = view.FindViewById<Button>(Resource.Id.button_set_time);
            selected_button.Click += Selected_button_Click;

            picker = view.FindViewById<TimePicker>(Resource.Id.the_time_picker);
           
            return view;

        }

        private void Selected_button_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var hour = picker.Hour;
            var minute = picker.Minute;

            DateTime selected = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
            SelectedTimeEventArgs selectedTimeEventArgs = new SelectedTimeEventArgs(selected);

            if (OnTimeSet != null)
                OnTimeSet.Invoke(this, selectedTimeEventArgs);

            Dismiss();

        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}