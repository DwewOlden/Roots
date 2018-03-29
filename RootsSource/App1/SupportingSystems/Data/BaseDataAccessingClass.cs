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
using Mono.Data.Sqlite;

namespace roots.SupportingSystems.Data
{
    /// <summary>
    /// Contains the main database conneciton functions.
    /// </summary>
    public class BaseDataAccessingClass
    {
        /// <summary>
        /// A conneciton to the database
        /// </summary>
        internal SqliteConnection connection;

        /// <summary>
        /// A constant giving the path to the database
        /// </summary>
        internal const string DB_FILENAME = "TripsDatabase.db3";

        /// <summary>
        /// A method that will return the path to the database
        /// </summary>
        internal string GetPathToDatabase()
        {
            // determine the path for the database file
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DB_FILENAME);
            return dbPath;
        }
    }
}