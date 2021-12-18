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

        static async Task Main(string[] args)
        {
            Console.WriteLine("Initiating scraper.");
            Scraper scraper = new();
            Console.WriteLine("Starting scraper.");
            scraper.StartScraper();

        }


    }
}
