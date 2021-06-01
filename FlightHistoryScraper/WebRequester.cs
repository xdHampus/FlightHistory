
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

        private const int baseRateDelay = 250;
        private const int rateDelayIncrementer = 2;
        private const int maxDelayIncrements = 16;

        private HttpClient client = new();
        private DateTime lastQuery = DateTime.Now;
        private int currentDelayIncrement = rateDelayIncrementer;


        private static readonly Lazy<WebRequester>
            lazy =
            new Lazy<WebRequester>
                (() => new WebRequester());



        public static WebRequester Instance { get { return lazy.Value; } }
	
        private WebRequester()
        {
        }


        public async Task<string> GetHTMLAsync(string url)
        {
            string result = string.Empty;
            await RateLimit();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
				if(response.IsSuccessStatusCode){
					result = await response.Content.ReadAsStringAsync();
                    resetDelay();
                }
                else
                {
                    increaseDelay();
                    Console.WriteLine($"Unable to get HTML, setting delay to {currentDelayMs()}ms."); ;
                    return GetHTMLAsync(url).Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            lastQuery = DateTime.Now;
            return result;
        }





        private async Task RateLimit()
        {

            int ms = (int)(DateTime.Now - lastQuery).TotalMilliseconds;
            int currentDelay = currentDelayMs();

            if (ms < currentDelay && ms > 0)
            {
                await Task.Delay(currentDelay - ms);
            }
        }


        private int currentDelayMs()
        {
            return baseRateDelay * currentDelayIncrement;
        }

        private void increaseDelay()
        {
            if (rateDelayIncrementer + currentDelayIncrement < maxDelayIncrements)
            {
                currentDelayIncrement += rateDelayIncrementer;
            }
        }
        private void decreaseDelay()
        {
            if (currentDelayIncrement - rateDelayIncrementer > rateDelayIncrementer)
            {
                currentDelayIncrement -= rateDelayIncrementer;
            }
        }
        private void resetDelay()
        {
            currentDelayIncrement = rateDelayIncrementer;
        }

    }
}
