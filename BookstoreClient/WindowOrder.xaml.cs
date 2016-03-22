﻿using System;
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
using System.Windows.Shapes;

namespace BookstoreClient
{
    /// <summary>
    /// Interaction logic for WindowOrder.xaml
    /// </summary>
    public partial class WindowOrder : Window
    {
        ObservableCollection<BookOrder> _SelectedBooks = new ObservableCollection<BookOrder>();

        public WindowOrder()
        {
            InitializeComponent();
        }

        public ObservableCollection<BookOrder> SelectedBooks
        {
            get
            {
                return _SelectedBooks;
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class BookOrder : BookSelected
    {
        public int Missing { get; set; }
    }

}
