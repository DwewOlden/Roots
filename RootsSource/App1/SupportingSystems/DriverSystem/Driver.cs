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

namespace roots.SupportingSystems.DriverSystem
{
    /// <summary>
    /// Stores details about the drivers, including the binary details of the avater
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// The name of the driver
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets if the driver is currently active
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// Gets the image data for the driver
        /// </summary>
        public byte[] ImageData { get; set; }

        public Driver()
        {

        }

        public Driver(string pName,bool pActive,string pImagePath)
        {
            this.Name = pName;
            this.Active = pActive;
            
        }


    }
}