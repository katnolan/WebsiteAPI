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

            int clubid = 16;


            //List<Event> events = new EventService(clubid, null, null, null).GetEvents();

            List<Personnel> employees = new PersonnelService(clubid, null, null, null).GetPersonnel();

            //events.ForEach(Console.WriteLine);

            employees.ForEach(Console.WriteLine);



        }

            

    }
}
