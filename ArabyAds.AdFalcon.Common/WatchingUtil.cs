using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Noqoush.Framework;
using Noqoush.Framework.Logging;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Common
{

    public static class WatchingUtil
    {

        private static ILog logger = LogManager.GetLogger("watch");
        [ThreadStatic]
        private static Stack<Watcher> _watcherStack;
        //[ThreadStatic]
        //private static List<Watcher> _watcherList = new List<Watcher>();
        private static bool enabled  = bool.TrueString == JsonConfigurationManager.AppSettings["enableWatcherLogs"];
        public static void StartWatch(string name)
        {
            _watcherStack ??= new Stack<Watcher>();
            if (!enabled) return;
            var watcher = new Watcher(name);
            var intendationString = "";
            for (int i = 0; i < _watcherStack.Count; i++)
            {
                intendationString += "|--";
            }
            logger.Debug(intendationString + "Watcher " + name + " Started {");
            watcher.sw.Start();
           // _watcherList.Add(watcher);
            _watcherStack.Push(watcher);

        }

        public static void EndWatch()
        {
            if (!enabled) return;
            var watcher = _watcherStack.Pop();
            if (watcher == null) return;
            watcher.sw.Stop();
            var intendationString = "";
            for (int i = 0; i < _watcherStack.Count; i++)
            {
                intendationString += "|--";
            }
            logger.Debug(intendationString + "Watcher " + watcher._logName + " ended } " + watcher.sw.Elapsed.TotalMilliseconds.ToString("0.###") + "ms");
        }
    }


    public class Watcher
    {
        public Stopwatch sw = new Stopwatch();

        public string _logName;
        public Watcher(string logName)
        {
            _logName = logName;
        }
    }
}
