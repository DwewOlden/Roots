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
    public class PetrolNotesEventArgs
    {
        public string Units { get; set; }

        public string CostPerUnit { get; set; }

        public string TotalCost { get; set; }

        public PetrolNotesEventArgs()
        {

        }

        public PetrolNotesEventArgs(string pUnits,string pCostPerUnit,string pTotalCost)
        {
            Units = pUnits;
            TotalCost = pTotalCost;
            CostPerUnit = pCostPerUnit;
        }
    }
}