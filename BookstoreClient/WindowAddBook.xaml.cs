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

namespace BookstoreClient
{
    /// <summary>
    /// Interaction logic for WindowAddBook.xaml
    /// </summary>
    public partial class WindowAddBook : Window
    {
        internal uint Quantity;
        internal string TitelAuthor;

        public WindowAddBook()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxCount.Text = Quantity.ToString();
            LabelTitelAuthor.Content = TitelAuthor;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (uint.TryParse(TextBoxCount.Text, out Quantity))
            {
                DialogResult = true;
                Close();
            }
            else MessageBox.Show(this, "Error in Quantity");
        }
    }
}
