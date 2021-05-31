using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FlightHistoryScraper
{
  
    public sealed class WebRequester
    {
    
        private static readonly Lazy<WebRequester>
            lazy =
            new Lazy<WebRequester>
                (() => new WebRequester());

        private const int rateDelay = 1000;
        private const int rateDelayMultiplier = 3;
		private const int maxDelayMultiplier = 18;

        
        private const string radarAreaUrl = "https://data-live.flightradar24.com/zones/fcgi/feed.js?faa=1&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        private const string flightUrl = "https://data-live.flightradar24.com/clickhandler/?version=1.5";
		private const string dateTimeFormat = "dd MMMM yyy HH:mm:ss";
		private HttpClient client = new HttpClient();

        public static WebRequester Instance { get { return lazy.Value; } }
        private int currentRateDelayM = 1;
		

        private WebRequester()
        {
        }

        private DateTime lastQuery = DateTime.Now;

        private async Task RateLimit()
        {
        
			int targetDelay = currentDelay();
            TimeSpan span = DateTime.Now - lastQuery;
            int ms = (int)span.TotalMilliseconds;

            Console.WriteLine($"	rate limit to {ms}ms");

            lastQuery = DateTime.Now;

            if (ms > 0 && ms < targetDelay)
            {
                int timeToWait = targetDelay - ms;
                if(timeToWait > targetDelay) {
                  timeToWait = targetDelay;
                }
                
				Console.WriteLine($"	actually limiting to {timeToWait}ms");              
                await Task.Delay(timeToWait);

                if(currentRateDelayM > 1){
                	currentRateDelayM /= rateDelayMultiplier;
                }
            }

        }

		private int currentDelay(){
			return rateDelay * rateDelayMultiplier * currentRateDelayM;
		}
		private void increaseMultiplier(){
			if(currentRateDelayM < maxDelayMultiplier){
				currentRateDelayM *= rateDelayMultiplier;
			}
		}        

        public async Task<string> GetRadarArea(string bounds = "78.618%2C39.928%2C-121.739%2C-37.101")
        {
            string result = string.Empty;

            await RateLimit();

            try
            {
                HttpResponseMessage response = await client.GetAsync($"{radarAreaUrl}&bounds={bounds}");
                response.EnsureSuccessStatusCode();

				if(response.IsSuccessStatusCode){
					result = await response.Content.ReadAsStringAsync();
                	Console.WriteLine($"	read areadata {DateTime.Now.ToString(dateTimeFormat)}");
					currentRateDelayM = 1;
				}
				else if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) {
					Console.WriteLine($"	too many requests {DateTime.Now.ToString(dateTimeFormat)}");
					currentRateDelayM *= rateDelayMultiplier;
					result = await GetRadarArea(bounds);
				} else {
					Console.WriteLine($"	error {response.StatusCode} at {DateTime.Now.ToString(dateTimeFormat)}");
				}
			

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return result;
        }

        public async Task<string> GetFlightData(string id)
        {
            string result = string.Empty;

			Console.WriteLine($"	starting await for {id} at {DateTime.Now.ToString(dateTimeFormat)}");
            await RateLimit();
			Console.WriteLine($"	await finished at {DateTime.Now.ToString(dateTimeFormat)}");

            try
            {
                HttpResponseMessage response = await client.GetAsync($"{flightUrl}&flight={id}");
                
				if(response.IsSuccessStatusCode){
					result = await response.Content.ReadAsStringAsync();
                	Console.WriteLine($"	read areadata {DateTime.Now.ToString(dateTimeFormat)}");
                	currentRateDelayM = 1;
				}
				else if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) {
					Console.WriteLine($"	too many requests {DateTime.Now.ToString(dateTimeFormat)}");
					currentRateDelayM *= rateDelayMultiplier;
					result = await GetFlightData(id);
				} else {
					Console.WriteLine($"	error {response.StatusCode} at {DateTime.Now.ToString(dateTimeFormat)}");
				}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

		    Console.WriteLine($"	Returning at {DateTime.Now.ToString(dateTimeFormat)}");
            return result;


        }

    }

}
