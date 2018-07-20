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

namespace roots.SupportingSystems
{
    public class SelectedTimeEventArgs:EventArgs
    {
        public DateTime SelectedTime { get; set; }

        public SelectedTimeEventArgs()
        {
            SelectedTime = DateTime.Now;
        }

        public SelectedTimeEventArgs(DateTime d)
        {
            SelectedTime = d;
        }

        public override string ToString()
        {
            return SelectedTime.ToString();
        }
    }
}