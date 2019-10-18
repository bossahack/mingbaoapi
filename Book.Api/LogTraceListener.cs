using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Book.Api
{
    public class LogTraceListener: TraceListener
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
            File.AppendAllText(FilePath, message);
        }

        public override void WriteLine(string message)
        {
            CheckRollover();
            _TraceWriter.WriteLine(message);
            //File.AppendAllText(FilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message + Environment.NewLine);
        }

        private void CheckRollover()
        {
            if (_CurrentDate.CompareTo(DateTime.Today) != 0)
            {
                _TraceWriter.Close();
                _TraceWriter = new StreamWriter(GenerateFileName(), true);
            }
        }

        //public override void Write(object o, string category)
        //{
        //    string msg = "";

        //    if (string.IsNullOrWhiteSpace(category) == false) //category参数不为空
        //    {
        //        msg = category + " : ";
        //    }

        //    if (o is Exception) //如果参数o是异常类,输出异常消息+堆栈,否则输出o.ToString()
        //    {
        //        var ex = (Exception)o;
        //        msg += ex.Message + Environment.NewLine;
        //        msg += ex.StackTrace;
        //    }
        //    else if (o != null)
        //    {
        //        msg = o.ToString();
        //    }

        //    WriteLine(msg);
        //}
    }
}