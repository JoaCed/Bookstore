using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Net.Sockets;

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
        IBookServiceImpl Service=null;
        ChannelFactory<IBookServiceImpl> CF=null;
        NetTcpBinding TcpBinding=null;

        public Task<IEnumerable<IBook>> GetBooksAsync(string searchString)
        {
            Book[] Books=null;
            Task<IEnumerable<IBook>> T;
            uint RetryCount = 0;

            if (TcpBinding == null)
            {
                TcpBinding = new NetTcpBinding();
                // The timeouts must be short or it will take a long time before the user see any error message
                TcpBinding.OpenTimeout = new TimeSpan(0, 0, 5);
                TcpBinding.SendTimeout = new TimeSpan(0, 0, 5);
            }
            T= new Task<IEnumerable<IBook>>(() =>  // Lambda expression
            {
                if(CF== null) CF = new ChannelFactory<IBookServiceImpl>(TcpBinding, "net.tcp://localhost:4567/BookService");


                // Sometimes a new channel must be created when there is an error
                // Service is set to null if there is an expected exception and a new channel is created and the operation is tried once more
                // I want to use the same channel as long as it works
                while(RetryCount<2)
                {
                    if (Service == null)
                    {
                        System.Diagnostics.Debug.Print("Creating new channel");
                        Service = CF.CreateChannel();
                    }
                    Books = null;

                    try
                    {
                        RetryCount++;
                        System.Diagnostics.Debug.Print("Calling GetBooks()");
                        Books = Service.GetBooks(searchString);
                        return Books;
                    }
                    catch (CommunicationException CEx)
                    {
                        Service = null;
                        if(RetryCount>=2) throw new BookServiceException("Communication error: " + CEx.Message);
                    }
                    catch (TimeoutException TEx)
                    {
                        Service = null;
                        if (RetryCount >= 2) throw new BookServiceException("Timeout error: " + TEx.Message);
                    }
                    catch (SocketException SockEx)
                    {
                        Service = null;
                        if (RetryCount >= 2) throw new BookServiceException("Communication error: " + SockEx.Message);
                    }
                    catch (Exception Ex)
                    {
                        Service = null;
                        throw new BookServiceException("Could not connect channel: " + Ex.Message);
                    }
                }
                return null;  // Calling WCF service did not work
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

    // This is the only exception the client should see from the Book service
    public class BookServiceException : Exception
    {
        public BookServiceException(string EText)
        {
            ErrorText = EText;
        }

        public string ErrorText;
    }

}
