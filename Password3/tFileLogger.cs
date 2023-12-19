using System;
using System.Threading.Tasks;

public class tFileLogger
{
        private readonly object _theLock = new object();
        private string _filePath = new string("applicationlog.txt");

        public tFileLogger(string theFilePath) {
            _filePath = theFilePath;
            Log("Logging started");
        }
        public tFileLogger() {
            Log("Logging started");
        }
        public static string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyyMMdd HH:mm:ss:ffff");
        }
        public void Log(string message)
        {
            lock (_theLock)
            {
                using (StreamWriter streamWriter = new StreamWriter(_filePath, append: true))
                {
                    streamWriter.WriteLine(GetTimestamp()+" : "+message);
                    streamWriter.Close();
                }
            }
        }
    }