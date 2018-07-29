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
using roots.SupportingSystems.Journey;

namespace roots.SupportingSystems.Data
{
    public class JourneyPointRespository : BaseDataAccessingClass
    {
        public bool DeleteJourneyPoints(int JourneyId)
        {
            try
            {
                string SQL = string.Format("DELETE FROM JOURNEYPOINTS WHERE JourneyId={0};", JourneyId);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQL;
                    var reader = c.ExecuteNonQuery();
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<TrackPointDTO> GetTrackPointsForJourney(int JourneyId)
        {
            List<TrackPointDTO> trackPointDTOs = new List<TrackPointDTO>();

            try
            {
                string SQL = string.Format("SELECT * FROM JOURNEYPOINTS WHERE JourneyId={0} ORDER By PointIndex;", JourneyId);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQL;
                    var reader = c.ExecuteReader();

                    while (reader.Read())
                        trackPointDTOs.Add(new TrackPointDTO() { Lat = reader.GetDouble(2), Lon = reader.GetDouble(3) });
                }

            }
            catch (Exception ex)
            {
                return new List<TrackPointDTO>().AsEnumerable();
            }

            return trackPointDTOs;

        }

        public bool InsertPointIntoTrack(int Journey, int Stop, double Lat, double Lon)
        {
            bool outcome = false;

            try
            {
                string SQLString = GetInsertJourneyPointString(Journey, Stop, Lat, Lon);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var k = c.ExecuteNonQuery();
                    if (k > 0)
                        outcome = true;
                }



                connection.Close();
                connection.Dispose();
                connection = null;

                return outcome;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string GetInsertJourneyPointString(int journey, int stop, double lat, double lon)
        {
            string s = string.Format("INSERT INTO [JOURNEYPOINTS] (PointIndex,JourneyId,Lat,Long) VALUES ({0},{1},{2},{3})"
                , stop, journey, lat, lon);

            return s;

        }
    }
}