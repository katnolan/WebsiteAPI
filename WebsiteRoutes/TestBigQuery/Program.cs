using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;

namespace TestBigQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine($"Hello World!");



            //Club[] clubs = new ClubsService().Get();


 
            //clubs.ToList().ForEach(Console.WriteLine);

            int clubid = 21;


            List<Event> events = new EventService(clubid, null, null, null).GetEvents();

            events.ForEach(Console.WriteLine);



        }

            

    }
}
