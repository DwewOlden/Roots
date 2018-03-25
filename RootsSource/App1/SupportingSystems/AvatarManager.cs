using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// A class that will copy the avater from one directory (the assetts) of the 
    /// project to the shared image of the project
    /// </summary>
    public class AvatarManager
    {
        /// <summary>
        /// The name of the file that we are going to be using as a default avater for all 
        /// new drivers
        /// </summary>
        private const string DefaultAvaterName = "Icon.png";

        /// <summary>
        /// The name the image will be saved as.
        /// </summary>
        public const string DefaultSaveImageName = "Image.png";

        /// <summary>
        /// Gets the path where we copy the image from
        /// </summary>
        /// <returns></returns>
        public string GetPathToDestinationImage()
        {
            // determine the path for the database file
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DefaultSaveImageName);
            return dbPath;
        }

        /// <summary>
        /// Gets the path where we copy the image to
        /// </summary>
        /// <returns></returns>
        public string GetPathToSourceImage()
        {
            return DefaultAvaterName;
        }

        /// <summary>
        /// Determines if the destination is already present
        /// </summary>
        /// <returns>True of the image is there, false is it is not</returns>
        public bool DestinationImageIsPresent()
        {
            string path = GetPathToDestinationImage();
            if (System.IO.File.Exists(path))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the image from the assetts direcotory to the shared directory of the applicatiom
        /// </summary>
        /// <returns></returns>
        public bool CopyImage()
        {
            try
            {
                using (var br = new BinaryReader(Application.Context.Assets.Open(GetPathToSourceImage())))
                {
                    using (var bw = new BinaryWriter(new FileStream(GetPathToDestinationImage(), FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            bw.Write(buffer, 0, length);
                    }
                }
            } catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}




