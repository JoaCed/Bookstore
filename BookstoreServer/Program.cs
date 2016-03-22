using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreServer
{
    class Program
    {
        static CBookService BookService;

        static void Main(string[] args)
        {
            BookService = new CBookService();
            bool StartOK = false;

            // Load data from JSON file
            BookService.LoadData();

            try
            {
                // Start up WCF service
                BookService.StartService();

                StartOK = true;
            }
            catch(Exception Ex)
            {
                AddLog(Ex.Message);
            }
            if(StartOK)  // The service must have started OK
            {
                Console.WriteLine("Press a key to stop service");
                Console.ReadKey();
                BookService.Stop();
            }
        }

        // Now it only write the text to the console window
        // If it was a Windows Service it would have to write it somewhere else
        static internal void AddLog(string Text)
        {
            Console.WriteLine(string.Format("{0:s} : {1}", DateTime.Now, Text));
        }

    }


}
