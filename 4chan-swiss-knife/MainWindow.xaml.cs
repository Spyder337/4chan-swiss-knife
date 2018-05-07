using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Web;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace _4chan_swiss_knife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BackgroundWorker worker;
        KeyEventHandler keyEvent;
        public MainWindow()
        {
            InitializeComponent();
            keyEvent += new KeyEventHandler(MainWindow_Shortcuts);
            App.threads.AddThread("http://boards.4chan.org/g/thread/65819304");
            App.threads.AddThread("http://boards.4chan.org/g/thread/65817442");
            urlList.ItemsSource = main._4chan_threads.Threads;
            
        }

        private void MainWindow_Shortcuts(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.O)
            {
                if(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (urlList.SelectedItem == null)
                        return;
                    foreach (object o in urlList.Items)
                    {
                        if ((o is main.Thread) && o == urlList.SelectedItem)
                        {

                            System.Diagnostics.Process.Start(((main.Thread)o).directory);
                        }
                    }
                }
            }
        }

        private void urlSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (urlBar.Text.Equals(""))
                return;
            App.threads.AddThread(urlBar.Text);
            urlList.ItemsSource = null;
            urlList.ItemsSource = main._4chan_threads.Threads;
            urlBar.Text = "";
        }

        private void urlRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(urlList.SelectedItem != null)
            {
                main._4chan_threads.Threads.Remove(((main.Thread)urlList.SelectedItem));
                urlList.ItemsSource = null;
                urlList.ItemsSource = main._4chan_threads.Threads;
            }
            else
            {
                return;
            }
        }

        private void urlList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (urlList.SelectedItem == null)
                return;
            foreach(object o in urlList.Items)
            {
                if((o is main.Thread) && o == urlList.SelectedItem)
                {
                    
                    System.Diagnostics.Process.Start(((main.Thread)o).Url);
                }
            }
        }

        private void startWatcher_Click(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += watcherDoWork;
            worker.RunWorkerAsync();
        }

        void watcherDoWork(object sender, DoWorkEventArgs e)
        {
            foreach(main.Thread t in urlList.Items)
            {
                App.threads.ScrapeThread(t);
            }
        }

        private void stopWatcher_Click(object sender, RoutedEventArgs e)
        {
        }

        private void threadRenameButton_Click(object sender, RoutedEventArgs e)
        {
            if (urlList.SelectedItem == null || newThreadNameBox.Text == "")
                return;
            foreach (object o in urlList.Items)
            {
                if ((o is main.Thread) && o == urlList.SelectedItem)
                {
                    ((main.Thread)o).Name = newThreadNameBox.Text;
                    newThreadNameBox.Text = "";
                }
            }
        }
    }
}
