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
            
			/*
            Timer areaScan = new Timer(AreaScanCallback,
                                       pairs,
                                       new TimeSpan(0, 0, 0),
                                       new TimeSpan(areaScanDelayHours, areaScanDelayMinutes, areaScanDelaySeconds));
            */
            AreaScanCallback(pairs);

            bool doNotReadKeys = args.Length <= 0;

            if (doNotReadKeys)
            {


            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Escape)
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
            else
            {
                while(true) { }
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
                	Console.WriteLine("Getting db con...");
                    db.Database.EnsureCreated();
					Console.WriteLine(" Db con received...");
                    var itemInDb = db.Flights.Include("Trails").Where(fx => fx.Identification.FlightIdentifier == flightId).FirstOrDefault();
					Console.WriteLine(" Fetched db content...");
                    if (itemInDb != null)
                    {
                    	Console.WriteLine("Fetching flight");
                        var res = await GetFlight(flightId);
						Console.WriteLine("Flight fetched");
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
                            Console.WriteLine("Did not find any new trails");
                            itemInDb.ScanCompleted = true;
                        }
						Console.WriteLine("Saving changes");
                        db.SaveChanges();
                        if (itemInDb.ScanCompleted.Value)
                        {
                            Console.WriteLine($"Scanner for flight {flightId} has been cancelled");
                            tokenSource.Cancel();
                        }
                       
                    } else {
                    	Console.WriteLine("-- item in db is null");
                    	tokenSource.Cancel();
                    }
                }
            }
            catch (Exception e)
            {
            	Console.WriteLine("  FAILURE");
                Console.WriteLine(e.Message);
            }
			Console.WriteLine(" Flight scanned properly");

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

            try {
            	
            if (!string.IsNullOrEmpty(areaData))
            {
                var flightIds = GetFlightIds(areaData);
                for (int i = 0; i < flightIds.Count; i++)
                {
                    Console.WriteLine($"Area scan of flight {flightIds[i]} - {i} out of {flightIds.Count - 1}");
					if(pairs.ContainsKey(flightIds[i])){
						Console.WriteLine("Flight already in scanner, not scanning.");
						continue;
					}
                    
                    using (var db = new FlightDbContext())
                    {
                        db.Database.EnsureCreated();

                        var itemInDb = db.FlightIdentifications
                        		.Where(fId => fId.FlightIdentifier.Equals(flightIds[i]))
                        		.FirstOrDefault();

                        if (itemInDb == null)
                        {
                            Console.WriteLine("Flight is new!");
                            var newFlight = await GetFlight(flightIds[i]);
                            if(newFlight != null) {
                            	db.Flights.Add(newFlight.Convert());
                            	createScannersList.Add(flightIds[i]);
                            	db.SaveChanges();
                            } else {
                            	Console.WriteLine("		Catastrophic failure getting flight");
                            }                     

                        }
                        else
                        {
                            if (!db.Flights
                            	.Where(fx => fx.Identification.Id == itemInDb.Id)
                            	.FirstOrDefault()
                            	.ScanCompleted.Value && !pairs.ContainsKey(flightIds[i]))
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
            } catch (Exception e){
            	Console.WriteLine("AREA exception");
            	Console.WriteLine(e.Message);
            }
            Console.WriteLine("DELAY");
            await Task.Delay(new TimeSpan(areaScanDelayHours, areaScanDelayMinutes, areaScanDelaySeconds)); 
            Console.WriteLine("DELAY END");
            AreaScanCallback(o);
			

        }





        private static async Task<Json.Flight> GetFlight(string id)
        {	
			Console.WriteLine(" Getting WR instance");
            var webRequester = WebRequester.Instance;
            Console.WriteLine(" Getting flight data async");
            var data = await webRequester.GetFlightData(id);
            Console.WriteLine(" Getting parsing flight data");
            var result = Json.Flight.FromJson(data);
			Console.WriteLine("Returning");

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

                if (item.Value.AnythingArray != null && item.Value.AnythingArray.Count == 19) //why is this 19?
                {
                    result.Add(item.Key);
                }
            }


            return result;
        }



    }
}
