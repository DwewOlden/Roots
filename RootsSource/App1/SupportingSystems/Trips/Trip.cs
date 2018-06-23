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
    public class Trip
    {
        /// <summary>
        /// The id (from the database) of the driver
        /// </summary>
        public int TripId { get; set; }

        /// <summary>
        /// The name of the driver
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the trip
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// When was the trip
        /// </summary>
        public string When { get; set; }

        /// <summary>
        /// Is the trip active
        /// </summary>
        public bool Active { get; set; }

        public Trip()
        {

        }

        public Trip(string pName, string pDescription)
        {
            this.Name = pName;
            this.Description = pDescription;
        }

        public Trip(string pName, string pDescription, string pWhen, int pDriverId,int pActive)
        {
            this.Name = pName;
            this.Description = pDescription; 
            this.When = pWhen;

            if (pActive == 0)
                Active = false;
            else
                Active = true;
        }
    }
}