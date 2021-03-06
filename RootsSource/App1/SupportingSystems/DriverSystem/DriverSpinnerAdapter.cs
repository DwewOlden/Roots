﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace roots.SupportingSystems.DriverSystem
{
    public class DriverSpinnerAdapter : BaseAdapter<Driver>
    {
        private Context mContext;
        private int mLayout;
        private List<Driver> mContacts;
        private Action<Driver> mPicSelected;

        public DriverSpinnerAdapter(Context context, int layout, List<Driver> contacts,Action<Driver> driverSelected)
        {
            mContext = context;
            mLayout = layout;
            mContacts = contacts;
            mPicSelected = driverSelected;
        }

       
        public override Driver this[int position]
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

            row.FindViewById<TextView>(Resource.Id.txtJourneyScreenDriverName).Text = mContacts[position].Name;

            return row;
        }
        
    }
}