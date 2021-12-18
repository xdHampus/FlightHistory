using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightHistoryCore;
using FlightHistoryCore.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightHistoryScraper
{
    public class Scraper
    {
        private const string radarAreaAPIUrl = "https://data-live.flightradar24.com/zones/fcgi/feed.js?faa=1&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        private const string flightUrl = "https://data-live.flightradar24.com/clickhandler/?version=1.5";
        private const string bounds = "78.618%2C39.928%2C-121.739%2C-37.101";
        private const int maxRescanFlightAttempts = 5;

        private string radarAreaUrl = $"{radarAreaAPIUrl}&bounds={bounds}";
        //private ScanQueue scanTargets = new();
        private Queue<string> scanTargets = new();

        private WebRequester webRequester = WebRequester.Instance;


        public Scraper()
        {
            using var db = new FlightDbContext();
            db.Database.EnsureCreated();
        }

        public void StartScraper()
        {
            ScanArea();
            ScanQueuedItems();

            StartScraper();
        }

        private void ScanArea()
        {
            Console.WriteLine($"Starting area scan at {CurrentDateTime()}.");
            string areaData = GetAreaData();
            if (!string.IsNullOrEmpty(areaData))
            {
                List<string> flightIds = GetFlightIds(areaData);
                for (int i = 0; i < flightIds.Count; i++)
                {
                    Console.WriteLine($"Area scanning flight {flightIds[i]} at {CurrentDateTime()} - {i} out of {flightIds.Count}.");
                    AreaScanFlight(flightIds[i]);
                }
            }
            Console.WriteLine($"Ending area scan at {CurrentDateTime()}.");
        }

        private void AreaScanFlight(string fId)
        {
            if (!scanTargets.Contains(fId))
            {
                if (!FlightInDB(fId))
                {
                    Console.WriteLine("Flight is new.");
                    ScanNewFlight(fId);
                }
                else if (FlightScanIncomplete(fId))
                {
                    Console.WriteLine("Flight was not scanned properly, queuing again.");
                    scanTargets.Enqueue(fId);
                }
            }
            else
            {
                Console.WriteLine("Flight already being scanned.");
            }
        }


        private bool FlightScanIncomplete(string id)
        {
            using var db = new FlightDbContext();
            Flight flight = db.Flights
                .Where(f => f.Identification.Id == db.FlightIdentifications
                    .Where(fId => fId.FlightIdentifier.Equals(id))
                    .FirstOrDefault().Id)
                .FirstOrDefault();
                
            return !flight.ScanCompleted.GetValueOrDefault();
        }

        private bool FlightInDB(string id)
        {

            using var db = new FlightDbContext();
            return db.FlightIdentifications
                    .Any(fId => fId.FlightIdentifier.Equals(id));
        }

        private void ScanNewFlight(string id)
        {
            Json.Flight jsonFlight = GetJsonFlight(id);
            if(jsonFlight != null)
            {
                using var db = new FlightDbContext();
                Flight flight = jsonFlight.Convert();

                if(flight != null && flight.Identification != null)
                {
                    db.Flights.Add(flight);
                    db.SaveChanges();
                    scanTargets.Enqueue(id);
                } 
                else
                {
                    Console.WriteLine("Error converting new flight.");
                }
            }
            else
            {
                Console.WriteLine("Error scanning new flight.");
            }
        }

        private void RescanFlight(string fId, int tries = 1)
        {
            using var db = new FlightDbContext();

            Flight flight = db.Flights
                .Where(f => f.Identification.Id == db.FlightIdentifications
                    .Where(id => id.FlightIdentifier.Equals(fId))
                    .FirstOrDefault().Id)
                .Include("Status")
                .Include("Trails")
                .FirstOrDefault();

            var res = GetJsonFlight(fId);

            if (res != null && res.Trail != null)
            {
                if (res.Trail.Count > flight.Trails.Count)
                {
                    Console.WriteLine($"Found {res.Trail.Count - flight.Trails.Count} new trails");
                    flight.Trails.AddRange(res.Trail
                        .Skip(flight.Trails.Count)
                        .ToList()
                        .ConvertAll(t => t.Convert()));
                }

                if (res.Status != null && !res.Status.Live.GetValueOrDefault())
                {
                    Console.WriteLine("Scan completed.");
                    flight.Status.Text = res.Status.Text;
                    flight.ScanCompleted = true;
                }
                else
                {
                    Console.WriteLine("Flight not completed, queuing again.");
                }


                db.SaveChanges();
                if (!flight.ScanCompleted.Value)
                {
                    scanTargets.Enqueue(fId);
                }
            }
            else if(tries < maxRescanFlightAttempts)
            {
                tries += 1;
                Console.WriteLine($"Scan failed, retrying {tries} out of {maxRescanFlightAttempts - 1}.");
                RescanFlight(fId, tries);
            }
            else
            {
                Console.WriteLine($"Scan failed too many attempts, cancelling scan.");
                flight.ScanCompleted = true;
                flight.Status.Live = true;
                flight.Status.Text = "Error";
                db.SaveChanges();
            }
        }

        private void ScanQueuedItems()
        {
            int initSize = scanTargets.Count;
            for (int i = 0; i < initSize; i++)
            {
                string fId = scanTargets.Dequeue();
                Console.WriteLine($"Recanning flights {fId} at {CurrentDateTime()} - {i} out of {initSize}.");
                RescanFlight(fId);
            }
        }




        private Json.Flight GetJsonFlight(string id)
        {
            var data = GetFlightData(id);
            var result = Json.Flight.FromJson(data);

            if (result == null || result.Trail == null)
            {
                Console.WriteLine($"Error getting data from flight {id}; pasting data below");
                Console.WriteLine(data);
            }

            return result;
        }

        private List<string> GetFlightIds(string jsonRadarArea)
        {
            var result = new List<string>();
            foreach (var item in Json.RadarArea.FromJson(jsonRadarArea).ToList())
            {
                if (item.Value.AnythingArray != null && item.Value.AnythingArray.Count == 19) //why is this 19?
                {
                    result.Add(item.Key);
                }
            }
            return result;
        }


        private string GetFlightData(string id)
        {
            return webRequester.GetHTMLAsync($"{flightUrl}&flight={id}").Result;
        }

        private string GetAreaData()
        {
            return webRequester.GetHTMLAsync(radarAreaUrl).Result;
        }

        private string CurrentDateTime()
        {
            return DateTime.Now.ToString("dd MMMM yyy HH:mm:ss");
        }

    }
}
