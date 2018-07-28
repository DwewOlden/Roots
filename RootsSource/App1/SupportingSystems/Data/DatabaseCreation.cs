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
    /// This class will add the database if it is not present, otherwise it will quietly ignore things
    /// </summary>
    public class DatabaseCreation:BaseDataAccessingClass
    {
        /// <summary>
        /// This is used in debug only, if set to true it will empty the database
        /// </summary>
        private const bool APPLY_KILL = false;

        /// <summary>
        /// A boolean flag indicating if the database already existed.
        /// </summary>
        private bool AlreadyExiststed = true;
        
        /// <summary>
        /// Checks if the datbase already exists, returns true if it does, and false if it 
        /// does not
        /// </summary>
        /// <returns>True if the database exists, false if it does not</returns>
        private bool DatabaseAlreadyExists()
        {
            string path = GetPathToDatabase();
            if (System.IO.File.Exists(path))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Creates an empty database
        /// </summary>
        /// <returns>True if the database could be created, false if it could not</returns>
        private bool CreateDatabase()
        {
            if (File.Exists(GetPathToDatabase()))
            {
                AlreadyExiststed = true;
                return true;
            }
            else
            {
                AlreadyExiststed = false;
                Mono.Data.Sqlite.SqliteConnection.CreateFile(GetPathToDatabase());
                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
            }

            if (File.Exists(GetPathToDatabase()))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Creates the database structure
        /// </summary>
        /// <returns>True if the structure could be created, false if it could not</returns>
        private bool CreateTableStructureInDatabase()
        {
            string[] commands = GetCommands();

            // Open the database connection and create table with data
            connection.Open();
            foreach (var command in commands)
            {
                using (var c = connection.CreateCommand())
                {
                    c.CommandText = command;
                    var rowcount = c.ExecuteNonQuery();
                }
            }
            
                return true;
        }

        /// <summary>
        /// Gets the SQL commands that are used to generate the database
        /// </summary>
        /// <returns></returns>
        private string[] GetCommands()
        {
            var commands = new[] {
                "CREATE TABLE [TRIPS] (Id INTEGER PRIMARY KEY,Name nText, Description nText, WhenFor nText,Active INTEGER,StartPoint nText);",
                "CREATE TABLE [JOURNEY] (Id INTEGER PRIMARY KEY,Driver INTEGER,Trip INTEGER,JourneyStarted datetime,JourneyEnded datetime,JourneyDistance real,EndPoint nText,Notes nText,Amount nText,CostPerUnit nText,TotalCost nText);",
                "CREATE TABLE [DRIVERS] (Id INTEGER PRIMARY KEY, Name nText,ImageName nText,Registered nText,Active INTEGER);",
                "CREATE TABLE [JOURNEYPOINTS] (PointIndex INTEGER,JourneyId INTEGER, Lat real,Long real,PRIMARY KEY (PointIndex,JourneyId));"
            };

            return commands;
        }

        /// <summary>
        /// Kills the database as part of the debug operations
        /// </summary>
        /// <returns></returns>
        protected bool KillDatabase()
        {
            if (File.Exists(GetPathToDatabase()))
                File.Delete(GetPathToDatabase());

            return true;
        }

        /// <summary>
        /// Sets up the database
        /// </summary>
        /// <returns>True if the database could be initalised, false if it could not</returns>
        public bool InitaliseDatabase()
        {
            if (APPLY_KILL)
                KillDatabase();

            if (!CreateDatabase())
                return false;

            if (!AlreadyExiststed)
                if (!CreateTableStructureInDatabase())
                return false;

            return true;
        }
    }
}