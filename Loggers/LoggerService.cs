using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggers
{
    public class LoggerService
    {
        private static object _locker = new object();
        private Logger logger;
        public LoggerService()
        {
            logger = LogManager.GetLogger("CourseStoreLoggers");
        }
        public void Error(string email, string ev, string source)
        {
            lock (_locker)
                logger.Error($"{ev} for: {email}; Source: {source}");
        }

        public void Info(string email, string ev, string source)
        {
            lock (_locker)
                logger.Info($"{ev} for: {email};  Source: {source}");
        }

        public void Warning(string email, string ev, string source)
        {
            lock (_locker)
                logger.Warn($"{ev} for: {email};  Source: {source}");
        }
        public void DeleteFiles()
        {
            while (true)
            {
                try
                {
                    var path = AppDomain.CurrentDomain.BaseDirectory + "/logs";
                    if (Directory.Exists(path))
                    {
                        var files = Directory.GetFiles(path);
                        foreach (var file in files)
                        {
                            try
                            {
                                var fileDateFixed = file.Substring(file.Length - 10);
                                var dtFile = DateTime.Parse(fileDateFixed);
                                if ((DateTime.Now - dtFile).TotalDays >= 7)
                                {
                                    File.Delete(file);
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
                System.Threading.Thread.Sleep(60 * 60 * 1000);
            }
        }
    }
}
