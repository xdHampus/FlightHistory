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

              private const int rateDelay = 3000;
        private const string radarAreaUrl = "https://data-live.flightradar24.com/zones/fcgi/feed.js?faa=1&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        private const string flightUrl = "https://data-live.flightradar24.com/clickhandler/?version=1.5";


        public static WebRequester Instance { get { return lazy.Value; } }

        private WebRequester()
        {
        }

        private DateTime lastQuery = DateTime.Now;

        private async Task RateLimit()
        {

            TimeSpan span = DateTime.Now - lastQuery;
            int ms = (int)span.TotalMilliseconds;

            Console.WriteLine(ms);

            lastQuery = DateTime.Now;

            if (ms > 0 && ms < rateDelay)
            {
                int timeToWait = rateDelay - ms;
                if(timeToWait > rateDelay) {
                  timeToWait = rateDelay;
                }
              
                await Task.Delay(rateDelay - ms);
            }

        }

        public async Task<string> GetRadarArea(string bounds = "78.618%2C39.928%2C-121.739%2C-37.101")
        {
            string result = string.Empty;

            await RateLimit();

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

        public async Task<string> GetFlightData(string id)
        {
            string result = string.Empty;

            await RateLimit();


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

    }

}
