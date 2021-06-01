using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightHistoryCore.Model;

namespace FlightHistoryScraper
{
    public class ScanQueue : Queue<Flight>
    {
        private HashSet<string> currentFlights = new();


        public new void Enqueue(Flight item)
        {
            if(item != null && item.Identification != null)
            {
                string id = item.Identification.FlightIdentifier;
                if (!currentFlights.Contains(id))
                {
                    currentFlights.Add(id);
                    base.Enqueue(item);
                }
            }
        }
        
        public new Flight Dequeue()
        {
            Flight flight = base.Dequeue();
            currentFlights.Remove(flight.Identification.FlightIdentifier);
            return flight;
        }

        public bool FlightExists(string id)
        {
            return currentFlights.Contains(id);
        }


    }
}
