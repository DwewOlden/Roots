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

namespace roots.SupportingSystems.Journey
{
    public class JourneyListAdapter:BaseAdapter<Journey>
    {
        private Context mContext;
        private int mLayout;
        private List<Journey> mJourneyList;
        private Action<TextView> JourneySelected;

        public JourneyListAdapter(Context context, int layout, List<Journey> contacts, Action<TextView> pJoourneySelected)
        {
            mContext = context;
            mLayout = layout;
            mJourneyList = contacts;
            this.JourneySelected = pJoourneySelected;
        }

        public override Journey this[int position]
        {
            get { return mJourneyList[position]; }
        }

        public override int Count
        {
            get { return mJourneyList.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
                row = LayoutInflater.From(mContext).Inflate(mLayout, parent, false);
            string distanceString = Convert.ToString(Math.Round(mJourneyList[position].Distance / 1.61, 2));
            string rowDetails = string.Format("{0} {1} drove {2} miles to {3}", mJourneyList[position].Starting.ToShortDateString(),
                mJourneyList[position].DriverName, distanceString, mJourneyList[position].EndPoint);

            row.FindViewById<TextView>(Resource.Id.txtJourneyTimes).Text = mJourneyList[position].JourneyTimes;
            row.FindViewById<TextView>(Resource.Id.txtJourneyDriver).Text = rowDetails;
            
            return row;
        }
    }
}