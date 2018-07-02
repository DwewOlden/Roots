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
    public class JourneyRepository : BaseDataAccessingClass
    {
        public bool UpdateJourneyDetails(int Id, double distance)
        {
            try
            {
                string SQLString = GetDistanceAndLastTimeString(Id,distance);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var k = c.ExecuteNonQuery();
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

        public int StartNewJourney(int Driver,int Trip)
        {
            DateTime dateTime = DateTime.Now;
            int Id = -1;

            try
            {
                string SQLString = GetInsertJourneyString(Driver,Trip);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var k = c.ExecuteNonQuery();
                }

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = GetLastTripString();
                    var k = c.ExecuteReader();
                    k.Read();
                    Id = k.GetInt32(0);

                }

                connection.Close();
                connection.Dispose();
                connection = null;

                return Id;

            }
            catch (Exception ex)
            {
                return Id;
            }
        }

        private string GetInsertJourneyString(int Driver,int Trip)
        {
            DateTime Now = DateTime.Now;
            var v = Roots.Support.SQLLiteDateTimes.DateTimeSQLite(Now);

            string s = string.Format("INSERT INTO [JOURNEY] (Driver,Trip,JourneyStarted,JourneyEnded,JourneyDistance,EndPoint) VALUES ({0},{1},'{2}','{3}',{4},{5})"
                ,Driver,Trip, v,v,0,"'Not Set'");

            return s;
        }

        private string GetDistanceAndLastTimeString (int Id,double distance)
        {
            DateTime Now = DateTime.Now;
            var v = Roots.Support.SQLLiteDateTimes.DateTimeSQLite(Now);
            
            string s = string.Format("UPDATE [JOURNEY] SET JourneyEnded = '{0}', JourneyDistance={1} WHERE Id= {2}", v, distance,Id);
            return s;
        }




        private string GetLastTripString()
        {
            string s = "SELECT MAX(Id) FROM JOURNEY;";
            return s;
        }
    }
}