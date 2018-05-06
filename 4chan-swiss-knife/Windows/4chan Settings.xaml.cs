using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _4chan_swiss_knife.Windows
{
    /// <summary>
    /// Interaction logic for _4chan_Settings.xaml
    /// </summary>
    public partial class _4chan_Settings : Window
    {
        public _4chan_Settings()
        {
            InitializeComponent();
            currSleepTimer.Text = main.thread_watcher_controller.MaxUpdateInterval.ToString();
        }

        private void submitTime_Click(object sender, RoutedEventArgs e)
        {
            if (newSleepTime.Text == null)
                return;
            if(Int32.TryParse(newSleepTime.Text, out int newTime))
            {
                if (newTime > 30)
                {
                    main.thread_watcher_controller.MaxUpdateInterval = newTime;
                    currSleepTimer.Text = newTime.ToString();
                }
            }
        }
    }
}
