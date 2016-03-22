using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace BookstoreServiceInterface
{

    public interface IBook
    {
        [DataMember]
        string Title { get; }
        [DataMember]
        string Author { get; }
        [DataMember]
        decimal Price { get; }
        [DataMember]
        int InStock { get; }
    }

    public interface IBookstoreService
    {
        Task<IEnumerable<IBook>> GetBooksAsync(string searchString);
    }

    public class BookstoreService : IBookstoreService
    {
        IBookServiceImpl Service;
        ChannelFactory<IBookServiceImpl> SCF;

        public Task<IEnumerable<IBook>> GetBooksAsync(string searchString)
        {
            Book[] Books;
            Task<IEnumerable<IBook>> T;
            NetTcpBinding TcpBinding;

            TcpBinding = new NetTcpBinding();
            TcpBinding.OpenTimeout = new TimeSpan(0, 0, 5);
            TcpBinding.SendTimeout= new TimeSpan(0, 0, 5);
            T= new Task<IEnumerable<IBook>>(() =>  // Lambda expression
            {
                if(SCF== null) SCF = new ChannelFactory<IBookServiceImpl>(TcpBinding, "net.tcp://localhost:4567/BookService");
                Service = SCF.CreateChannel();
                Books = null;

                try
                {
                    Books = Service.GetBooks(searchString);
                }
                catch (Exception Ex)
                {
                    throw new BookServiceException("Could not connect channel: "+Ex.Message);
                }
                return Books;
            });
            T.Start();
            return T;
        }
    }

    [ServiceContract]
    public interface IBookServiceImpl
    {
        [OperationContract]
        Book[] GetBooks(string searchString);
    }

    [DataContract]
    public class Book : IBook
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int InStock { get; set; }
    }

    public class BookServiceException : Exception
    {
        public BookServiceException(string EText)
        {
            ErrorText = EText;
        }

        public string ErrorText;
    }

}
