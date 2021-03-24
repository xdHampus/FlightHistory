using FlightRadarCore;
using FlightRadarCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FlightRaderScrape
{
    class Program
    {

        private const int rateDelay = 1000;
        private const string radarAreaUrl = "https://data-live.flightradar24.com/zones/fcgi/feed.js?faa=1&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        private const string flightUrl = "https://data-live.flightradar24.com/clickhandler/?version=1.5";

        private const int areaScanDelayHours = 0;
        private const int areaScanDelayMinutes = 30;
        private const int areaScanDelaySeconds = 0;


        private const int flightInitialScanDelayHours = 0;
        private const int flightInitialcanDelayMinutes = 5;
        private const int flightInitialScanDelaySeconds = 0;
        private const int flightScanDelayHours = 0;
        private const int flightcanDelayMinutes = 15;
        private const int flightScanDelaySeconds = 0;




        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Dictionary<string, CancellationTokenSource> pairs = new Dictionary<string, CancellationTokenSource>();



            var startTime = DateTime.Now;
            Timer areaScan = new Timer(AreaScanCallback,
                                       pairs,
                                       new TimeSpan(0, 0, 0),
                                       new TimeSpan(areaScanDelayHours, areaScanDelayMinutes, areaScanDelaySeconds));
            

            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Escape)
                {

                    var nextScan = Math.Floor((DateTime.Now - startTime).TotalSeconds % new TimeSpan(areaScanDelayHours,
                                                                                                     areaScanDelayMinutes,
                                                                                                     areaScanDelaySeconds).TotalSeconds)
                        .ToString();
                    var timeSpanNext = new TimeSpan(areaScanDelayHours,    areaScanDelayMinutes, areaScanDelaySeconds) - new TimeSpan(0, 0, int.Parse(nextScan));

                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine($"Service has run for {DateTime.Now - startTime}");
                    Console.WriteLine($"Next area scan in { timeSpanNext }");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    
                }
            }

        }

        private static CancellationTokenSource CreateFlightScanner(string flightId)
        {

            CancellationTokenSource cts = new CancellationTokenSource();

            // Pass the token to the cancelable operation.
            ThreadPool.QueueUserWorkItem(new WaitCallback(FlightScanCallBack), new object[] { flightId, cts });


            return cts;

        }

        private static async void FlightScanCallBack(object objectArray)
        {

            try
            {
                var flightId = (string)((object[])objectArray)[0];
                var tokenSource = (CancellationTokenSource)((object[])objectArray)[1];

                if (tokenSource.Token.IsCancellationRequested)
                {
                    Console.WriteLine($"Called cancelled scanner for flight {flightId}");
                    return;
                }


                Console.WriteLine($"Rescanning flight {flightId}");

                using (var db = new FlightDbContext())
                {
                    db.Database.EnsureCreated();

                    var itemInDb = db.Flights.Include("Trails").Where(fx => fx.Identification.FlightIdentifier == flightId).FirstOrDefault();

                    if (itemInDb != null)
                    {
                        var res = GetFlight(flightId);

                        if (res.Trail != null && res.Trail.Count > itemInDb.Trails.Count)
                        {
                            Console.WriteLine($"Found {res.Trail.Count - itemInDb.Trails.Count} new trails");

                            itemInDb.Trails.AddRange(Json.Flight.ParseTrails(res.Trail.Skip(itemInDb.Trails.Count).ToList()));

                        }
                        else
                        {
                            itemInDb.ScanCompleted = true;
                            Console.WriteLine("Did not find any new trails");
                        }

                        db.SaveChanges();

                        if (itemInDb.ScanCompleted.Value)
                        {
                            Console.WriteLine($"Scanner for flight {flightId} has been cancelled");
                            tokenSource.Cancel();
                        }
                       
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            await Task.Delay(new TimeSpan(flightScanDelayHours, flightcanDelayMinutes, flightScanDelaySeconds)); 
            FlightScanCallBack(objectArray);


        }

        private async static void AreaScanCallback(Object o)
        {
            Console.WriteLine("Running area scan: " + DateTime.Now);

            var pairs = (Dictionary<string, CancellationTokenSource>)o;
            var createScannersList = new List<string>();
            var areaData = GetRadarArea();

            if (!string.IsNullOrEmpty(areaData))
            {
                var flightIds = GetFlightIds(areaData);
                for (int i = 0; i < flightIds.Count; i++)
                {
                    Console.WriteLine($"Area scan of flight {flightIds[i]} - {i} out of {flightIds.Count - 1}");
                    using (var db = new FlightDbContext())
                    {
                        db.Database.EnsureCreated();

                        var itemInDb = db.FlightIdentifications.Where(fId => fId.FlightIdentifier.Equals(flightIds[i])).FirstOrDefault();

                        if (itemInDb == null)
                        {
                            Console.WriteLine("Flight is new!");

                            db.Flights.Add(Json.Flight.ToModel(GetFlight(flightIds[i])));
                            await Task.Delay(rateDelay);
                            createScannersList.Add(flightIds[i]);
                            db.SaveChanges();
                        }
                        else
                        {
                            if (!db.Flights.Where(fx => fx.Identification.Id == itemInDb.Id).FirstOrDefault().ScanCompleted.Value && !pairs.ContainsKey(flightIds[i]))
                            {
                                createScannersList.Add(flightIds[i]);
                            }
                            Console.WriteLine("Flight found!");

                        }

                    }

                }

                foreach (var item in createScannersList)
                {
                    if (!pairs.ContainsKey(item))
                    {
                        Console.WriteLine($"Creating scanner for {item}");
                        pairs.Add(item, CreateFlightScanner(item));
                    }


                    await Task.Delay(rateDelay);
                }
            }
            else
            {
                Console.WriteLine("No area data found");
            }



        }



        private static string GetRadarArea(string bounds = "78.618%2C39.928%2C-121.739%2C-37.101")
        {
            string result = string.Empty;

            try
            {
                using HttpClient client = new HttpClient();
                using HttpResponseMessage response = client.GetAsync($"{radarAreaUrl}&bounds={bounds}").Result;
                using HttpContent content = response.Content;
                result = content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return result;
        }

        private static Json.Flight GetFlight(string id)
        {
            var data = GetFlightData(id);

            var result = Json.Flight.FromJson(data);

            if(result == null || result.Trail == null)
            {
                Console.WriteLine($"Error getting data from flight {id}; pasting data below");
                Console.WriteLine(data);
            }

            return result;
        }

        private static string GetFlightData(string id)
        {
            string result = string.Empty;

            try
            {
                using HttpClient client = new HttpClient();
                using HttpResponseMessage response = client.GetAsync($"{flightUrl}&flight={id}").Result;
                using HttpContent content = response.Content;
                result = content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return result;


        }

        private static List<string> GetFlightIds(string jsonRadarArea)
        {
            var result = new List<string>();

            foreach (var item in Json.RadarArea.FromJson(jsonRadarArea).ToList())
            {

                if (item.Value.AnythingArray != null && item.Value.AnythingArray.Count == 19)
                {
                    result.Add(item.Key);
                }
            }


            return result;
        }



    }
}
