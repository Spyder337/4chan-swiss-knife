using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4chan_swiss_knife.main
{
    public class _4chan_threads
    {
        public static List<Thread> Threads = new List<Thread>();
        private string saveFolder = Microsoft.WindowsAPICodePack.Shell.KnownFolders.Documents.Path;
        private HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
        private HtmlAgilityPack.HtmlDocument doc;
        private int sleepTimer = 10000; //Time in milliseconds
        private Task ThreadWatcher;
        

        public _4chan_threads()
        {
            ThreadWatcher = new Task(checkPages);
            ThreadWatcher.Start();
        }

        public void AddThread(string url)
        {
            Thread t = new Thread();
            t.name = "God Hates Me!!!!!";
            t.url = url;
            t.timer = sleepTimer;
            t.elapsedTimer = 0;
            t.downloadedImages = 0;
            t.lastChecked = DateTime.MinValue;
            Threads.Add(t);
        }

        private void checkPages()
        {

        }
    }

    public class Thread
    {
        public string name;
        public string url;
        public int timer; //In milliseconds
        public int elapsedTimer;
        public int downloadedImages;
        public DateTime lastChecked;
        
        override public string ToString()
        {
            string returnString = "";
            returnString = $"Name : {name}\n\tUrl : {url}";
            return returnString;
        }
    }
}
