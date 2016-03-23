using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using BookstoreServiceInterface;
using System.IO;
using System.Web.Script.Serialization;

namespace BookstoreServer
{
    internal class CBookService
    {
        static internal List<Book> Books = new List<Book>(); // This is the book database

        ServiceHost Host;

        internal void StartService()
        {
            Host = new ServiceHost(typeof(BookServiceImpl));
            Host.Open();
            Program.AddLog("WCF service started");
        }

        internal void Stop()
        {
            // Dont try to close it if it is not running
            if (Host != null)
            {
                if (Host.State == CommunicationState.Opened)
                {
                    Program.AddLog("Shutting down WCF service");
                    Host.Close();
                }
            }
        }

        internal void LoadData()
        {
            Book B2;

            RootObject ro = null;
            try
            {
                // Read JSON file to string variable and deserialize it
                StreamReader sr = new StreamReader("Data\\books.json");
                string jsonString = sr.ReadToEnd();
                sr.Close();
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ro = ser.Deserialize<RootObject>(jsonString);

                // Copy object to a class with correct interface
                foreach (book B in ro.books)
                {
                    B2 = new Book();
                    B2.Title = B.title;
                    B2.Author = B.author;
                    B2.Price = Convert.ToDecimal(B.price);
                    B2.InStock = B.inStock;
                    CBookService.Books.Add(B2);
                }
                Program.AddLog("Book data loaded");
            }
            catch (Exception Ex)
            {
                Program.AddLog("Could not read data: " + Ex.Message);
            }
        }

    }

    [ServiceContract]
    public interface IBookServiceImpl
    {
        [OperationContract]
        Book[] GetBooks(string searchString);
    }

    public class BookServiceImpl : IBookServiceImpl
    {
        public Book[] GetBooks(string searchString)
        {
            List<Book> MatchedBooks = new List<Book>();

            Program.AddLog("Search string: " + searchString);
            foreach (Book B in CBookService.Books)
            {
                // Convert to upper so the search gets case insensitive
                if (B.Title.ToUpper().IndexOf(searchString.ToUpper()) >= 0 || B.Author.ToUpper().IndexOf(searchString.ToUpper()) >= 0)
                {
                    MatchedBooks.Add(B);
                }
            }
            return MatchedBooks.ToArray();
        }
    }

    // For JSON deserialization
    public class RootObject
    {
        public List<book> books;
    }

    // For JSON deserialization
    public class book
    {
        public string title { get; set; }
        public string author { get; set; }
        public double price { get; set; }
        public int inStock { get; set; }
    }

}
