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
    public class MyFileWriter
    {
        private string _sdcardPath;

        private string _fileName;

        private string _filePath;

        private StreamWriter _writer = null;

        public MyFileWriter()
        {

            _sdcardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            GenerateFileName();
            _writer = new StreamWriter(_filePath);

        }

        private void GenerateFileName()
        {
            string _baseName = "HolidayExport_";
            DateTime now = DateTime.Now;

            _fileName = string.Format("{0}{1}{2}{3}_{4}{5}",
                _baseName
                , Convert.ToString(now.Year).PadLeft(2, '0')
               , Convert.ToString(now.Month).PadLeft(2, '0')
                , Convert.ToString(now.Day).PadLeft(2, '0')
                , Convert.ToString(now.Hour).PadLeft(2, '0')
                , Convert.ToString(now.Minute).PadLeft(2, '0'));
            _filePath = System.IO.Path.Combine(_sdcardPath, _fileName);
            _filePath = _filePath + ".txt";
        }

        public bool WriteLine(string line)
        {
            try
            {
                _writer.WriteLine(line);
                _writer.Flush();

                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public bool CloseResource()
        {
            try
            {
                _writer.Close();
                _writer.Dispose();
                _writer = null;

                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}