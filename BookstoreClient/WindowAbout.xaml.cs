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
using System.Reflection;

namespace BookstoreClient
{
    /// <summary>
    /// Interaction logic for WindowAbout.xaml
    /// </summary>
    public partial class WindowAbout : Window
    {
        public WindowAbout()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LabelAbout.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
