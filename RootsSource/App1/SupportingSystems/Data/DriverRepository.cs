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
    public class DriverRepository:BaseDataAccessingClass
    {
        public bool InsertNewDriver(string Name)
        {
            try
            {
                string SQLString = GetInsertDriverString(Name);

                connection = new SqliteConnection("Data Source=" + GetPathToDatabase());
                connection.Open();

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = SQLString;
                    var k = c.ExecuteNonQuery();
                }

                using (var c = connection.CreateCommand())
                {
                    c.CommandText = "SELECT * FROM DRIVERS;";
                    var k = c.ExecuteReader();
                }

                connection.Close();
                connection.Dispose();
                connection = null;
                
                return true;

            } catch (Exception)
            {
                return false;
            }
        }

        public int GetLastDriverID()
        {
            try
            {
                string SQLString = GetLastDriverString();

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

        private string GetInsertDriverString(string name)
        {
            string s = "INSERT INTO [DRIVERS] (Name,ImageName,Registered,Active) VALUES ('" + name + "','"+ AvatarManager.DefaultSaveImageName +"','2017-1-1 00:00:00.000',1);";
            return s;
        }

        private string GetLastDriverString()
        {
            string s = "SELECT MAX(Id) FROM DRIVERS;";
            return s;
        }
    }
}