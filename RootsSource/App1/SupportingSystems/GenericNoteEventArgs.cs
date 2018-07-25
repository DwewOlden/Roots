
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
    public class GenericNoteEventArgs:EventArgs
    {
        public string Notes { get; set; }

        public GenericNoteEventArgs()
        {

        }

        public GenericNoteEventArgs(string note)
        {
            Notes = note;
        }
    }
}