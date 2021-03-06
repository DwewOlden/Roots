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

namespace roots.SupportingSystems.DriverSystem
{
    public class CreateDriverEventArgs:EventArgs
    {
        public string DriverName { get; set; }

        public int DriverId { get; set; }

        public CreateDriverEventArgs(string pDriverName,int pDriverId)
        {
            DriverId = pDriverId;
            DriverName = pDriverName;
        }
    }

}