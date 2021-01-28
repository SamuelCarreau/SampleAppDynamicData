using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;

namespace TestDynamicsData.Helpers
{
    public static class LogHelper
    {
        public static void LogException(Exception exception)
        {
            if (exception is null) { return; }
            Debug.WriteLine(ExceptionToString(exception));
        }

        private static string ExceptionToString(Exception exception)
        {
            int count = 0;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("#########################");
            do
            {

                builder.AppendLine("Count: " + count);
                builder.AppendLine("MESSAGE: " + exception.Message);
                builder.AppendLine("STACKTRACE: " + exception.StackTrace);

                count++;
                exception = exception.InnerException;
            } while (exception != null);
            builder.AppendLine("#########################");
            builder.AppendLine();
            return builder.ToString();
        }

        public static void Log(string message)
        {
            Debug.WriteLine(PrependTime(message));
        }

        public static void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Debug.WriteLine(PrependTime(message));
        }

        private static string PrependTime(string text)
        {
            return string.Concat($"[{DateTime.Now.ToString("T")}]",text);
        }
    }
}
