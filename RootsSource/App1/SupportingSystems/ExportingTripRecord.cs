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
    public class ExportingTripRecord
    {
        public const string Delimiter = "\t";

        /// <summary>
        /// Index 3
        /// </summary>
        public DateTime JourneyStarted { get; set; }

        /// <summary>
        /// Index 4
        /// </summary>
        public DateTime JourneyEnded { get; set; }

        /// <summary>
        /// Index 5
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Index 6
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Index 7
        /// </summary>
        public string Notes { get; set; }


        /// <summary>
        /// Index 8
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Index 9
        /// </summary>
        public string CostPerUnit { get; set; }


        /// <summary>
        /// Index 10
        /// </summary>
        public string TotalCost { get; set; }

        /// <summary>
        /// Index 12
        /// </summary>
        public string DriverName { get; set; }


        public string StartPoint { get; set; }

        public string GetAsString()
        {
            string line = string.Empty;
            line = line +JourneyStarted.ToShortDateString() + Delimiter;
            line = line +JourneyStarted.ToString("HH:mm") + Delimiter;
            line = line +StartPoint + Delimiter;
            line = line +DriverName + Delimiter;
            line = line +JourneyEnded.ToString("HH:mm") + Delimiter;
            line = line +Math.Round((Distance/1.61),1) + Delimiter;
            line = line +JourneyEnded.Subtract(JourneyStarted).ToString(@"hh\:mm") + Delimiter;
            line = line + EndPoint + Delimiter;
            line = line +Notes + Delimiter;
            line = line +Amount + Delimiter;
            line = line +CostPerUnit + Delimiter;
            line = line + TotalCost;
            
            return line;
            
        }

        public static string GetHeader()
        {
            string line = string.Empty;
            line = line + "Date" + Delimiter;
            line = line + "Started" + Delimiter;
            line = line + "From" + Delimiter;
            line = line + "Driving" + Delimiter;
            line = line + "Ended" + Delimiter;
            line = line + "Distance" + Delimiter;
            line = line + "Duration" + Delimiter;
            line = line + "Stopped At" + Delimiter;
            line = line + "Notes" + Delimiter;
            line = line + "Amount" + Delimiter;
            line = line + "Cost" + Delimiter;
            line = line + "Total";

            return line;
        }

    }
}