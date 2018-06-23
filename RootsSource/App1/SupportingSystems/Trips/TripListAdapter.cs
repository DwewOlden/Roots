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

namespace roots.SupportingSystems.Trips
{
    public class TripListAdapter : BaseAdapter<Trip>
    {
        private Context mContext;
        private int mLayout;
        private List<Trip> mContacts;
        private Action<TextView> TripSelected;

        public TripListAdapter(Context context, int layout, List<Trip> contacts, Action<TextView> pTripSelected)
        {
            mContext = context;
            mLayout = layout;
            mContacts = contacts;
            this.TripSelected = pTripSelected;
        }

        public override Trip this[int position]
        {
            get { return mContacts[position]; }
        }

        public override int Count
        {
            get { return mContacts.Count; }
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

            string isActive = string.Empty;
            if (mContacts[position].Active)
                isActive = ", Active";

            row.FindViewById<TextView>(Resource.Id.txtTripScreenTripName).Text = mContacts[position].Name;
            row.FindViewById<TextView>(Resource.Id.txtTripScreenTripWhen).Text = mContacts[position].When + isActive;

            return row;
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Hello");
        }
    }
}