using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class JourneyRepository : BaseDataAccessingClass
    {
        string format = "yyyy-MM-dd HH:mm:ss";

        public JourneyDetailsDTO GetSpecificJourneyStats(int Id)
        {

            JourneyDetailsDTO dto = new JourneyDetailsDTO();

            try
            {
                string SQLString = GetSpecificJourney(Id);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var reader = c.ExecuteReader();

                    while (reader.Read())
                    {
                        Journey.JourneyDetailsDTO j = new Journey.JourneyDetailsDTO()
                        {
                            Id = reader.GetInt32(0),
                           
                            Starting = DateTime.ParseExact(reader.GetString(3), "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None),
                            Duration = DateTime.ParseExact(reader.GetString(4), "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None)
                                    .Subtract(DateTime.ParseExact(reader.GetString(3), "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                            Distance = reader.GetDouble(5),
                            EndPoint = reader.GetString(6),
                            DriverName = reader.GetString(8)
                        };



                    }

                    reader.Close();
                }

                connection.Close();
                connection.Dispose();
                connection = null;

                return dto;

            }
            catch (Exception ex)
            {
                return new JourneyDetailsDTO();
            }
        }


        public bool GetAllDriverStats(int Id, out Dictionary<string, int> Time, out Dictionary<string, double> Distance)
        {
            Time = new Dictionary<string, int>();
            Distance = new Dictionary<string, double>();

            try
            {
                string SQLString = GetCompleteRecordsFromTrip(Id);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var reader = c.ExecuteReader();

                    while (reader.Read())
                    {
                        Journey.Journey j = new Journey.Journey()
                        {
                            Starting = DateTime.ParseExact(reader.GetString(3), "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None),
                            Ending = DateTime.ParseExact(reader.GetString(4), "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None),
                            DriverName = reader.GetString(8)
                        };



                    }

                    reader.Close();
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


        public List<Journey.Journey> GetAllTripJourneys(int Id)
        {
            List<Journey.Journey> journeysList = new List<Journey.Journey>();

            try
            {
                string SQLString = GetCompleteRecordsFromTrip(Id);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var reader = c.ExecuteReader();

                    while (reader.Read())
                    {
                        Journey.Journey j = new Journey.Journey()
                        {
                            JourneyId = reader.GetInt32(0),
                            DriverId = reader.GetInt32(1),
                            TripId = reader.GetInt32(2),
                            Starting = DateTime.ParseExact(reader.GetString(3), "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None),
                            Ending = DateTime.ParseExact(reader.GetString(4), "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None),

                            Distance = reader.GetDouble(5),
                            EndPoint = reader.GetString(6),
                            DriverName = reader.GetString(8)
                        };

                        journeysList.Add(j);

                    }

                    reader.Close();
                }

                connection.Close();
                connection.Dispose();
                connection = null;

                return journeysList;

            }
            catch (Exception ex)
            {
                return new List<Journey.Journey>();
            }
        }


        public bool GetTripTotals(int Id, out TimeSpan time, out double distance, out int journeys)
        {
            time = new TimeSpan();
            distance = 0.0;
            journeys = 0;

            try
            {
                string SQLString = GetRecordsFromTrip(Id);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var reader = c.ExecuteReader();

                    while (reader.Read())
                    {

                        string journeyStartsString = reader.GetString(3);
                        DateTime journeyStarts = DateTime.ParseExact(journeyStartsString, "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        string journeyEndsString = reader.GetString(4);
                        DateTime journeyEnds = DateTime.ParseExact(journeyEndsString, "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        TimeSpan duration = journeyEnds.Subtract(journeyStarts);
                        time = time.Add(duration);

                        distance = distance + reader.GetDouble(5);
                        journeys++;
                    }

                    reader.Close();
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

        public bool UpdateWithEndPoint(int Id, string PlaceName)
        {
            try
            {
                string sql = GetEndPointSQLString(Id, PlaceName);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = sql;
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

        public bool CountMetricToday(int Id, out double distance, out TimeSpan time)
        {
            time = new TimeSpan();
            distance = 0.0;

            try
            {
                string SQLString = GetRecordsFromTodayString(Id);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var reader = c.ExecuteReader();

                    while (reader.Read())
                    {
                        string journeyStartsString = reader.GetString(3);
                        DateTime journeyStarts = DateTime.ParseExact(journeyStartsString, "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        string journeyEndsString = reader.GetString(4);
                        DateTime journeyEnds = DateTime.ParseExact(journeyEndsString, "yyyy-M-d H:m:s.FFF", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        TimeSpan duration = journeyEnds.Subtract(journeyStarts);
                        time = time.Add(duration);

                        distance = distance + reader.GetDouble(5);
                    }

                    reader.Close();
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

        public bool UpdateJourneyDetails(int Id, double distance)
        {
            try
            {
                string SQLString = GetDistanceAndLastTimeString(Id, distance);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = "SELECT COUNT(*) FROM JOURNEY WHERE JourneyStarted =Date('now');";
                    var k = c.ExecuteReader();
                    k.Read();
                    System.Diagnostics.Debug.WriteLine(k.GetInt32(0));
                }

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

        public int StartNewJourney(int Driver, int Trip)
        {
            DateTime dateTime = DateTime.Now;
            int Id = -1;

            try
            {
                string SQLString = GetInsertJourneyString(Driver, Trip);

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

        public int ManuallyInsertNewJourney(int Driver, int Trip, DateTime StartingTime, DateTime EndingTime, double Distance, string EndPoint)
        {
            DateTime dateTime = DateTime.Now;
            int Id = -1;

            try
            {
                string SQLString = GetInsertManualJourneyString(Driver, Trip, StartingTime, EndingTime, Distance, EndPoint);

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


        private string GetSpecificJourney(int Id)
        {
            string s = "SELECT J.*,D.Id,D.Name FROM JOURNEY J INNER JOIN DRIVERS D ON J.Driver = D.Id  WHERE J.Id = " + Id;
            return s;
        }

        private string GetCompleteRecordsFromTrip(int Id)
        {
            string s = "SELECT J.*,D.Id,D.Name FROM JOURNEY J INNER JOIN DRIVERS D ON J.Driver = D.Id  WHERE Trip = " + Id;
            return s;
        }

        private string GetInsertJourneyString(int Driver, int Trip)
        {
            DateTime Now = DateTime.Now;
            var v = Roots.Support.SQLLiteDateTimes.DateTimeSQLite(Now);

            string s = string.Format("INSERT INTO [JOURNEY] (Driver,Trip,JourneyStarted,JourneyEnded,JourneyDistance,EndPoint) VALUES ({0},{1},'{2}','{3}',{4},{5})"
                , Driver, Trip, v, v, 0, "'Not Set'");

            return s;
        }

        private string GetInsertManualJourneyString(int Driver, int Trip, DateTime StartingTime, DateTime EndingTime, double Distance, string EndPoint)
        {
            DateTime Now = DateTime.Now;
            var v1 = Roots.Support.SQLLiteDateTimes.DateTimeSQLite(StartingTime);
            var v2 = Roots.Support.SQLLiteDateTimes.DateTimeSQLite(EndingTime);
            Distance = Distance * 1.61;

            string FormattedEndPoint = string.Format("'{0}'", EndPoint);

            string s = string.Format("INSERT INTO [JOURNEY] (Driver,Trip,JourneyStarted,JourneyEnded,JourneyDistance,EndPoint) VALUES ({0},{1},'{2}','{3}',{4},{5})"
                , Driver, Trip, v1, v2, Distance, FormattedEndPoint);

            return s;
        }

        private string GetDistanceAndLastTimeString(int Id, double distance)
        {
            DateTime Now = DateTime.Now;
            var v = Roots.Support.SQLLiteDateTimes.DateTimeSQLite(Now);

            string s = string.Format("UPDATE [JOURNEY] SET JourneyEnded = '{0}', JourneyDistance={1} WHERE Id= {2}", v, distance, Id);
            return s;
        }

        private string GetEndPointSQLString(int id, string placeName)
        {
            string s = string.Format("UPDATE [JOURNEY] SET EndPoint = '{0}' WHERE Id={1}", placeName, id);
            return s;
        }

        private string GetRecordsFromTrip(int Id)
        {
            string s = "SELECT * FROM JOURNEY WHERE Trip = " + Id;
            return s;
        }

        private string GetRecordsFromTodayString(int Id)
        {
            //string s = "SELECT * FROM JOURNEY WHERE Trip =" + Id;
            string s = "SELECT * FROM JOURNEY WHERE Trip =" + Id + " AND JourneyStarted BETWEEN DATE('now') AND DATE('now', '+1 day')";
            return s;
        }

        private string GetLastTripString()
        {
            string s = "SELECT MAX(Id) FROM JOURNEY;";
            return s;
        }
    }
}