using FlightHistoryCore;
using FlightHistoryCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FlightHistoryScraper
{
    class Program
    {
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

            bool doNotReadKeys = args.Length > 0;

            while (true)
            {
                if (doNotReadKeys && Console.ReadKey().Key != ConsoleKey.Escape)
                {

                    var nextScan = Math.Floor((DateTime.Now - startTime).TotalSeconds % new TimeSpan(areaScanDelayHours,
                                                                                                     areaScanDelayMinutes,
                                                                                                     areaScanDelaySeconds).TotalSeconds)
                        .ToString();
                    var timeSpanNext = new TimeSpan(areaScanDelayHours, areaScanDelayMinutes, areaScanDelaySeconds) - new TimeSpan(0, 0, int.Parse(nextScan));

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
                        var res = await GetFlight(flightId);

                        if (res == null || res.Trail != null && res.Trail.Count > itemInDb.Trails.Count)
                        {
                            Console.WriteLine($"Found {res.Trail.Count - itemInDb.Trails.Count} new trails");

                            itemInDb.Trails.AddRange(res.Trail
                                .Skip(itemInDb.Trails.Count)
                                .ToList()
                                .ConvertAll(t => t.Convert()));
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
            var webRequester = WebRequester.Instance;

            var areaData = await webRequester.GetRadarArea();

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
                            db.Flights.Add((await GetFlight(flightIds[i])).Convert());
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

                }
            }
            else
            {
                Console.WriteLine("No area data found");
            }



        }





        private static async Task<Json.Flight> GetFlight(string id)
        {

            var webRequester = WebRequester.Instance;
            var data = await webRequester.GetFlightData(id);

            var result = Json.Flight.FromJson(data);

            if(result == null || result.Trail == null)
            {
                Console.WriteLine($"Error getting data from flight {id}; pasting data below");
                Console.WriteLine(data);
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
