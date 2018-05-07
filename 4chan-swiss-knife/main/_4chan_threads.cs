using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _4chan_swiss_knife.main
{
    public class _4chan_threads
    {
        public static BindingList<Thread> Threads = new BindingList<Thread>();
        //public static ObservableCollection<Thread> Threads = new ObservableCollection<Thread>();
        private string saveFolder = Microsoft.WindowsAPICodePack.Shell.KnownFolders.Documents.Path;
        private HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
        private HtmlAgilityPack.HtmlDocument doc;
        private System.Net.WebClient webClient;
        private int sleepTimer = 10000; //Time in milliseconds
        private Task ThreadWatcher;
        
        public bool AddThread(string url)
        {
            try
            {
                doc = web.Load(url);
                if (web.StatusCode != System.Net.HttpStatusCode.OK)
                    return false;
                Thread t = new Thread();
                t.Board = doc.DocumentNode.SelectSingleNode(@"//*[@class=""boardTitle""]").InnerText;
                t.Name = doc.DocumentNode.SelectSingleNode(@"//*[@class=""subject""]").InnerText;
                if (t.Name.Equals("") || t.Name == null)
                {
                    t.Name = doc.DocumentNode.SelectSingleNode(@"//*[@class=""thread""]").Id.Substring(1);
                }

                t.directory = Path.Combine(thread_watcher_controller.RootFolderLocation, t.Board.Replace("/", "_"), t.Name.Replace("/", "_"));
                CreateDirectory(t.directory);

                t.Url = url;
                t.timer = 0;
                t.elapsedTimer = 0;
                t.DownloadedImages = 0;
                t.lastChecked = DateTime.MinValue;
                Threads.Add(t);
                return true;
            } catch(Exception e)
            {

            }
            return false;
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public void ScrapeThread(Thread thread)
        {
            Console.WriteLine($"Scrape thread : {thread.Name}");
            Console.WriteLine($"Scrape thread : {thread.directory}");
            doc = web.Load(thread.Url);
            var images = doc.DocumentNode.SelectNodes(@"//*[@class=""fileText""]");
            string[] files = Directory.GetFiles(thread.directory);
            foreach(var image in images)
            {
                webClient = new System.Net.WebClient();
                string fileName = image.ChildNodes["a"].InnerText;
                string fileLocation = "http://" + image.ChildNodes["a"].Attributes["href"].Value.Substring(2);

                if (!File.Exists(Path.Combine(thread.directory, fileName)))
                {
                    Console.WriteLine(Path.Combine(thread.directory, fileName));
                    webClient.DownloadFile(fileLocation, Path.Combine(thread.directory, fileName));
                    thread.DownloadedImages++;
                }
            }
        }
    }

    public class Thread : INotifyPropertyChanged
    {
        private string board = String.Empty;
        private string name = String.Empty;
        private string url = String.Empty;
        public string directory;
        public int timer; //In Seconds starts at 0 and increases to 120 seconds in 10 second intervals
        public int elapsedTimer;
        private int downloadedImages;
        public int images;
        public DateTime lastChecked;

        private HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
        private HtmlAgilityPack.HtmlDocument doc;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Board
        {
            get
            {
                return this.board;
            }
            set
            {
                if(value != this.board)
                {
                    this.board = value;
                    NotifyPropertyChanged("Board");
                }
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if(value != this.name)
                {
                    this.name = value;
                    this.directory = Path.Combine(thread_watcher_controller.RootFolderLocation, this.Board.Replace("/", "_"), this.Name.Replace("/", "_"));
                    _4chan_threads.CreateDirectory(this.directory);
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                if(this.url != value)
                {
                    this.url = value;
                    NotifyPropertyChanged("Url");
                }
            }
        }

        public int DownloadedImages
        {
            get
            {
                return this.downloadedImages;
            }
            set
            {
                if(this.downloadedImages != value)
                {
                    this.downloadedImages = value;
                    NotifyPropertyChanged("DownloadedImages");
                }
            }
        }

        override public string ToString()
        {
            string returnString = "";
            returnString = $"Name : {name}\n\tUrl : {url}\n\tDownloaded : {downloadedImages}";
            return returnString;
        }
    }
}
