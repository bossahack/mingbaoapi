using System;
using System.Diagnostics;
using System.IO;

namespace Book.Job
{
    public class LogTraceListener : TraceListener
    {
        public string FilePath { get; private set; }
        private DateTime _CurrentDate;
        StreamWriter _TraceWriter;

        public LogTraceListener(string filepath)
        {
            FilePath = filepath;
            _TraceWriter = new StreamWriter(GenerateFileName(), true);
        }

        private string GenerateFileName()
        {
            _CurrentDate = DateTime.Today;
            return Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath) + "_" + _CurrentDate.ToString("yyyyMMdd") + Path.GetExtension(FilePath));
        }

        public override void Write(string message)
        {
            _TraceWriter.Write(DateTime.Now.ToString("HH:mm:ss ") + message);
        }

        public override void WriteLine(string message)
        {
            CheckRollover();
            _TraceWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + message);
        }

        private void CheckRollover()
        {
            if (_CurrentDate.CompareTo(DateTime.Today) != 0)
            {
                _TraceWriter.Close();
                _TraceWriter = new StreamWriter(GenerateFileName(), true);
            }
        }

    }
}
