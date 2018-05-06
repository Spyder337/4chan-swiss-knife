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

namespace _4chan_swiss_knife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.threads.AddThread("cock.li");
            urlList.ItemsSource = main._4chan_threads.Threads;
        }

        private void urlSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (urlBar.Text.Equals(""))
                return;
            urlList.Items.Add(urlBar.Text);
            urlBar.Text = "";
        }

        private void urlRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(urlList.SelectedItem != null)
            {
                urlList.Items.Remove(urlList.SelectedItem);
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
            Windows._4chan_Settings settings = new Windows._4chan_Settings();
            settings.Owner = this;
            settings.Show();
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            main._4chan_threads.Threads.RemoveAt(0);
        }
    }
}
