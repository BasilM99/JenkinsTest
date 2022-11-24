using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Core
{
    public sealed class ExceptionHandler
    {

        public static void ExHandler(Exception exception)
        {

            string logFile = @"C:/logs/weblog.txt";

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exception.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exception.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exception.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exception.InnerException.Source);
                if (exception.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exception.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exception.GetType().ToString());
            sw.WriteLine("Exception: " + exception.Message);
            sw.WriteLine("Source: " + exception);
            //sw.WriteLine("Stack Trace: ");
            //if (exception.StackTrace != null)
            //{
            //    sw.WriteLine(exception.StackTrace);
            //    sw.WriteLine();
            //}
            sw.WriteLine("**************** END *******************");
            sw.Close();
        }

    }
}
