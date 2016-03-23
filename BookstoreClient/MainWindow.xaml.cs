using BookstoreServiceInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        BookstoreService Service= new BookstoreService();
        ObservableCollection<IBook> _FoundBooks = new ObservableCollection<IBook>();
        ObservableCollection<BookSelected> _SelectedBooks = new ObservableCollection<BookSelected>();

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

        public ObservableCollection<BookSelected> SelectedBooks
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
            AddDlg.TitelAuthor = B.Title + " - " + B.Author;
            if (AddDlg.ShowDialog() == true) // Did the user click OK
            {
                BookSelected NewBook = new BookSelected();
                BookSelected SelectedBookFound=null;
                    bool Found = false;

                // Check if there already is a line for this book
                foreach(BookSelected SelectedBook in _SelectedBooks)
                {
                    if(B.Title==SelectedBook.Title && B.Author==SelectedBook.Author)
                    {
                        Found = true;
                        SelectedBookFound = SelectedBook;
                    }
                }

                if (Found == false)
                {
                    // Add a new order line
                    NewBook.Title = B.Title;
                    NewBook.Author = B.Author;
                    NewBook.Price = B.Price;
                    NewBook.Quantity = AddDlg.Quantity;
                    _SelectedBooks.Add(NewBook);
                    ButtonPlaceOrder.IsEnabled = true;
                }
                else
                {
                    // Add to existing line
                    SelectedBookFound.Quantity += AddDlg.Quantity;
                }
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

            foreach (BookSelected BO in _SelectedBooks)
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
                _SelectedBooks.Remove((BookSelected)ListViewSelected.SelectedItem);
                UpdateTotalPrice();
                if(_SelectedBooks.Count==0) ButtonPlaceOrder.IsEnabled = false; // Cant place empty orders
            }
        }

        private void MenuItemHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            WindowAbout AboutDlg = new WindowAbout();

            AboutDlg.Owner = this;
            AboutDlg.ShowDialog();
        }

        private void ButtonPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            WindowOrder OrderDlg = new WindowOrder();

            OrderDlg.Owner = this;
            // Copy to order list and calculate missing in stock
            foreach(BookSelected BS in _SelectedBooks)
            {
                BookOrder BO = new BookOrder();
                int Missing;

                BO.Title = BS.Title;
                BO.Author = BS.Author;
                BO.Price = BS.Price;
                BO.Quantity = BS.Quantity;
                Missing = (int)BS.Quantity - BookInStock(BS.Title, BS.Author);
                BO.Missing = Missing>0?Missing:0;
                OrderDlg.SelectedBooks.Add(BO);
            }
            if(OrderDlg.ShowDialog()==true)
            {
                // If users clicked OK clear list
                _SelectedBooks.Clear();
                ButtonPlaceOrder.IsEnabled = false;
            }
            UpdateTotalPrice();
        }

        private int BookInStock(string Title,string Author)
        {
            int InStock = 0;

            foreach(IBook B in _FoundBooks)
            {
                if(B.Title==Title && B.Author==Author)
                {
                    InStock = B.InStock;
                }
            }
            return InStock;
        }

    }

    public class BookSelected : INotifyPropertyChanged
    {
        private uint quantity;

        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public uint Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Quantity"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
