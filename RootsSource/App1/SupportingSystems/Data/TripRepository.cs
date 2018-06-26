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
using Mono.Data.Sqlite;

namespace roots.SupportingSystems.Data
{
    public class TripRepository : BaseDataAccessingClass
    {

        public bool SetActiveTrip(int id)
        {
            try
            {
                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    string SQL = "UPDATE TRIPS SET ACTIVE = 0";
                    c.CommandText = SQL;
                    c.ExecuteNonQuery();

                    SQL = "UPDATE TRIPS SET ACTIVE = 1 WHERE ID=" + id;
                    c.CommandText = SQL;
                    c.ExecuteNonQuery();
                }

                connection.Close();
                connection.Dispose();
                connection = null;

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public int GetActiveTrip()
        {
            try
            {
                string SQLString = GetActiveTripString();

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                int Id = int.MinValue;

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var reader = c.ExecuteReader();
                    if (reader.Read())
                        Id = reader.GetInt32(0);

                }

                connection.Close();
                connection.Dispose();
                connection = null;

                return Id;

            }
            catch (Exception ex)
            {
                return Int32.MinValue;
            }
        }
        
        public List<Trip> GetAllTrips()
        {
            try
            {
                string SQLString = GetAllTripsString();

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                List<Trip> tripList = new List<Trip>();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var reader = c.ExecuteReader();

                    while (reader.Read())
                        tripList.Add(new Trip(reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetInt32(0), reader.GetInt32(4)));
                }

                connection.Close();
                connection.Dispose();
                connection = null;

                return tripList;

            }
            catch (Exception ex)
            {
                return new List<Trip>();
            }
        }

        public bool InsertNewTrip(string Name, string Description, string When)
        {
            try
            {
                string SQLString = GetInsertTripString(Name, Description, When);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var k = c.ExecuteNonQuery();
                }

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = "SELECT * FROM TRIPS;";
                    var k = c.ExecuteReader();
                }

                connection.Close();
                connection.Dispose();
                connection = null;

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetLastTripID()
        {
            try
            {
                string SQLString = GetLastTripString();

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var k = c.ExecuteReader();
                    var cccc = k[0];

                    connection.Close();
                    connection.Dispose();
                    connection = null;

                    return Convert.ToInt32(cccc);
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private string GetInsertTripString(string name, string description, string when)
        {
            string s = "INSERT INTO [TRIPS] (Name,Description,WhenFor,Active) VALUES ('" + name + "','" + description + "','" + when + "',0);";
            return s;
        }

        private string GetLastTripString()
        {
            string s = "SELECT MAX(Id) FROM TRIPS;";
            return s;
        }

        public string GetAllTripsString()
        {
            string s = "SELECT * FROM TRIPS;";
            return s;
        }

        public string GetActiveTripString()
        {
            string s = "SELECT Id FROM TRIPS WHERE ACTIVE = 1 ORDER BY Id LIMIT 1";
            return s;
        }
    }
}