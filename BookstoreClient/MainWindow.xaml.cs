using BookstoreServiceInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookstoreClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BookstoreService Service;
        ObservableCollection<IBook> _FoundBooks = new ObservableCollection<IBook>();
        ObservableCollection<BookOrder> _SelectedBooks = new ObservableCollection<BookOrder>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemFileQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        async private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Service = new BookstoreService();
            IEnumerable<IBook> Books = null;

            ButtonSearch.IsEnabled = false; // I dont want reentrancy
            try
            {
                Books = await Service.GetBooksAsync(TextBoxSearchString.Text);
            }
            catch (BookServiceException BSEx)
            {
                ErrorDialog(BSEx.ErrorText);
            }

            // Put all found books in list view
            _FoundBooks.Clear();
            if (Books != null)
            {
                foreach (IBook B in Books)
                {
                    _FoundBooks.Add(B);
                }
            }
            ButtonSearch.IsEnabled = true;
        }

        void ErrorDialog(string ErrorText)
        {
            MessageBox.Show(this, ErrorText, "Bookstore", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public ObservableCollection<IBook> FoundBooks
        {
            get
            {
                return _FoundBooks;
            }
        }

        public ObservableCollection<BookOrder> SelectedBooks
        {
            get
            {
                return _SelectedBooks;
            }
        }

        private void ButtonAddSelected_Click(object sender, RoutedEventArgs e)
        {
            WindowAddBook AddDlg = new WindowAddBook();
            Book B;

            B = (Book)ListViewFound.SelectedItem;
            AddDlg.Owner = this;
            AddDlg.Quantity = 1;
            AddDlg.TitelAuthor = B.Title + " " + B.Author;
            if (AddDlg.ShowDialog() == true) // Did the user click OK
            {
                BookOrder NewBook = new BookOrder();

                // Add a new order line
                NewBook.Title = B.Title;
                NewBook.Author = B.Author;
                NewBook.Price = B.Price;
                NewBook.Quantity = AddDlg.Quantity;
                _SelectedBooks.Add(NewBook);
                UpdateTotalPrice();
            }
        }

        private void ListViewFound_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewFound.SelectedItems.Count == 1)  // We only handle single selections
            {
                ButtonAddSelected.IsEnabled = true;
            }
            else ButtonAddSelected.IsEnabled = false;
        }

        // Call this to calculate and display the new price amount
        private void UpdateTotalPrice()
        {
            Decimal TotalPrice = 0;

            foreach (BookOrder BO in _SelectedBooks)
            {
                TotalPrice += BO.Price * BO.Quantity;
            }
            LabelTotalPrice.Content = string.Format("Total price: {0:0.00}", TotalPrice);
        }

        private void ListViewSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewSelected.SelectedItems.Count == 1)  // We only handle single selections
            {
                ButtonDeleteOrderLine.IsEnabled = true;
            }
            else ButtonDeleteOrderLine.IsEnabled = false;
        }

        private void ButtonDeleteOrderLine_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSelected.SelectedItems.Count == 1)  // We only handle single selections
            {
                _SelectedBooks.Remove((BookOrder)ListViewSelected.SelectedItem);
                UpdateTotalPrice();
            }
        }

        private void MenuItemHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            WindowAbout AboutDlg = new WindowAbout();

            AboutDlg.Owner = this;
            AboutDlg.ShowDialog();
        }
    }

    public class BookOrder
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public uint Quantity { get; set; }
    }

}
