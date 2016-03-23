Bookstore project

The solution is in three parts. There is a server, client and an interface DLL. All 
applications and the DLL needs .NET Framework 4.6 installed. There is no other 
dependencies.


BookstoreServer
This is the bookstore server. It is an WCF standalone host. Implemented as a command 
line application. It should be started from the command line. When the program starts 
it first reads book data from the JSON file then starts the WCF service. The book data 
file must be in a folder with the name "Data" in current directory and have the name 
books.json. The WCF service listens to the port 4567 TCP. If the port is in use there 
will be an error message. It needs the BookstoreServiceInterface.dll and the 
BookstoreServer.exe.config file in the same folder.

BookstoreClient
This is the user interface. It is a WPF application. It does not need any installation 
program. Just doubleclick on the BookstoreClient.exe file in explorer. It needs the 
BookstoreServiceInterface.dll file in the same folder.

BookstoreServiceInterface
This is the interface DLL. It handles the WCF client part. It also have definitions that 
is needed on both the server and client side. The interface between the client and the DLL 
is simple. The client only has to instanciate an object of type BookstoreService and call 
the member function GetBooksAsync(). The GetBooksAsync() function will return an array of 
books or throw a BookServiceException exception. This makes it easy to use the service from 
some other client.

How to run the solution

1 Start BookstoreServer from the command line. 
2 If it says that data is loaded and WCF service is started it is ready to be used
3 Double click on BookstoreClient.exe in explorer
4 Enter a search string in "Search for" textbox in lower left corner. Empty string finds all books.
5 Click on Search button
6 Click on a row in Found books
7 Click on ">>"
8 A dialog will be displayed. Enter wanted quantity in textbox
9 Click OK
10 A row will be added to the selected books list
11 Click "Place order" button.
12 A dialog will show order information. The missing column shows how many more books are ordered than in stock.
13 Click OK. The only thing that happens is that the selected books list is cleared.
